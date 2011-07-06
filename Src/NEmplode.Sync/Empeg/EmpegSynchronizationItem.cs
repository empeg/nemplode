using NEmplode.EmpegCar.Database;

namespace NEmplode.Sync.Empeg
{
    internal class EmpegSynchronizationItem : SynchronizationItem
    {
        private readonly EmpegCarPlaylists _playlists;
        private readonly DatabaseItem _databaseItem;

        public EmpegSynchronizationItem(EmpegCarPlaylists playlists, DatabaseItem databaseItem)
        {
            _playlists = playlists;
            _databaseItem = databaseItem;
        }

        public override SynchronizationItemKey GetCompareKey()
        {
            return new SynchronizationItemKey(_playlists.GetPathTo(_databaseItem));
        }

        public override string ToString()
        {
            return _databaseItem.ToString();
        }
    }
}