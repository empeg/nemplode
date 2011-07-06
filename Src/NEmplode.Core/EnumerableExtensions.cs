using System;
using System.Collections.Generic;

namespace NEmplode.Core
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

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
                yield return x.Current;
        }

        public static int Compare(IEnumerable<string> xs, IEnumerable<string> ys, StringComparer comparer)
        {
            return Compare(xs, ys, comparer.Compare);
        }

        public static int Compare<T>(IEnumerable<T> xs, IEnumerable<T> ys, Comparer<T> comparer)
        {
            return Compare(xs, ys, comparer.Compare);
        }

        public static int Compare<T>(IEnumerable<T> xs, IEnumerable<T> ys, Comparison<T> comparison)
        {
            using (IEnumerator<T> xe = xs.GetEnumerator())
            using (IEnumerator<T> ye = ys.GetEnumerator())
            {
                for (; ; )
                {
                    bool x = xe.MoveNext();
                    bool y = ye.MoveNext();

                    // We've run out; they're equal.
                    if (!x && !y)
                        return 0;

                    // Has one of them run out before the other?
                    if (!x)
                        return -1;
                    if (!y)
                        return 1;

                    int result = comparison(xe.Current, ye.Current);
                    if (result != 0)
                        return result;
                }
            }
        }
    }
}
