﻿namespace Pyro.Language
{
    using Exec;

    /// <summary>
    /// Converts a string to a Pi Continuation.
    /// </summary>
    public interface ITranslator
        : IProcess
    {
        int TraceLevel { get; set; }

        bool Translate(string text, out Continuation result, EStructure st = EStructure.Program);
    }
}

