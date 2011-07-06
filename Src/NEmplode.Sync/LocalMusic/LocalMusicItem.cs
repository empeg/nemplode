using System.IO;

namespace NEmplode.Sync.LocalMusic
{
    internal class LocalMusicItem : SynchronizationItem
    {
        private readonly string _relativePath;

        public LocalMusicItem(string relativePath)
        {
            _relativePath = relativePath;
        }

        public override string ToString()
        {
            return _relativePath;
        }

        public override SynchronizationItemKey GetCompareKey()
        {
            return new SynchronizationItemKey(_relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }
    }
}