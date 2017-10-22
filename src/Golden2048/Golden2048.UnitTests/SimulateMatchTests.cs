using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Golden2048.Core;
using System.Collections.Generic;

namespace Golden2048.UnitTests
{
    [TestClass]
    public class SimulateMatchTests
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
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());

            board.PullRight();
            board.PutValue(0, 0, 1);
            board.Compare(
                1, 0, 0, 0,
                0, 0, 0, 1,
                0, 0, 0, 0,
                0, 0, 0, 0
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());

            board.PullUp();
            board.PutValue(2, 2, 1);
            board.Compare(
                1, 0, 0, 1,
                0, 0, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 0
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());

            board.PullLeft();
            board.PutValue(1, 3, 1);
            board.Compare(
                2, 0, 0, 0,
                0, 0, 0, 0,
                1, 0, 0, 0,
                0, 1, 0, 0
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());

            board.PullDown();
            board.PutValue(3, 2, 2);
            board.Compare(
                0, 0, 0, 0,
                0, 0, 0, 0,
                2, 0, 0, 2,
                1, 1, 0, 0
            );
            Assert.IsTrue(board.CanPullLeft());
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());

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
            Assert.IsTrue(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsTrue(board.CanPullDown());

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
            Assert.IsFalse(board.CanPullUp());
            Assert.IsTrue(board.CanPullRight());
            Assert.IsFalse(board.CanPullDown());

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

        [TestMethod]
        public void Match2()
        {
            var board = new Board();
            board.Compare(
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            board.PutValue(2, 1, 2);
            board.Compare(
                0, 0, 0, 0,
                0, 0, 2, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            board.PullUp();
            board.PutValue(2, 1, 2);
            board.Compare(
                0, 0, 2, 0,
                0, 0, 2, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            board.PullLeft();
            board.PutValue(2, 2, 2);
            board.Compare(
                2, 0, 0, 0,
                2, 0, 0, 0,
                0, 0, 2, 0,
                0, 0, 0, 0
            );

            board.PullUp();
            board.PutValue(3, 3, 2);
            board.Compare(
                4, 0, 2, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 2
            );

            board.PullLeft();
            board.PutValue(1, 2, 2);
            board.Compare(
                4, 2, 0, 0,
                0, 0, 0, 0,
                0, 2, 0, 0,
                2, 0, 0, 0
            );

            board.PullUp();
            board.PutValue(2, 0, 2);
            board.Compare(
                4, 4, 2, 0,
                2, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            board.PullLeft();
            board.Compare(
                8, 2, 0, 0,
                2, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );
        }

        [TestMethod]
        public void PullLeftBug()
        {
            var board = new Board();
            board.PutValue(0, 0, 4);
            board.PutValue(2, 0, 2);
            board.PutValue(3, 3, 2);
            board.Compare(
                4, 0, 2, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 2
            );

            board.PullLeft();
            board.Compare(
                4, 2, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                2, 0, 0, 0
            );
        }

        [TestMethod]
        public void PullUpCombo()
        {
            var destroyedList = new List<CellData>();
            var mergedFromList = new List<CellData>();
            var mergedToList = new List<CellData>();

            var board = new Board();
            board.CellMoved += (sender, e) =>
            {
                mergedFromList.Add(e.From);
                mergedToList.Add(e.To);
                destroyedList.Add(e.Merged.Value);
            };

            board.PutValue(0, 0, 2);
            board.PutValue(0, 1, 2);
            board.PutValue(0, 2, 2);
            board.PutValue(0, 3, 2);
            board.Compare(
                2, 0, 0, 0,
                2, 0, 0, 0,
                2, 0, 0, 0,
                2, 0, 0, 0
            );

            board.PullUp();
            board.Compare(
                4, 0, 0, 0,
                4, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            Assert.AreEqual(2, destroyedList.Count);
            Assert.AreEqual(1, destroyedList[0].Y);
            Assert.AreEqual(3, destroyedList[1].Y);
            Assert.AreEqual(2, mergedFromList.Count);
            Assert.AreEqual(0, mergedFromList[0].Y);
            Assert.AreEqual(2, mergedFromList[1].Y);
            Assert.AreEqual(2, mergedToList.Count);
            Assert.AreEqual(0, mergedToList[0].Y);
            Assert.AreEqual(1, mergedToList[1].Y);

            destroyedList.Clear();
            mergedFromList.Clear();
            mergedToList.Clear();

            board.PullUp();
            board.Compare(
                8, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            Assert.AreEqual(1, destroyedList.Count);
            Assert.AreEqual(1, destroyedList[0].Y);
            Assert.AreEqual(1, mergedFromList.Count);
            Assert.AreEqual(0, mergedFromList[0].Y);
            Assert.AreEqual(1, mergedToList.Count);
            Assert.AreEqual(0, mergedToList[0].Y);
        }

        [TestMethod]
        public void PullDownCombo()
        {
            var destroyedList = new List<CellData>();
            var mergedFromList = new List<CellData>();
            var mergedToList = new List<CellData>();

            var board = new Board();
            board.CellMoved += (sender, e) =>
            {
                mergedFromList.Add(e.From);
                mergedToList.Add(e.To);
                destroyedList.Add(e.Merged.Value);
            };

            board.PutValue(0, 0, 2);
            board.PutValue(0, 1, 2);
            board.PutValue(0, 2, 2);
            board.PutValue(0, 3, 2);
            board.Compare(
                2, 0, 0, 0,
                2, 0, 0, 0,
                2, 0, 0, 0,
                2, 0, 0, 0
            );

            board.PullDown();
            board.Compare(
                0, 0, 0, 0,
                0, 0, 0, 0,
                4, 0, 0, 0,
                4, 0, 0, 0
            );

            Assert.AreEqual(2, destroyedList.Count);
            Assert.AreEqual(2, destroyedList[0].Y);
            Assert.AreEqual(0, destroyedList[1].Y);
            Assert.AreEqual(2, mergedFromList.Count);
            Assert.AreEqual(3, mergedFromList[0].Y);
            Assert.AreEqual(1, mergedFromList[1].Y);
            Assert.AreEqual(2, mergedToList.Count);
            Assert.AreEqual(3, mergedToList[0].Y);
            Assert.AreEqual(2, mergedToList[1].Y);

            destroyedList.Clear();
            mergedFromList.Clear();
            mergedToList.Clear();

            board.PullDown();
            board.Compare(
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                8, 0, 0, 0
            );

            Assert.AreEqual(1, destroyedList.Count);
            Assert.AreEqual(2, destroyedList[0].Y);
            Assert.AreEqual(1, mergedFromList.Count);
            Assert.AreEqual(3, mergedFromList[0].Y);
            Assert.AreEqual(1, mergedToList.Count);
            Assert.AreEqual(3, mergedToList[0].Y);
        }

        [TestMethod]
        public void PullLeftCombo()
        {
            var destroyedList = new List<CellData>();
            var mergedFromList = new List<CellData>();
            var mergedToList = new List<CellData>();

            var board = new Board();
            board.CellMoved += (sender, e) =>
            {
                mergedFromList.Add(e.From);
                mergedToList.Add(e.To);
                destroyedList.Add(e.Merged.Value);
            };

            board.PutValue(0, 0, 2);
            board.PutValue(1, 0, 2);
            board.PutValue(2, 0, 2);
            board.PutValue(3, 0, 2);
            board.Compare(
                2, 2, 2, 2,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            board.PullLeft();
            board.Compare(
                4, 4, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            Assert.AreEqual(2, destroyedList.Count);
            Assert.AreEqual(1, destroyedList[0].X);
            Assert.AreEqual(3, destroyedList[1].X);
            Assert.AreEqual(2, mergedFromList.Count);
            Assert.AreEqual(0, mergedFromList[0].X);
            Assert.AreEqual(2, mergedFromList[1].X);
            Assert.AreEqual(2, mergedToList.Count);
            Assert.AreEqual(0, mergedToList[0].X);
            Assert.AreEqual(1, mergedToList[1].X);

            destroyedList.Clear();
            mergedFromList.Clear();
            mergedToList.Clear();

            board.PullLeft();
            board.Compare(
                8, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            Assert.AreEqual(1, destroyedList.Count);
            Assert.AreEqual(1, destroyedList[0].X);
            Assert.AreEqual(1, mergedFromList.Count);
            Assert.AreEqual(0, mergedFromList[0].X);
            Assert.AreEqual(1, mergedToList.Count);
            Assert.AreEqual(0, mergedToList[0].X);
        }

        [TestMethod]
        public void PullRightCombo()
        {
            var destroyedList = new List<CellData>();
            var mergedFromList = new List<CellData>();
            var mergedToList = new List<CellData>();

            var board = new Board();
            board.CellMoved += (sender, e) =>
            {
                mergedFromList.Add(e.From);
                mergedToList.Add(e.To);
                destroyedList.Add(e.Merged.Value);
            };

            board.PutValue(0, 0, 2);
            board.PutValue(1, 0, 2);
            board.PutValue(2, 0, 2);
            board.PutValue(3, 0, 2);
            board.Compare(
                2, 2, 2, 2,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            board.PullRight();
            board.Compare(
                0, 0, 4, 4,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            Assert.AreEqual(2, destroyedList.Count);
            Assert.AreEqual(2, destroyedList[0].X);
            Assert.AreEqual(0, destroyedList[1].X);
            Assert.AreEqual(2, mergedFromList.Count);
            Assert.AreEqual(3, mergedFromList[0].X);
            Assert.AreEqual(1, mergedFromList[1].X);
            Assert.AreEqual(2, mergedToList.Count);
            Assert.AreEqual(3, mergedToList[0].X);
            Assert.AreEqual(2, mergedToList[1].X);

            destroyedList.Clear();
            mergedFromList.Clear();
            mergedToList.Clear();

            board.PullRight();
            board.Compare(
                0, 0, 0, 8,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

            Assert.AreEqual(1, destroyedList.Count);
            Assert.AreEqual(2, destroyedList[0].X);
            Assert.AreEqual(1, mergedFromList.Count);
            Assert.AreEqual(3, mergedFromList[0].X);
            Assert.AreEqual(1, mergedToList.Count);
            Assert.AreEqual(3, mergedToList[0].X);
        }
    }
}
