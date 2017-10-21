using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Golden2048.Core
{
    public struct CellData
    {
        public int X { get; }
        public int Y { get; }
        public int Index { get; }
        public int Value { get; }
        public CellData(int x, int y, int index, int value)
        {
            X = x;
            Y = y;
            Index = index;
            Value = value;
        }
        internal CellData(Cell data) : this(data.X, data.Y, data.Index, data.Value)
        {
        }
    }

    public class CellMovedEventArgs : EventArgs
    {
        public CellData From { get; }
        public CellData To { get; }
        internal CellMovedEventArgs(CellData from, CellData to)
        {
            From = from;
            To = to;
        }
    }

    public class CellMergedEventArgs : EventArgs
    {
        public CellData Destroyed { get; }
        public CellData Merged { get; }
        public CellMergedEventArgs(CellData destroyed, CellData merged)
        {
            Destroyed = destroyed;
            Merged = merged;
        }
    }


    public class CellCreatedEventArgs : EventArgs
    {
        public CellData Created { get; }
        internal CellCreatedEventArgs(CellData created)
        {
            Created = created;
        }
    }

    public class Board : IEnumerable<Cell>
    {
        private const int sizeX = 4;
        private const int sizeY = 4;
        private const int maxX = 3;
        private const int maxY = 3;

        private Cell[,] cellBoard = new Cell[sizeX, sizeY];
        private List<Cell> cellList = new List<Cell>();
        private static Random rnd = new Random();
        private DropOutStack<List<int>> undo = new DropOutStack<List<int>>(3);

        public event EventHandler<CellMovedEventArgs> CellMoved;
        public event EventHandler<CellCreatedEventArgs> CellCreated;
        public event EventHandler<CellMergedEventArgs> CellMerged;

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

        public int GetValue(int x, int y)
        {
            return cellBoard[x, y].Value;
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
                var values = new List<CellData>();
                for (var x = 0; x < sizeX; x++)
                {
                    var cell = cellBoard[x, y];
                    if (cell.Value > 0) values.Add(new CellData(cell));
                }

                TruncateList(values);

                for (var x = 0; x < sizeX; x++)
                {
                    var to = cellBoard[x, y];
                    if (x < values.Count)
                    {
                        var from = values[0];
                        to.Value = from.Value;
                        if (from.X != x)
                        {
                            CellMoved?.Invoke(this, new CellMovedEventArgs(from, new CellData(to)));
                        }
                    }
                    else
                    {
                        to.Value = 0;
                    }
                }
            }
        }

        private void TruncateList(List<CellData> values)
        {
            for (var n = 0; n < values.Count - 1; n++)
            {
                var item = values[n];
                var nextItem = values[n + 1];
                if (item.Value == nextItem.Value)
                {
                    var merged = new CellData(item.X, item.Y, item.Index, item.Value * 2);
                    values[n] = merged;
                    values.RemoveAt(n + 1);
                    CellMerged?.Invoke(this, new CellMergedEventArgs(nextItem, merged));
                }
            }
        }

        public void PullRight()
        {
            SaveUndo();
            for (var y = 0; y < sizeY; y++)
            {
                var values = new List<CellData>();
                for (var x = maxX; x >= 0; x--)
                {
                    var cell = cellBoard[x, y];
                    if (cell.Value > 0) values.Add(new CellData(cell));
                }

                TruncateList(values);

                var n = 0;
                for (var x = maxX; x >= 0; x--)
                {
                    var to = cellBoard[x, y];
                    if (n < values.Count)
                    {
                        var from = values[n];
                        n++;
                        to.Value = from.Value;
                        if (from.X != x)
                        {
                            CellMoved?.Invoke(this, new CellMovedEventArgs(from, new CellData(to)));
                        }
                    }
                    else
                    {
                        to.Value = 0;
                    }
                }
            }
        }

        public void PullTop()
        {
            SaveUndo();
            for (var x = 0; x < sizeX; x++)
            {
                var values = new List<CellData>();
                for (var y = 0; y < sizeY; y++)
                {
                    var cell = cellBoard[x, y];
                    if (cell.Value > 0) values.Add(new CellData(cell));
                }

                TruncateList(values);

                for (var y = 0; y < sizeY; y++)
                {
                    var to = cellBoard[x, y];
                    if (y < values.Count)
                    {
                        var from = values[y];
                        to.Value = from.Value;
                        if (from.Y != y)
                        {
                            CellMoved?.Invoke(this, new CellMovedEventArgs(from, new CellData(to)));
                        }
                    }
                    else
                    {
                        to.Value = 0;
                    }
                }
            }
        }

        public void PullBottom()
        {
            SaveUndo();
            for (var x = 0; x < sizeX; x++)
            {
                var values = new List<CellData>();
                for (var y = maxY; y >= 0; y--)
                {
                    var cell = cellBoard[x, y];
                    if(cell.Value > 0) values.Add(new CellData(cell));
                }

                TruncateList(values);

                var n = 0;
                for (var y = maxY; y >= 0; y--)
                {
                    var to = cellBoard[x, y];
                    if (n < values.Count)
                    {
                        var from = values[n];
                        n++;
                        to.Value = from.Value;
                        if (from.Y != y)
                        {
                            CellMoved?.Invoke(this, new CellMovedEventArgs(from, new CellData(to)));
                        }
                    }
                    else
                    {
                        to.Value = 0;
                    }
                }
            }
        }

        public void PutValue(int x, int y, int value)
        {
            var target = cellBoard[x, y];
            if (target.Value > 0)
            {
                var from = new CellData(target);
                target.Value = value;
                CellMoved?.Invoke(this, new CellMovedEventArgs(from, new CellData(target)));
            }
            else
            {
                target.Value = value;
                CellCreated?.Invoke(this, new CellCreatedEventArgs(new CellData(target)));
            }
            SaveUndo();
        }

        public int Length => cellList.Count;

        public bool PutRandomValue()
        {
            MoveCount++;
            var cells = cellList.Where(i => i.Value == 0).ToList();
            if (cells.Count == 0) return false;

            var index = rnd.Next(0, cells.Count);
            int value;
            if (MoveCount < 20)
            {
                value = 2;
            }
            else
            {
                var dice = rnd.Next(0, 10);
                value = dice == 0 ? 4 : 2;
            }
            var cell = cells[index];
            cell.Value = value;
            CellCreated?.Invoke(this, new CellCreatedEventArgs(new CellData(cell)));
            return true;
        }

        public void Compare(params int[] values)
        {
            foreach (var cell in cellList)
            {
                var expected = values[cell.Index];
                var actual = cell.Value;
                if (expected != actual)
                {
                    throw new Exception($"Posição [{cell.X},{cell.Y}]: Era esperado {expected} mas foi encontrado {actual}");
                }
            }
        }

        public IEnumerator<Cell> GetEnumerator() => cellList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => cellList.GetEnumerator();
    }
}
