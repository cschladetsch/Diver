﻿using System;

namespace Pyro.Impl
{
    /// <inheritdoc />
    /// <summary>
    /// Common implementation behind all type-specific objects in the Dive system.
    /// </summary>
    internal class ConstRefBase
        : IConstRefBase
    {
        public static ConstRefBase None = new ConstRefBase(null, null, Id.None);
        public Id Id => _id;
        public IRegistry Registry { get; internal set; }
        public Type ValueType => BaseValue?.GetType();
        public IClassBase Class { get; internal set; }
        public bool IsConst => true;
        public object BaseValue => _baseValue;

        protected readonly Id _id;
        protected object _baseValue;

        public ConstRefBase(IRegistry reg, IClassBase klass, Id id)
        {
            _id = id;
            Registry = reg;
            Class = klass;
        }

        public ConstRefBase(IRegistry reg, IClassBase @class, Id id, object val)
            : this(reg, @class, id)
        {
            reg.AddConst(val);
        }

        public T Get<T>()
        {
            return (T) _baseValue;
        }
    }
}
