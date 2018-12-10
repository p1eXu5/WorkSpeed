using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;

namespace NpoiExcel
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

        public TableRect (int value)
        {
            Top = value;
            Left = value;
            Bottom = Top + value - 1;
            Right = Left + value;
        }
    }
}
