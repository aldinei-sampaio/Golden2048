using System;
using System.Collections.Generic;
using System.Text;

namespace Golden2048.Core
{
    public class DropOutStack<T> where T : class
    {
        private T[] items;
        private int top = 0;
        public DropOutStack(int capacity)
        {
            items = new T[capacity];
        }

        public void Push(T item)
        {
            items[top] = item;
            top = (top + 1) % items.Length;
        }

        public T Pop()
        {
            var next = (items.Length + top - 1) % items.Length;
            var value = items[next];
            if (value == null) return null;
            top = next;
            items[top] = null;
            return value;
        }

        public void Clear()
        {
            for (var n = 0; n < items.Length; n++)
            {
                items[n] = null;
            }
            top = 0;
        }
    }
}
