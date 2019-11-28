namespace Pyro.Console
{
    using System;
    using System.Linq;
    using System.Text;
    using Language;
    using Network;
    using ExecutionContext;
    using Con = System.Console;

    /// <summary>
    /// A Repl console for Pyro.
    /// 
    /// Can connect and enter into other consoles.
    /// </summary>
    internal class Program
        : AppCommon.AppCommonBase
    {
        // Port that we listen on for incoming connections.
        private const int ListenPort = 7777;

        // our peer that can connect to any other peer, and can also be connected to by any other peer
        private IPeer _peer;
        
        // only used for translation - not execution. Execution is performed by servers or clients.
        private readonly Context _context;

        public static void Main(string[] args)
            => new Program(args).Repl();

        private Program(string[] args)
            : base(args)
        {
            _context = new Context { Language = ELanguage.Rho };
            RegisterTypes.Register(_context.Registry);

            if (!StartPeer(args))
                Exit(1);

            SetupPeer();

            RunInitialisationScripts();
        }

        private void SetupPeer()
        {
            _peer.OnConnected += OnConnected;
            _peer.OnReceivedRequest += (client, text) => WriteLine(text, ConsoleColor.Magenta);
        }

        private bool StartPeer(string[] args)
        {
            var port = ListenPort;
            if (args.Length == 1 && !int.TryParse(args[0], out port))
                return Error($"Local server listen port number expected as argument, got {args[0]}");

            _peer = Create.NewPeer(port);
            var ctx = _peer.Local.Context;
            var reg = ctx.Registry;
            var scope = ctx.Executor.Scope;
            
            reg.Register(new ClassBuilder<TestClient>(reg).Class);
            scope["remote"] = new TestClient();
            
            return _peer.SelfHost() || Error("Failed to start local server");
        }

        private void RunInitialisationScripts()
        {
            // TODO: run things like ~/.pyrorc.{pi,rho}
        }

        private void Repl()
        {
            while (true)
            {
                try
                {
                    WritePrompt();
                    if (!Execute(GetInput()))
                        continue;

                    WriteDataStack();
                }
                catch (Exception e)
                {
                    Error($"{e.Message}");
                }
            }
        }

        private void WritePrompt()
        {
            // TODO: use a lock to ensure these go on same line.
            Write($"{_peer.Remote?.Socket?.RemoteEndPoint} ", ConsoleColor.DarkGray);
            Write($"{_context.Language}> ", ConsoleColor.Gray);
            Con.ForegroundColor = ConsoleColor.White;
        }

        private string GetInput()
        {
            return Console.ReadLine();
            /* want to trap Ctrl-Keys
            var sb = new StringBuilder();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var ch = Console.ReadKey();
                    if (ch.Key == ConsoleKey.Enter)
                    {
                        return sb.ToString();
                    }
                    
                    if (ch.Modifiers == ConsoleModifiers.Control)
                    {
                        switch (ch.Key)
                        {
                            case ConsoleKey.D:
                                _peer.Leave();
                                continue;
                        }
                    }

                    sb.Append(ch.KeyChar);
                }
            }
            */
        }

        private bool PreProcess(string input)
        {
            // Idea here was to allow execution of arbitrary commands in the current context.
            // This is a wonderfully stupid idea for security reasons, but still.
            // In any case, I've removed it for now.
            //
            // if (!string.IsNullOrEmpty(input))
            // {
            //    if (input.StartsWith("."))
            //    {
            //        //var cmd = input.Substring(1);
            //        var bash = @"C:\Program Files\Git\git-bash.exe";
            //        var proc = new System.Diagnostics.Process();
            //        proc.OutputDataReceived += (sender, args) => WriteLine(args.Data);
            //        ProcessStartInfo si = new ProcessStartInfo();
            //        si.RedirectStandardOutput = true;
            //        si.UseShellExecute = false;
            //        si.CreateNoWindow = true;
            //        si.RedirectStandardOutput = true;
            //        si.FileName = bash;
            //        //si.Arguments = cmd;
            //        proc.StartInfo = si;
            //        var sr =
            //        proc.StandardOutput =
            //        return true;
            //    }
            // }

            switch (input)
            {
                case "?":
                case "help":
                    return ShowHelp();
                case "rho":
                    _context.Language = ELanguage.Rho;
                    return true;
                case "pi":
                    _context.Language = ELanguage.Pi;
                    return true;
                case "leave":
                    _peer.Leave();
                    return true;
            }

            return false;
        }

        /// <summary>
        /// First, we translate the input (which could be any supported language) to Pi.
        /// Then we convert that to Pi text and send to current server.
        /// </summary>
        private bool Execute(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                // get refresh of remote data stack
                _peer.Remote?.Continue(" ");
                return true;
            }

            try
            {
                if (PreProcess(input))
                    return true;

                if (!_context.Translate(input, out var cont))
                    return Error(_context.Error);

                return _peer.Execute(cont.ToText());
            }
            catch (Exception e)
            {
                Error(e.Message);
                _peer.Execute($"Error: {e.Message} {e.InnerException?.Message}");
            }

            return false;
        }

        private void OnConnected(IPeer peer, IClient client)
        {
            WriteLine($"Connected to {client}.");
        }

        public static void WriteDataStackContents(INetCommon client, int max = 50)
        {
            var str = new StringBuilder();
            // Make a copy as it could be changed by another network call while we're iterating over data stack.
            var results = client.Context.Executor.DataStack.ToList();
            var reg = client.Context.Registry;
            var n = Math.Min(max, results.Count - 1);
            foreach (var result in results)
                str.AppendLine($"{n--}: {reg.ToPiScript(result)}");

            WriteLine(str.ToString(), ConsoleColor.Yellow);
        }

        private void WriteDataStack()
        {
            var current = Con.ForegroundColor;
            WriteDataStackContents(_peer.Remote);
            Con.ForegroundColor = current;
        }

        private bool ShowHelp()
        {
            Con.WriteLine(
@"
The prompt shows the current executing context and language.

When you type at the prompt, your text is executed in the current context - which could be local (default) or remote.

Before the prompt is printed, the data-stack of the current server is printed. Operations you perform act on this data-stack. Each connection to a remote server has its own private context.

To connect to a remote node, type:
Rho> peer.Connect(""_hostName"", port) 

To then switch execution context, type:
Rho> peer.Enter(n) 

Press Ctrl-D or type 'leave' to leave current context and return to local context.
Press Ctrl-C to quit.
");
            return true;
        }

        protected override void Shutdown()
        {
            const ConsoleColor color = ConsoleColor.DarkGray;
            Error("Shutting down...", color);
            _peer?.Stop();
            Error("Done", color);
            Con.ForegroundColor = ConsoleColor.White;
            Exit();
        }
    }
}

