using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NEmplode.IO;

namespace NEmplode.Sync.LocalMusic
{
    internal class LocalMusicStore : ISynchronizationStore
    {
        private readonly DirectoryInfo _rootDirectory;

        public LocalMusicStore(string rootPath)
        {
            _rootDirectory = new DirectoryInfo(rootPath);
        }

        public IEnumerable<SynchronizationItem> GetCurrentItems()
        {
            Trace.TraceInformation("Getting current items in {0}", _rootDirectory);

            return _rootDirectory.EnumerateFileSystemEntries(x => x.FullName)
                .Where(x => x.Extension == ".mp3")
                .Select(x => new LocalMusicItem(_rootDirectory.GetRelativePath(x)));
        }
    }
}