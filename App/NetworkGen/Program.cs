using System;

namespace Pyro.NetworkGen
{
    public enum EBuildType
    {
        Proxy,
        Agent,
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: NetworkGen {Proxy,Agent} inputDir outputDir");
                return;
            }

            var type = Enum.Parse(typeof(EBuildType), args[0]);
            var inputAsm = args[1];
            var outputDir = args[2];

            switch (type)
            {
                case EBuildType.Agent:
                    BuildAgent(inputAsm, outputDir);
                    break;

                case EBuildType.Proxy:
                    BuildProxy(inputAsm, outputDir);
                    break;
            }
        }

        private static void BuildProxy(string inputAsm, string outputDir)
        {
            throw new NotImplementedException();
        }

        private static void BuildAgent(string inputAsm, string outputDir)
        {
            throw new NotImplementedException();
        }
    }
}
