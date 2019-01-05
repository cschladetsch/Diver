﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Diver;
using Diver.Exec;
using Diver.Impl;
using Diver.Language;

using Con = System.Console;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program(args).Repl();
        }

        string GetVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        private Program(string[] args)
        {
            WriteHeader();

            _registry = new Registry();
            _exec = new Executor(_registry);
            _piTranslator = new PiTranslator(_registry);
            _rhoTranslator = new RhoTranslator(_registry);
            AddTypes(_registry);
        }

        private void WriteHeader()
        {
            Con.ForegroundColor = ConsoleColor.DarkGray;
            Con.WriteLine($"Console {GetVersion()}\n");
        }

        private void AddTypes(IRegistry registry)
        {
        }

        private void Repl()
        {
            while (true)
            {
                try
                {
                    Process();
                }
                catch (Exception e)
                {
                    Error($"{e.Message}");
                }
            }
        }

        private void Process()
        {
            WritePrompt();
            var input = Con.ReadLine();
            if (!_piTranslator.Translate(input))
            {
                Error($"{_piTranslator.Error}");
                return;
            }

            _exec.Continue(_piTranslator.Result());
            WriteDataStack();
        }

        private void WritePrompt()
        {
            Con.ForegroundColor = ConsoleColor.Gray;
            Con.Write(MakePrompt());
            Con.ForegroundColor = ConsoleColor.White;
        }

        void Error(string text)
        {
            Con.ForegroundColor = ConsoleColor.Red;
            Con.WriteLine(text);
        }

        private void WriteDataStack()
        {
            WriteDataStackContents();
        }

        public void WriteDataStackContents(int max = 20)
        {
            Con.ForegroundColor = ConsoleColor.Yellow;
            var str = new StringBuilder();
            var data = _exec.DataStack.ToArray();
            max = Math.Min(data.Length, max);
            for (var n = max - 1; n >= 0; --n)
            {
                var obj = data[n];
                str.AppendLine($"{n}: {Print(obj)}");
            }
            Con.WriteLine(str.ToString());
        }

        private string Print(object obj)
        {
            switch (obj)
            {
                case string str:
                    return $"\"{str}\"";
                case List<object> list:
                    var sb = new StringBuilder();
                    sb.Append('[');
                    var comma = "";
                    foreach (var elem in list)
                    {
                        sb.Append(comma + Print(elem));
                        comma = ", ";
                    }
                    sb.Append(']');
                    return sb.ToString();
            }

            return obj.ToString();
        }

        private string MakePrompt()
        {
            return "pi> ";
        }

        private readonly IRegistry _registry;
        private readonly Executor _exec;
        private readonly PiTranslator _piTranslator;
        private RhoTranslator _rhoTranslator;
    }
}
