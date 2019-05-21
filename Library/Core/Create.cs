﻿namespace Pyro
{
    /// <summary>
    /// Top-level factory for Core
    /// </summary>
    public static class Create
    {
        public static IFactory Factory = new Impl.Factory();

        public static IRegistry NewRegistry()
        {
            return Factory.NewRegistry();
        }
    }
}
