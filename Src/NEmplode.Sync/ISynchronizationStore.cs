using System.Collections.Generic;

namespace NEmplode.Sync
{
    internal interface ISynchronizationStore
    {
        IEnumerable<SynchronizationItem> GetCurrentItems();
    }
}