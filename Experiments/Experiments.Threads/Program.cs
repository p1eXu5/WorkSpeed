using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiments.Threads
{
    public class Program
    {
        private const string P_CONCURRENT_STACK = "ConcurrentStackBenchmark";
        private const string P_MULTIPLE_AWAIT = "MultipleAwait";

        private static Dictionary< string, IProgram > _programs;


        public static void Main ()
        {
            _programs = new Dictionary< string, IProgram > {
                [ P_CONCURRENT_STACK ] = new ConcurrentStackBenchmark(),
                [ P_MULTIPLE_AWAIT ] = new MultipleAwait(),
            };

            _programs[ P_MULTIPLE_AWAIT ].Run();

            Console.ReadKey( true );
        }
    }
}
