using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import
{
    public struct TableRect
    {
        public int Top;
        public int Left;
        public int Bottom;

        /// <summary>
        /// Last column + 1
        /// </summary>
        public int Right;

        public TableRect (int top, int left, int count)
        {
            Top = top;
            Left = left;
            Bottom = Top + count - 1;
            Right = Left + count;
        }
    }
}
