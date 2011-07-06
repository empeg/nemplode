using System;
using NEmplode.Core;

namespace NEmplode.Sync
{
    internal class SynchronizationClient
    {
        private readonly ISynchronizationStore _sourceStore;
        private readonly ISynchronizationStore _destinationStore;

        public SynchronizationClient(ISynchronizationStore sourceStore, ISynchronizationStore destinationStore)
        {
            _sourceStore = sourceStore;
            _destinationStore = destinationStore;
        }

        public void Synchronize()
        {
            // Enumerate the items in the source store.
            var sourceItems = _sourceStore.GetCurrentItems();
            var destinationItems = _destinationStore.GetCurrentItems();

            //int sourceCount = 0;
            //foreach (var sourceItem in sourceItems)
            //{
            //    Console.WriteLine(sourceItem);
            //    ++sourceCount;
            //}

            //Console.WriteLine("{0:N0} items found in source.", sourceCount);

            int destinationCount = 0;
            foreach (var destinationItem in destinationItems)
            {
                Console.WriteLine(destinationItem);
                ++destinationCount;
            }

            Console.WriteLine("{0:N0} items found in destination.", destinationCount);

            var mapping = new LocalToEmpegMapping();
            OrderedReconcile.Reconcile(
                sourceItems,
                destinationItems,
                mapping.Compare,
                sourceOnly => { Console.WriteLine("Source: {0}", sourceOnly); },
                destinationOnly => { Console.WriteLine("Destination: {0}", destinationOnly); },
                (source, destination) => { Console.WriteLine("Both: {0} {1}", source, destination); });

            // TODO: Reconcile the two sets.
        }
    }

    internal class LocalToEmpegMapping
    {
        public int Compare(SynchronizationItem source, SynchronizationItem destination)
        {
            var sourceKey = source.GetCompareKey();
            var destinationKey = destination.GetCompareKey();

            return sourceKey.CompareTo(destinationKey);
        }
    }
}