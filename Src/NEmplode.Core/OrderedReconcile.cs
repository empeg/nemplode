using System;
using System.Collections.Generic;

namespace NEmplode.Core
{
    public static class OrderedReconcile
    {
        /// <summary>
        /// Compare two ordered enumerable sequences, notifying the differences.
        /// This is essentially an implementation of WalkSortedLists, which I wrote in C++ back in 2003.
        /// </summary>
        /// 
        /// <param name="left">The sorted collection of items on the left-hand side of the comparison.</param>
        /// <param name="right">The sorted collection of items on the left-hand side of the comparison.</param>
        /// 
        /// <param name="comparison">An delegate that knows how to compare a left-hand item with a right-hand item.
        /// This is usually the same comparer that enforced the ordering in the first place.</param>
        /// 
        /// <param name="leftOnly">Called when an item is present only on the left-hand side.</param>
        /// <param name="rightOnly">Called when an item is present only on the right-hand side.</param>
        /// <param name="bothSides">Called when an item exists on both sides.</param>
        /// TODO: If this yielded (null, R) and (L, null), it'd be something like OrderedZip.
        public static void Reconcile<TLeft, TRight>(
            IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TRight, int> comparison,
            Action<TLeft> leftOnly,
            Action<TRight> rightOnly,
            Action<TLeft, TRight> bothSides)
        {
            ReconcileHelper(new EnumerableIterator<TLeft>(left), new EnumerableIterator<TRight>(right), comparison, leftOnly, rightOnly, bothSides);
        }

        private static void ReconcileHelper<TLeft, TRight>(
            EnumerableIterator<TLeft> left, EnumerableIterator<TRight> right,
            Func<TLeft, TRight, int> comparison,
            Action<TLeft> leftOnly,
            Action<TRight> rightOnly,
            Action<TLeft, TRight> bothSides)
        {
            while (left.IsValid && right.IsValid)
            {
                // While left < right, the items in left aren't in right
                while (left.IsValid && right.IsValid && comparison(left.Current, right.Current) < 0)
                {
                    leftOnly(left.Current);
                    left.MoveNext();
                }

                // While right < left, the items in right aren't in left
                while (left.IsValid && right.IsValid && comparison(left.Current, right.Current) > 0)
                {
                    rightOnly(right.Current);
                    right.MoveNext();
                }

                // While left == right, the items are in both
                while (left.IsValid && right.IsValid && comparison(left.Current, right.Current) == 0)
                {
                    bothSides(left.Current, right.Current);
                    left.MoveNext();
                    right.MoveNext();
                }
            }

            // Mop up.
            while (left.IsValid)
            {
                leftOnly(left.Current);
                left.MoveNext();
            }

            while (right.IsValid)
            {
                rightOnly(right.Current);
                right.MoveNext();
            }
        }
    }
}