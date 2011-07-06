using System;
using NEmplode.Core;

namespace NEmplode.Sync
{
    internal abstract class SynchronizationItem
    {
        public abstract SynchronizationItemKey GetCompareKey();
    }

    internal class SynchronizationItemKey
    {
        private readonly string[] _keyComponents;

        public SynchronizationItemKey(string[] keyComponents)
        {
            _keyComponents = keyComponents;
        }

        public int CompareTo(SynchronizationItemKey destinationKey)
        {
            return EnumerableExtensions.Compare(_keyComponents, destinationKey._keyComponents, StringComparer.OrdinalIgnoreCase);
        }
    }
}