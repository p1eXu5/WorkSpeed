using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Experiments.Threads
{
    public class MultipleAwait : IProgram
    {
        public async void Run ()
        {
            Console.WriteLine( "Next: will call MethodAsync [ 1 ]" );
            await Method3Async();

            Console.WriteLine( "After: was called MethodAsync [ 9 ]" );
        }

        private void MethodAsync ()
        {
            Console.WriteLine( "Next: will call Method2Async [ 2 ]" );
            Method2Async();

            Console.WriteLine( "After: was called Method2Async [ 8 ]" );
        }

        private async void Method2Async ()
        {
            Console.WriteLine( "Next: will call Method3Async [ 3 ]" );
            await Method3Async();

            Console.WriteLine( "After: was called Method2Async [ 12 ]" );
        }

        private async Task Method3Async ()
        {
            Console.WriteLine( "Next: will run Foo as thread [ 4 ]" );
            var task = Task.Run( () => Foo() );
            task.Wait();

            Console.WriteLine( "After: was invoked Foo [ 10 ]" );
        }

        private async void Foo ()
        {
            Console.WriteLine( "Next: will run Boo as thread [ 5 ]" );
            var task = Task.Run( () => Boo() );
            Console.WriteLine( "After: was invoked Boo [ 6 ]" );

            for ( int i = 0; i < 2; i++ ) {
                Console.WriteLine( " foo" );
            }

            Console.WriteLine( "Next: await Boo task [ 7 ]" );
            await task;
            Console.WriteLine( "After: awaited Boo task [ 11 ]" );
            Console.WriteLine( "Foo end" );
        }

        private void Boo ()
        {
            Thread.Sleep( 5000 );
            for ( int i = 0; i < 2; i++ ) {
                Console.WriteLine( "    boo" );
            }
            Console.WriteLine( "Boo end" );
        }
    }
}
