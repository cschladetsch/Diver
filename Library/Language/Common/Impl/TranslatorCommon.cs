﻿using System.Text;
using System.Collections.Generic;

namespace Pyro.Language.Impl
{
    using Exec;

    /// <inheritdoc cref="ITranslator" />
    /// <summary>
    /// Common to all processes that translate an AST to Pi code sequences.
    /// </summary>
    public class TranslatorCommon
        : ProcessCommon
        , ITranslator
    {
        public int TraceLevel { get; set; }

        protected virtual Continuation Result => _stack.Count == 0 ? null : Top();

        private readonly Stack<Continuation> _stack = new Stack<Continuation>();

        protected TranslatorCommon(IRegistry r)
            : base(r)
        {
            Reset();
        }

        public virtual bool Translate(string text, out Continuation cont, EStructure st = EStructure.Program)
        {
            Reset();
            cont = null;
            return !string.IsNullOrEmpty(text) || Fail("Empty input");
        }

        public new void Reset()
        {
            base.Reset();
            _stack.Clear();
            _stack.Push(Continuation.New(_reg));
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var cont in _stack)
                str.Append(cont);

            return str.ToString();
        }

        protected void PushNew()
        {
            _stack.Push(Continuation.New(_reg));
        }

        protected Continuation Pop()
        {
            return _stack.Pop();
        }

        protected Continuation Top()
        {
            return _stack.Peek();
        }

        protected void Append(object obj)
        {
            Top().Code.Add(obj);
        }
    }
}

