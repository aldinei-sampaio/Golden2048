using System;
using System.Collections.Generic;
using System.Text;

namespace Golden2048.Core
{
    public class Cell
    {
        public int X { get; }
        public int Y { get; }
        public int Index { get; }
        public int Value { get; set; }

        public Cell(int x, int y, int index)
        {
            X = x;
            Y = y;
            Index = index;
        }
    }
}
