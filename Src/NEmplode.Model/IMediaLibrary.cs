using System.Collections.Generic;

namespace NEmplode.Model
{
    public interface IMediaLibrary
    {
        IEnumerable<IFolderItem> RootFolders { get; }
    }
}
