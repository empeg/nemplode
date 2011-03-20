using System.Collections.Generic;

namespace NEmplode.Model
{
    static class FolderItemExtensions
    {
        public static List<IFolderItem> GetPath(this IFolderItem folderItem)
        {
            var path = new List<IFolderItem>();
            PushAncestors(folderItem, path);
            return path;
        }

        private static void PushAncestors(IFolderItem folderItem, ICollection<IFolderItem> components)
        {
            if (folderItem == null)
                return;

            PushAncestors(folderItem.Parent, components);
            components.Add(folderItem);
        }
    }
}