using System;
using System.Collections.Generic;
using System.Linq;

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
            {
                yield return x.Current;
            }
        }
   }
    public static class DescendantExtensions
    {
        public static IEnumerable<TItem> EnumerateDescendants<TItem, TKey>(TItem rootItem, Func<TItem, TKey> keySelector, Func<TItem, bool> hasChildren, Func<TItem, IEnumerable<TItem>> getChildren)
        {
            return DescendantsImpl(new[] { rootItem }, hasChildren, getChildren);
        }

        private static IEnumerable<TItem> DescendantsImpl<TItem>(IEnumerable<TItem> source, Func<TItem, bool> hasChildren, Func<TItem, IEnumerable<TItem>> getChildren)
        {
            var stack = new Stack<IEnumerator<TItem>>();

            try
            {
                IEnumerator<TItem> e = null;
                for (; ; )
                {
                    if (e == null)
                        e = source.GetEnumerator();

                    try
                    {
                        while (e.MoveNext())
                        {
                            var current = e.Current;
                            yield return current;

                            if (hasChildren(current))
                            {
                                stack.Push(e);

                                source = getChildren(current);
                                e = null;

                                break;
                            }
                        }
                    }
                    finally
                    {
                        if (e != null)
                            e.Dispose();
                    }

                    if (e == null)
                        continue;

                    if (stack.Any())
                        e = stack.Pop();
                    else
                        break;
                }
            }
            finally
            {
                while (stack.Any())
                    stack.Pop().Dispose();
            }
        }
    }
}
