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
    }
}