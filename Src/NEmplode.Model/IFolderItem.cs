using System.Collections.ObjectModel;

namespace NEmplode.Model
{
    /// <summary>
    /// IFolderItem represents something that can be contained in a "folder". It's not necessarily a folder itself.
    /// For example, you might have an implementation representing a filesystem folder.
    /// Alternatively, it might be more abstract, such as a user group or a music playlist.
    /// </summary>
    public interface IFolderItem
    {
        /// <summary>
        /// The parent of this item. Can be null.
        /// </summary>
        IFolderItem Parent { get; }

        /// <summary>
        /// Human-readable text for this item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Parseable text for this item. Must be unique relative to its siblings.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Returns the children of this item -- the items contained in this folder.
        /// These may, themselves, also be folders.
        /// If an item *cannot* contain children, then it should throw NotSupportedException.
        /// If an item has problems enumerating its children, then it should throw an appropriate exception,
        /// rather than returning an empty collection.
        /// If an item *does not currently* contain children, then it should return an empty collection.
        /// </summary>
        ObservableCollection<IFolderItem> Children { get; }

        /// <summary>
        /// *Can* this item contain other folders? Usually used to decide whether an item gets a [+] in a tree view.
        /// It's defined as "can ..." to give implementing classes the opportunity to be lazy.
        /// If the implementation can answer the question (does it contain folders) cheaply, then it can do that.
        /// </summary>
        bool CanContainFolders { get; }

        /// <summary>
        /// Populate the Children collection.
        /// </summary>
        void RealizeChildren();
    }
}