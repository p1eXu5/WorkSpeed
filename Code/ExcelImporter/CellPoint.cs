using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelImporter
{
    public struct CellPoint
    {
        public readonly int X;
        public readonly int Y;

        public CellPoint (int x, int y)
        {
            X = x;
            Y = y;
        }

        public static CellPoint NegativePoint => new CellPoint (-1, -1);
        public static CellPoint ZeroPoint => new CellPoint (0, 0);

        public override int GetHashCode()
        {
            return X + Y * 101;
        }

        public static bool operator < (CellPoint pointA, CellPoint pointB)
        {
            if (pointA.Y < pointB.Y) {
                return true;
            }

            if (pointA.Y == pointB.Y) {
                if (pointA.X < pointB.X) {
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
            return (pointA.Y == pointB.Y) && (pointA.X == pointB.X);
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
    }
}
