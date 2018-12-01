using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import
{
    public class ReadOnlyHashSet<T> : IEnumerable<T>
    {
        private readonly HashSet<T> _hashSet;

        public ReadOnlyHashSet()
        {
            _hashSet = new HashSet<T>();
        }

        public ReadOnlyHashSet (IEnumerable<T> elements)
        {
            _hashSet = new HashSet<T>(elements);
        }

        public void Add (T element)
        {
            _hashSet.Add (element);
        }

        public void Clear()
        {
            _hashSet.Clear();
        }

        public bool Contains (T element)
        {
            return _hashSet.Contains (element);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _hashSet.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return _hashSet.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return _hashSet.GetHashCode();
        }
    }
}
