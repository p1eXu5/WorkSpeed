using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiments.Threads
{
    public class ConcurrentStackBenchmark : IProgram
    {
        public void Run ()
        {
            var coll = new List< string >();
            var concColl = new ConcurrentStack< string >();

            var arr = new[] {
                "asd",
                "qwe",
                "sdf",
                "ert",
                "dfg",
                "cvb",
                "tyu",
                "yui",
                "ghj",
                "bnm",
                "zxc",
                "xcv",
                "vbn",
                "bnm",
                "nm,",
                "fgh",
                "hjk",
                "jkl",
                "kl;",
            };

            var rand = new Random();
            int n = 10_000;
            var source = new string[n];

            for ( int i = 0; i < n; i++ ) {
                source[ i ] = arr[ rand.Next( arr.Length ) ];
            }

            var sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach( source, s => { if ( s is string str ) concColl.Push( str ); } );
            sw.Stop();
            Console.WriteLine( sw.Elapsed );
            
            coll.Clear();
            sw.Restart();
            foreach ( var data in source ) {
                if ( data is string str ) coll.Add( str );
            }
            sw.Stop();
            Console.WriteLine( sw.Elapsed );

            Console.WriteLine( "End" );
            Console.ReadKey( true );
        }
    }
}
