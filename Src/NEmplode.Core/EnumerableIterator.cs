using System.Collections.Generic;

namespace NEmplode.Core
{
    /// <summary>
    /// Converts a .NET-style IEnumerator into an STL-style forward iterator.
    /// </summary>
    /// <typeparam name="T">The type contained in the underlying collection.</typeparam>
    internal class EnumerableIterator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private bool _isValid;

        public EnumerableIterator(IEnumerable<T> enumerable)
        {
            _enumerator = enumerable.GetEnumerator();
            _isValid = _enumerator.MoveNext();
        }

        public bool IsValid
        {
            get { return _isValid; }
        }

        public T Current
        {
            get { return _enumerator.Current; }
        }

        public void MoveNext()
        {
            _isValid = _enumerator.MoveNext();
        }
    }
}