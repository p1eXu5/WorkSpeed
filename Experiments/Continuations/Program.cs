using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Continuations
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Task (() => WriteNum (3)).Start();
            //Task.Factory.StartNew (() => WriteNum (4));

            Parallel.For (1, 16, WriteNum);

            Console.ReadKey (true);
        }

        static void WriteNum (int num)
        {
            foreach (var i in SomeClass.GetEnumerable (num)) {
                Console.Write ($"{i} ");
            }
        }
    }

    class CollEvent : EventArgs
    {
        public CollEvent (int i)
        {
            CollCount = i;
        }

        public int CollCount { get; }
    }

    class SomeClass
    {
        public static IEnumerable<int> GetEnumerable (int num)
        {
            for (int i = 0; i < 100; i++) {
                yield return num;
            }
        }
    }
}
