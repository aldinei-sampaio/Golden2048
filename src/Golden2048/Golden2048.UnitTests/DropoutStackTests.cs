using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Golden2048.Core;

namespace Golden2048.UnitTests
{
    [TestClass]
    public class DropoutStackTests
    {
        [TestMethod]
        public void DropoutTest()
        {
            var stack = new DropOutStack<string>(3);
            Assert.IsNull(stack.Pop());
            stack.Push("A");
            stack.Push("B");
            stack.Push("C");
            Assert.AreEqual("C", stack.Pop());
            Assert.AreEqual("B", stack.Pop());
            Assert.AreEqual("A", stack.Pop());
            Assert.IsNull(stack.Pop());
            stack.Push("A");
            stack.Push("B");
            stack.Push("C");
            Assert.AreEqual("C", stack.Pop());
            Assert.AreEqual("B", stack.Pop());
            Assert.AreEqual("A", stack.Pop());
            Assert.IsNull(stack.Pop());
        }

        [TestMethod]
        public void DropoutTest2()
        {
            var stack = new DropOutStack<string>(3);
            Assert.IsNull(stack.Pop());
            stack.Push("A");
            stack.Push("B");
            stack.Push("C");
            stack.Push("D");
            Assert.AreEqual("D", stack.Pop());
            Assert.AreEqual("C", stack.Pop());
            Assert.AreEqual("B", stack.Pop());
            Assert.IsNull(stack.Pop());
            stack.Push("A");
            stack.Push("B");
            stack.Push("C");
            stack.Push("D");
            Assert.AreEqual("D", stack.Pop());
            Assert.AreEqual("C", stack.Pop());
            Assert.AreEqual("B", stack.Pop());
        }
    }
}
