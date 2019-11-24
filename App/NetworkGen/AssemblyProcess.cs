using System.Reflection;

namespace Pyro.NetworkGen
{
    public class AssemblyProcess
    {
        protected static NLog.Logger _Logger => NLog.LogManager.GetCurrentClassLogger();

        protected AssemblyProcess(string asmName)
        {
            var asm = Assembly.LoadFile(asmName);
            foreach (var module in asm.GetModules())
            {
                foreach (var type in module.GetTypes())
                {
                    _Logger.Info(type.FullName);
                }
            }
        }
    }
}
