using System;
using System.Collections.Generic;

namespace NEmplode.Model
{
    /// <summary>
    /// Represents the current location.
    /// </summary>
    public interface IFolderPath
    {
        void Set(IFolderItem folderItem);
        IFolderItem Top { get; }

        event EventHandler<FolderPathChangedEventArgs> Changed;

        IEnumerable<IFolderItem> Components { get; }
    }

    public class FolderPathChangedEventArgs : EventArgs
    {
        public IFolderItem Top { get; set; }
    }
}