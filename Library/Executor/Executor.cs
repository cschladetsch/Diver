﻿using System;
using System.Collections.Generic;

namespace Pyro.Exec
{
    /// <summary>
    /// Processes a sequence of Continuations.
    /// </summary>
    public partial class Executor
        : Reflected<Executor>
    {
        public Stack<object> DataStack => _data;
        public Stack<Continuation> ContextStack => _context;
        public string SourceFilename;
        public int NumOps => _numOps;
        public bool Rethrows { get; set; }

        public Executor()
        {
            AddOperations();
        }

        public void PushContext(Continuation continuation)
        {
            _context.Push(continuation);
        }

        public void Continue()
        {
            Continue(_context.Pop());
        }

        public void Clear()
        {
            _data = new Stack<object>();
            _context = new Stack<Continuation>();
            _break = false;
            _current = null;
            _numOps = 0;
        }

        void Assert()
        {
            //WriteLine("--- Asserting ---");
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
            Continue(continuation.Value);
        }

        public void Continue(Continuation continuation)
        {
            _current = continuation;
            _break = false;

            while (true)
            {
                Execute(_current);
                _break = false;
                if (_context.Count > 0)
                {
                    _current = _context.Pop();
                    _current.Enter(this);
                }
                else
                    break;
            }
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
                            Write($"{op} -->");
                            WriteDataStack();
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!string.IsNullOrEmpty(SourceFilename))
                        WriteLine($"While executing {SourceFilename}:");
                    if (TraceLevel > 10)
                    {
                        WriteLine(DebugWrite());
                        WriteLine($"Exception: {e}");
                    }
                    throw;
                }

                if (_break)
                    break;
            }
        }

        private void Perform(object next)
        {
            ++_numOps;
            if (next == null)
                throw new NullValueException();

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
                {
                    item = Resolve(next);
                    throw new UnknownIdentifierException(next);
                }
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
                return _registry.GetClass(label.Text) ?? ResolveContextually(label);

            if (identBase is Pathname path)
                return ResolvePath(path);

            throw new CannotResolve($"{identBase}");
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
            foreach (var cont in _context)
            {
                if (cont.HasScopeObject(ident))
                    return cont.Scope[ident];
            }

            if (Scope.TryGetValue(ident, out var obj))
                return obj;

            return null;
        }

        private Continuation Context()
        {
            return _current;
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
            var next = RPop();
            switch (next)
            {
                case ICallable call:
                    call.Invoke(_registry, DataStack);
                    break;
                case IClassBase @class:
                    Push(@class.NewInstance(DataStack));
                    break;
                default:
                    _context.Push(next);
                    break;
            }
            Break();
        }

        /// <summary>
        /// Stop the current continuation and resume whatever is on the context stack
        /// </summary>
        private void Break()
        {
            _break = true;
        }

        public void Push(object obj)
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

        public dynamic Pop()
        {
            if (_data.Count == 0)
                throw new DataStackEmptyException();

            var pop = _data.Pop();
            return !(pop is IRefBase data) ? pop : data.BaseValue;
        }

        private bool _break;
        private Stack<object> _data = new Stack<object>();
        private Continuation _current;
        private Stack<Continuation> _context = new Stack<Continuation>();
        private readonly Dictionary<EOperation, Action> _actions = new Dictionary<EOperation, Action>();
        private IRegistry _registry => Self.Registry;
        private int _numOps;
    }
}
