using System;

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

            int sourceCount = 0;
            foreach (var sourceItem in sourceItems)
            {
                Console.WriteLine(sourceItem);
                ++sourceCount;
            }

            Console.WriteLine("{0:N0} items found in source.", sourceCount);

            int destinationCount = 0;
            foreach (var destinationItem in destinationItems)
            {
                Console.WriteLine(destinationItem);
                ++destinationCount;
            }

            Console.WriteLine("{0:N0} items found in destination.", destinationCount);

            // TODO: Reconcile the two sets.
        }
    }
}