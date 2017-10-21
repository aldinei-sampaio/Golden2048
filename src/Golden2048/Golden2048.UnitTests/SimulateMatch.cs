using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Golden2048.Core;

namespace Golden2048.UnitTests
{
    [TestClass]
    public class SimulateMatch
    {
        [TestMethod]
        public void Match1()
        {
            var board = new Board();
            board.Compare(
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            board.PutValue(2, 1, 1);
            board.Compare(
                0, 0, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullTop());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullBottom());

            board.PullRight();
            board.PutValue(0, 0, 1);
            board.Compare(
                1, 0, 0, 0,
                0, 0, 0, 1,
                0, 0, 0, 0,
                0, 0, 0, 0
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullTop());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullBottom());

            board.PullTop();
            board.PutValue(2, 2, 1);
            board.Compare(
                1, 0, 0, 1,
                0, 0, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 0
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullTop());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullBottom());

            board.PullLeft();
            board.PutValue(1, 3, 1);
            board.Compare(
                2, 0, 0, 0,
                0, 0, 0, 0,
                1, 0, 0, 0,
                0, 1, 0, 0
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullTop());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullBottom());

            board.PullBottom();
            board.PutValue(3, 2, 2);
            board.Compare(
                0, 0, 0, 0,
                0, 0, 0, 0,
                2, 0, 0, 2,
                1, 1, 0, 0
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullTop());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullBottom());

            board.PullRight();
            board.PutValue(0, 1, 1);
            board.PutValue(1, 0, 8);
            board.Compare(
                0, 8, 0, 0,
                1, 0, 0, 0,
                0, 0, 0, 4,
                0, 0, 0, 2
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullTop());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullBottom());

            board.PullLeft();
            board.PutValue(0, 1, 1);
            board.PutValue(0, 0, 8);
            board.Compare(
                8, 0, 0, 0,
                1, 0, 0, 0,
                4, 0, 0, 0,
                2, 0, 0, 0
            );
            Assert.IsFalse(board.CanPullLeft());
            Assert.IsFalse(board.CanPullTop());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsFalse(board.CanPullBottom());

            board.Undo();
            board.Undo();
            board.Compare(
                0, 8, 0, 0,
                1, 0, 0, 0,
                0, 0, 0, 4,
                0, 0, 0, 2
            );
        }

        [TestMethod]
        public void RandomValue()
        {
            var x = 0;
            var y = 0;
            var value = 0;

            var board = new Board();
            board.CellCreated += (sender, e) =>
            {
                x = e.Created.X;
                y = e.Created.Y;
                value = e.Created.Value;
            };

            for (var n = 0; n < 16; n++)
            {
                board.PutRandomValue();
                Assert.IsTrue(value > 0);
                Assert.AreEqual(value, board.GetValue(x, y));
            }
        }        
    }
}
