using System.Collections.Generic;

namespace NEmplode.Core
{
    public static class EnumerableExtensions
    {
        public delegate bool EqualityComparison<in T>(T x, T y);

        public static IEnumerable<T> CommonPrefix<T>(this IEnumerable<T> xs, IEnumerable<T> ys)
        {
            return CommonPrefix(xs, ys, EqualityComparer<T>.Default.Equals);
        }

        public static IEnumerable<T> CommonPrefix<T>(this IEnumerable<T> xs, IEnumerable<T> ys, EqualityComparison<T> eq)
        {
            IEnumerator<T> x = xs.GetEnumerator();
            IEnumerator<T> y = ys.GetEnumerator();

            while (x.MoveNext() && y.MoveNext() && eq(x.Current, y.Current))
            {
                yield return x.Current;
            }
        }
   }
}
