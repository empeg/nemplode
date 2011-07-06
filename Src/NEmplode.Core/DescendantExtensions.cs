using System;
using System.Collections.Generic;
using System.Linq;

namespace NEmplode.Core
{
    public static class DescendantExtensions
    {
        public static IEnumerable<TItem> EnumerateDescendants<TItem>(TItem rootItem, Func<TItem, bool> hasChildren, Func<TItem, IEnumerable<TItem>> getChildren)
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

                            // TODO: Accumulate parents here as well.
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