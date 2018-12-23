using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class TwoDirectionQueue<T>
    {
        private readonly LinkedList< T > _list;

        public TwoDirectionQueue ()
        {
            _list = new LinkedList< T >();
        }

        public void Enqueue ( T elem )
        {
            _list.AddLast( elem );
        }

        public T Dequeue ()
        {
            if ( !_list.Any() ) throw new InvalidOperationException();

            var elem = _list.First;
            _list.Remove( elem );
            _list.AddLast( elem );

            return elem.Value;
        }

        public T Peek ()
        {
            if ( !_list.Any() ) throw new InvalidOperationException();

            return _list.First.Value;
        }

        public void Push ( T elem )
        {
            _list.AddFirst( elem );
        }

        public T Pop ()
        {
            if ( !_list.Any() ) throw new InvalidOperationException();

            var elem = _list.Last;
            _list.Remove( elem );
            _list.AddLast( elem );

            return elem.Value;
        }
    }
}
