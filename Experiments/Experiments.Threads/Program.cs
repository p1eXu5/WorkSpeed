using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiments.Threads
{
    public class Program
    {
        public static async Task Main ()
        {
            Console.WriteLine( "Hello World!" );

            var program = new Program();
            await program.MethodAsync();

            Console.WriteLine( "End" );
            Console.ReadKey( true );
        }

        private Task MethodAsync ()
        {
            return Task.Run( () => Foo() );
        }

        private async void Foo ()
        {
            var task = Method2Async();

            for ( int i = 0; i < 240; i++ ) {
                Console.WriteLine( " foo" );
            }

            await task;
            Console.WriteLine( "Foo end" );
        }

        private Task Method2Async ()
        {
            return Task.Run( () => Boo() );
        }

        private void Boo ()
        {
            for ( int i = 0; i < 240; i++ ) {
                Console.WriteLine( "    boo" );
            }
            Console.WriteLine( "Boo end" );
        }
    }
}
