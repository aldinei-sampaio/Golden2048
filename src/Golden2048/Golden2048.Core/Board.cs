using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Golden2048.Core
{
    public class Board : IEnumerable<Cell>
    {
        private const int sizeX = 4;
        private const int sizeY = 4;
        private const int maxX = 3;
        private const int maxY = 3;

        private Cell[,] cellBoard = new Cell[sizeX, sizeY];
        private List<Cell> cellList = new List<Cell>();
        private Random rnd = new Random();
        private DropOutStack<List<int>> undo = new DropOutStack<List<int>>(3);
        
        public int MoveCount { get; private set; }

        public Board()
        {
            var n = 0;
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    var cell = new Cell(x, y, n);
                    cellBoard[x, y] = cell;
                    cellList.Add(cell);
                    n++;
                }
            }
        }

        public void Reset()
        {
            cellList.ForEach(i => i.Value = 0);
            undo.Clear();
        }

        public void Initialize(params int[] values)
        {
            foreach(var cell in cellList)
            {
                if (cell.Index >= values.Length) return;
                cell.Value = values[cell.Index];
            }
        }

        public bool CanPullLeft()
        {
            for (var y = 0; y < sizeY; y++)
            {
                var value = cellBoard[0, y].Value;
                var lastNonZero = value;
                for (var x = 1; x < sizeX; x++)
                {
                    var newValue = cellBoard[x, y].Value;
                    if (newValue > 0)
                    {
                        if (value == 0 || lastNonZero == newValue) return true;
                        lastNonZero = newValue;
                    }
                    value = newValue;
                }
            }
            return false;
        }

        public bool CanPullRight()
        {
            for (var y = 0; y < sizeY; y++)
            {
                var value = cellBoard[maxX, y].Value;
                var lastNonZero = value;
                for (var x = maxX - 1; x >= 0; x--)
                {
                    var newValue = cellBoard[x, y].Value;
                    if (newValue > 0)
                    {
                        if (value == 0 || lastNonZero == newValue) return true;
                        lastNonZero = newValue;
                    }
                    value = newValue;
                }
            }
            return false;
        }

        public bool CanPullTop()
        {
            for (var x = 0; x < sizeX; x++)
            {
                var value = cellBoard[x, 0].Value;
                var lastNonZero = value;
                for (var y = 1; y < sizeY; y++)
                {
                    var newValue = cellBoard[x, y].Value;
                    if (newValue > 0)
                    {
                        if (value == 0 || lastNonZero == newValue) return true;
                        lastNonZero = newValue;
                    }
                    value = newValue;
                }
            }
            return false;
        }

        public bool CanPullBottom()
        {
            for (var x = 0; x < sizeX; x++)
            {
                var value = cellBoard[x, maxY].Value;
                var lastNonZero = value;
                for (var y = maxY - 1; y >= 0; y--)
                {
                    var newValue = cellBoard[x, y].Value;
                    if (newValue > 0)
                    {
                        if (value == 0 || lastNonZero == newValue) return true;
                        lastNonZero = newValue;
                    }
                    value = newValue;
                }
            }
            return false;
        }

        private void SaveUndo()
        {
            var values = cellList.Select(i => i.Value).ToList();
            undo.Push(values);
        }

        public void Undo()
        {
            var value = undo.Pop();
            if (value == null) return;
            for (var n = 0; n < value.Count; n++) cellList[n].Value = value[n];
        }

        public void PullLeft()
        {
            SaveUndo();
            for (var y = 0; y < sizeY; y++)
            {
                var values = new List<int>();
                for (var x = 0; x < sizeX; x++)
                {
                    var value = cellBoard[x, y].Value;
                    if (value > 0) values.Add(value);
                }

                TruncateList(values);

                for (var x = 0; x < sizeX; x++)
                {
                    if (x < values.Count)
                    {
                        cellBoard[x, y].Value = values[x];
                    }
                    else
                    {
                        cellBoard[x, y].Value = 0;
                    }
                }
            }
        }

        private static void TruncateList(List<int> values)
        {
            for (var n = 0; n < values.Count - 1; n++)
            {
                if (values[n] == values[n + 1])
                {
                    values[n] *= 2;
                    values.RemoveAt(n + 1);
                }
            }
        }

        public void PullRight()
        {
            SaveUndo();
            for (var y = 0; y < sizeY; y++)
            {
                var values = new List<int>();
                for (var x = maxX; x >= 0; x--)
                {
                    var value = cellBoard[x, y].Value;
                    if (value > 0) values.Add(value);
                }

                TruncateList(values);

                var n = 0;
                for (var x = maxX; x >= 0; x--)
                {
                    if (n < values.Count)
                    {
                        cellBoard[x, y].Value = values[n];
                        n++;
                    }
                    else
                    {
                        cellBoard[x, y].Value = 0;
                    }
                }
            }
        }

        public void PullTop()
        {
            SaveUndo();
            for (var x = 0; x < sizeX; x++)
            {
                var values = new List<int>();
                for (var y = 0; y < sizeY; y++)
                {
                    var value = cellBoard[x, y].Value;
                    if (value > 0) values.Add(value);
                }

                TruncateList(values);

                for (var y = 0; y < sizeY; y++)
                {
                    if (y < values.Count)
                    {
                        cellBoard[x, y].Value = values[y];
                    }
                    else
                    {
                        cellBoard[x, y].Value = 0;
                    }
                }
            }
        }

        public void PullBottom()
        {
            SaveUndo();
            for (var x = 0; x < sizeX; x++)
            {
                var values = new List<int>();
                for (var y = maxY; y >= 0; y--)
                {
                    var value = cellBoard[x, y].Value;
                    if (value > 0) values.Add(value);
                }

                TruncateList(values);

                var n = 0;
                for (var y = maxY; y >= 0; y--)
                {
                    if (n < values.Count)
                    {
                        cellBoard[x, y].Value = values[n];
                        n++;
                    }
                    else
                    {
                        cellBoard[x, y].Value = 0;
                    }
                }
            }
        }

        public void PutValue(int x, int y, int value)
        {
            cellBoard[x, y].Value = value;
            SaveUndo();
        }

        public int Length => cellList.Count;

        public bool PutRandomValue()
        {
            MoveCount++;
            var cells = cellList.Where(i => i.Value == 0).ToList();
            if (cells.Count == 0) return false;

            var index = rnd.Next(0, cells.Count);
            if (MoveCount < 20)
            {
                cells[index].Value = 2;
            }
            else
            {
                var dice = rnd.Next(0, 10);
                cells[index].Value = dice == 0 ? 4 : 2;
            }
            return true;
        }

        public IEnumerator<Cell> GetEnumerator() => cellList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => cellList.GetEnumerator();
    }
}
