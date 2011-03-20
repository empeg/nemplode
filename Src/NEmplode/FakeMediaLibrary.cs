using System.Collections.Generic;
using System.ComponentModel.Composition;
using NEmplode.Model;

namespace NEmplode
{
    [Export(typeof(IMediaLibrary))]
    public class FakeMediaLibrary : IMediaLibrary
    {
        public IEnumerable<IFolderItem> RootFolders
        {
            get
            {
                return new[]
                           {
                               new FakeFolderItem("Playlists"),
                               new FakeFolderItem("Artists"),
                               new FakeFolderItem("Albums"),
                               new FakeFolderItem("Genres"),
                               new FakeFolderItem("Years"),
                               new FakeFolderItem("All Tracks")
                           };
            }
        }
    }
}