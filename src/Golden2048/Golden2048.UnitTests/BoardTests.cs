using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Golden2048.Core;

namespace Golden2048.UnitTests
{
    [DeploymentItem("System.ValueTuple.dll")]
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void Initialize()
        {
            var board = new Board();
            board.Initialize(
                0002, 0004, 0008, 0016,
                0032, 0064, 0128, 0256,
                0512, 1024, 2048, 0000,
                0002, 0004, 0008, 0016
            );

            board.Compare(
                0002, 0004, 0008, 0016,
                0032, 0064, 0128, 0256,
                0512, 1024, 2048, 0000,
                0002, 0004, 0008, 0016
            );
        }

        [TestMethod]
        public void PullLeft()
        {
            var board = new Board();
            board.Initialize(
                01, 01, 00, 00,
                00, 02, 02, 01,
                03, 00, 03, 02,
                02, 01, 02, 02
            );

            Assert.IsTrue(board.CanPullLeft());
            board.PullLeft();

            board.Compare(
                02, 00, 00, 00,
                04, 01, 00, 00,
                06, 02, 00, 00,
                02, 01, 04, 00
            );

            board.Initialize(
                1, 0, 0, 0,
                0, 2, 0, 0,
                0, 0, 4, 0,
                0, 0, 0, 8
            );

            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());

            board.PullLeft();
            board.Compare(
                1, 0, 0, 0,
                2, 0, 0, 0,
                4, 0, 0, 0,
                8, 0, 0, 0
            );

            Assert.IsFalse(board.CanPullLeft());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsFalse(board.CanPullUp());
            Assert.IsFalse(board.CanPullDown());
        }

        [TestMethod]
        public void PullFullBoard()
        {
            var values = new int[]
            {
                01, 02, 03, 04,
                02, 03, 04, 05,
                01, 02, 01, 02,
                02, 01, 03, 05
            };

            var board = new Board();
            board.Initialize(values);

            Assert.IsFalse(board.CanPullLeft());
            board.PullLeft();
            board.Compare(values);

            Assert.IsFalse(board.CanPullRight());
            board.PullRight();
            board.Compare(values);

            Assert.IsFalse(board.CanPullUp());
            board.PullUp();
            board.Compare(values);

            Assert.IsFalse(board.CanPullDown());
            board.PullDown();
            board.Compare(values);
        }

        [TestMethod]
        public void PullEmptyBoard()
        { 
            var values = new int[]
            {
                00, 00, 00, 00,
                00, 00, 00, 00,
                00, 00, 00, 00,
                00, 00, 00, 00
            };

            var board = new Board();
            board.Initialize(values);

            Assert.IsFalse(board.CanPullLeft());
            board.PullLeft();
            board.Compare(values);

            Assert.IsFalse(board.CanPullRight());
            board.PullRight();
            board.Compare(values);

            Assert.IsFalse(board.CanPullUp());
            board.PullUp();
            board.Compare(values);

            Assert.IsFalse(board.CanPullDown());
            board.PullDown();
            board.Compare(values);
        }

        [TestMethod]
        public void PullRight()
        {
            var board = new Board();
            board.Initialize(
                01, 01, 00, 00,
                00, 02, 02, 01,
                03, 00, 03, 02,
                02, 01, 02, 02
            );

            Assert.IsTrue(board.CanPullRight());
            board.PullRight();

            board.Compare(
                00, 00, 00, 02,
                00, 00, 04, 01,
                00, 00, 06, 02,
                00, 02, 01, 04
            );

            board.Initialize(
                1, 0, 0, 0,
                0, 2, 0, 0,
                0, 0, 4, 0,
                0, 0, 0, 8
            );

            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());

            board.PullRight();
            board.Compare(
                0, 0, 0, 1,
                0, 0, 0, 2,
                0, 0, 0, 4,
                0, 0, 0, 8
            );

            Assert.IsTrue(board.CanPullLeft());
            Assert.IsFalse(board.CanPullUp());
            Assert.IsFalse(board.CanPullRight());
            Assert.IsFalse(board.CanPullDown());

        }

        [TestMethod]
        public void PullTop()
        {
            var board = new Board();
            board.Initialize(
                01, 01, 01, 02,
                00, 01, 01, 02,
                03, 01, 01, 01,
                02, 01, 02, 01
            );

            Assert.IsTrue(board.CanPullUp());
            board.PullUp();

            board.Compare(
                01, 02, 02, 04,
                03, 02, 01, 02,
                02, 00, 02, 00,
                00, 00, 00, 00
            );

            board.Initialize(
                1, 0, 0, 0,
                0, 2, 0, 0,
                0, 0, 4, 0,
                0, 0, 0, 8
            );

            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());

            board.PullUp();
            board.Compare(
                1, 2, 4, 8,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            Assert.IsFalse(board.CanPullLeft());
            Assert.IsFalse(board.CanPullUp());
            Assert.IsFalse(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());
        }

        [TestMethod]
        public void PullBottom()
        {
            var board = new Board();
            board.Initialize(
                01, 01, 01, 02,
                00, 01, 01, 02,
                03, 01, 01, 01,
                02, 01, 02, 01
            );

            Assert.IsTrue(board.CanPullDown());
            board.PullDown();

            board.Compare(
                00, 00, 00, 00,
                01, 00, 01, 00,
                03, 02, 02, 04,
                02, 02, 02, 02
            );
            
            board.Initialize(
                1, 0, 0, 0,
                0, 2, 0, 0,
                0, 0, 4, 0,
                0, 0, 0, 8
            );

            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());

            board.PullDown();
            board.Compare(
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                1, 2, 4, 8
            );

            Assert.IsFalse(board.CanPullLeft());
            Assert.IsTrue(board.CanPullUp());
            Assert.IsFalse(board.CanPullRight());
            Assert.IsFalse(board.CanPullDown());
        }

        [TestMethod]
        public void Bug1()
        {
            var board = new Board();
            board.Initialize(
                8, 4, 0, 0,
                2, 0, 0, 0,
                0, 0, 0, 0,
                0, 2, 0, 0
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());
        }
    }
}
