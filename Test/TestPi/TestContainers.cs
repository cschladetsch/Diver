﻿using System.Collections.Generic;
using NUnit.Framework;

namespace Diver.Test
{
    [TestFixture]
    public class TestContainers : TestCommon
    {
        [Test]
        public void TestMap()
        {
            var makeMap = "1 2 \"foo\" \"bar\" 2 tomap";
            Run(makeMap);

            TestMapContents();
            AssertEmpty();

            // expand map to the stack
            Run(makeMap + " expand");
            _exec.WriteDataStack(10);
            AssertTop(2);
            Assert.AreEqual(4, DataStack.Count);
            DataStack.Clear();

            // remake map from stack contents
            Run(makeMap + " expand tomap");
            TestMapContents();
            AssertEmpty();
        }

        private void TestMapContents()
        {
            var map = Pop<Dictionary<object, object>>();
            Assert.IsTrue(map.ContainsKey(1));
            Assert.IsTrue(map.ContainsKey("foo"));
            Assert.IsTrue(map.ContainsValue(2));
            Assert.AreEqual("bar", map["foo"]);
        }

        [Test]
        public void TestArray()
        {
            Run("[] [] == assert");

            Run("[1 2 3]");
            TestListContents();

            Run("1 2 3 3 tolist");
            TestListContents();

            Run("1 2 3 3 tolist expand tolist");
            TestListContents();
        }

        private void TestListContents()
        {
            var list = Pop<List<object>>();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }
    }
}