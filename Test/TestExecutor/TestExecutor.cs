﻿using System.Collections.Generic;

using NUnit.Framework;

using Pyro.Exec;

namespace Pyro.Test
{
    [TestFixture]
    public class TestExecutor
        : TestCommon
    {
        [Test]
        public void TestAddIntegers()
        {
            TestAdd(1,2,3);
        }

        [Test]
        public void TestAddStrings()
        {
            TestAdd("foo", "bar", "foobar");
        }

        public void TestAdd(object a, object b, object sum)
        {
            var code = _Registry.Add(new List<object>());
            var coro = _Registry.Add(new Continuation(code.Value));

            code.Value.Add(a);
            code.Value.Add(b);
            code.Value.Add(EOperation.Plus);

            _Exec.Continue(coro);

            var data = _Exec.DataStack;
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(sum, data.Pop());
        }
    }
}

