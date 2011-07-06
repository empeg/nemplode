using System.Collections.Generic;
using System.Linq;

namespace NEmplode.Sync.Empeg
{
    internal class EmpegMusicStore : ISynchronizationStore
    {
        public IEnumerable<SynchronizationItem> GetCurrentItems()
        {
            return Enumerable.Empty<SynchronizationItem>();
        }
    }
}