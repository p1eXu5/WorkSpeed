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
            var c = new SomeClass();
            c.CollEvent += (s, e) =>
                           {
                               Console.WriteLine ($"Event {e.CollCount} {Thread.CurrentThread.Name}");
                           };

            Console.WriteLine($"{Thread.CurrentThread.Name = "thread 1"}");

            for (int i = 0; i < 20; i++) {

                Thread.Sleep (1000);
                Console.WriteLine (i);

                if (10 == i) {
                    c.GetDataAsync();
                }
            }

            Console.ReadKey (true);
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
        public event EventHandler<CollEvent> CollEvent;

        public void GetDataAsync()
        {
            Task.Factory.StartNew (GetDataInnerAsync, TaskCreationOptions.LongRunning);
        }

        private void GetDataInnerAsync()
        {
            var coll = new List<int>();
            Console.WriteLine($"{Thread.CurrentThread.Name = "thread 2"}");

            for (int i = 0; i < 10; i++) {
                
                Thread.Sleep (100);
                coll.Add (i);
            }

            coll.ForEach (OnRaise);
        }

        private void OnRaise(int i)
        {
            CollEvent?.Invoke (this, new CollEvent (i));
        }
    }
}
