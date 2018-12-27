﻿using System;
using System.Collections.Generic;

namespace Diver.Exec
{
    /// <summary>
    /// Processes a sequence of Continuations.
    /// </summary>
    public partial class Executor
    {
        public Stack<object> DataStack => _data;
        public Stack<IRef<Continuation>> ContextStack => _context;
        public string SourceFilename;

        public Executor()
        {
            AddOperations();
        }

        public void Clear()
        {
            _data = new Stack<object>();
            _context = new Stack<IRef<Continuation>>();
            _break = false;
            _current = null;
        }

        void Assert()
        {
            if (!Pop<bool>())
                throw new AssertionFailedException();
        }

        private dynamic RPop()
        {
            return Resolve(Pop());
        }

        private dynamic RPop<T>()
        {
            return ResolvePop<T>();
        }

        private dynamic ResolvePop<T>()
        {
            return Resolve(Pop<T>());
        }

        private void DebugBreak()
        {
            throw new DebugBreakException();
        }

        private void StoreValue()
        {
            var name = Pop<IdentBase>();
            var val = Pop();
            if (name is Label label)
            {
                Context().SetScopeObject(label.Text, val);
                return;
            }

            throw new NotImplementedException();
        }

        private void GetValue()
        {
            var label = Pop<string>();
            var fromScope = Context().FromScope(label);
            Push(fromScope);
        }

        public void Continue(IRef<Continuation> continuation)
        {
            _current = continuation;
            Execute(_current.Value);
            _break = false;
            _current = null;
        }

        private void Execute(Continuation cont)
        {
            while (cont.Next(out var next))
            {
                // unbox reference types
                if (next is IRefBase refBase)
                    next = refBase.BaseValue;

                try
                {
                    Perform(next);
                    
                    if (TraceLevel > 5)
                    {
                        if (next is EOperation op)
                        {
                            Write("leads to-->");
                            WriteDataStack();
                        }
                    }
                }
                catch (Exception e)
                {
                    WriteLine(e);
                    if (!string.IsNullOrEmpty(SourceFilename))
                        WriteLine($"While executing {SourceFilename}:");
                    WriteLine(DebugWrite());
                    throw;
                }

                if (_break)
                    break;
            }
        }

        private void Perform(object next)
        {
            PerformPrelude(next);
            if (next is EOperation op)
            {
                if (_actions.TryGetValue(op, out var action))
                    action();
                else
                    throw new NotImplementedException($"Operation {op}");
            }
            else
            {
                var item = Resolve(next);
                if (item == null)
                    throw new UnknownIdentifierException(next);
                _data.Push(item);
            }
        }

        private object Resolve(object next)
        {
            if (next == null)
                return null;
            if (!(next is IdentBase ident))
                return next;
            return ident.Quoted ? ident : Resolve(ident);
        }

        private object Resolve(IdentBase identBase)
        {
            if (identBase is Label label)
                return ResolveContextually(label);

            if (identBase is Pathname path)
                return ResolvePath(path);

            throw new NotImplementedException($"Cannot resolve {identBase}");
        }

        private object ResolvePath(Pathname path)
        {
            throw new NotImplementedException();
        }

        private object ResolveContextually(Label label)
        {
            var current = Context();
            var ident = label.Text;
            if (current.HasScopeObject(ident))
                return current.Scope[ident];
            foreach (var contRef in _context)
            {
                var cont = contRef.Value;
                if (cont.HasScopeObject(ident))
                    return cont.Scope[ident];
            }

            return null;
        }

        private Continuation Context()
        {
            return _current.Value;
        }

        /// <summary>
        /// Perform a continuation, then return to current context
        /// </summary>
        private void Suspend()
        {
            _context.Push(_current);
            Resume();
        }

        /// <summary>
        /// Resume the continuation that spawned the current one
        /// </summary>
        private void Resume()
        {
            _context.Push(Pop());
            Break();
        }

        /// <summary>
        /// Stop the current continuation and resume whatever is on the context stack
        /// </summary>
        private void Break()
        {
            _break = true;
        }

        private void Push(object obj)
        {
            if (obj == null)
                throw new NullValueException();
            _data.Push(obj);
        }

        public T Pop<T>()
        {
            if (_data.Count == 0)
                throw new DataStackEmptyException();
            var top = Pop();
            if (top == null)
                throw new NullValueException();
            if (top is T val)
                return val;
            if (!(top is IRef<T> data))
                throw new TypeMismatchError(typeof(T), top.GetType());
            return data.Value;
        }

        private dynamic Pop()
        {
            if (_data.Count == 0)
                throw new DataStackEmptyException();

            var pop = _data.Pop();
            return !(pop is IRefBase data) ? pop : data.BaseValue;
        }

        private bool _break;
        private Stack<object> _data = new Stack<object>();
        private IRef<Continuation> _current;
        private Stack<IRef<Continuation>> _context = new Stack<IRef<Continuation>>();
        private readonly Dictionary<EOperation, Action> _actions = new Dictionary<EOperation, Action>();
    }
}
