using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golden2048.Core
{
    public static class Helper
    {
        public static void Compare(this Board board, params int[] values)
        {
            foreach (var cell in board)
            {
                var expected = values[cell.Index];
                var actual = cell.Value;
                if (expected != actual)
                {
                    Assert.Fail($"Posição [{cell.X},{cell.Y}]: Era esperado {expected} mas foi encontrado {actual}");
                }
            }
        }
    }
}
