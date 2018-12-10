using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiExcel
{
    public struct CellPoint
    {
        public readonly short Column;
        public readonly int Row;

        public CellPoint (short column, int row)
        {
            Column = column;
            Row = row;
        }

        public static CellPoint NegativePoint => new CellPoint (-1, -1);
        public static CellPoint ZeroPoint => new CellPoint (0, 0);

        public override int GetHashCode()
        {
            return Column + Row * 101;
        }

        public static bool operator < (CellPoint pointA, CellPoint pointB)
        {
            if (pointA.Row < pointB.Row) {
                return true;
            }

            if (pointA.Row == pointB.Row) {
                if (pointA.Column < pointB.Column) {
                    return true;
                }
            }

            return false;
        }

        public static bool operator > (CellPoint pointA, CellPoint pointB)
        {
            return !(pointA < pointB);
        }

        public static bool operator == (CellPoint pointA, CellPoint pointB)
        {
            return (pointA.Row == pointB.Row) && (pointA.Column == pointB.Column);
        }

        public static bool operator != (CellPoint pointA, CellPoint pointB)
        {
            return !(pointA == pointB);
        }

        public override bool Equals (object obj)
        {
            if (ReferenceEquals (null, obj)) return false;
            return obj is CellPoint other && Equals (other);
        }

        public bool Equals (CellPoint pointB)
        {
            return this == pointB;
        }

        public override string ToString()
        {
            return new StringBuilder().Append("Column: ").Append(Column).Append("; ")
                                      .Append("Row: ").Append(Row).Append(".")
                                      .ToString(); ;
        }
    }
}
