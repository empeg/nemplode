using System.Collections.ObjectModel;
using NEmplode.Model;

namespace NEmplode.EmpegCar.Model
{
    public class EmpegQueryFolderItem : IFolderItem
    {
        public EmpegQueryFolderItem(IFolderItem parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public IFolderItem Parent { get; private set; }

        public string Name { get; private set; }

        public string Id
        {
            get { return Name; }
        }

        public ObservableCollection<IFolderItem> Children
        {
            get { return new ObservableCollection<IFolderItem>(); }
        }

        public bool CanContainFolders
        {
            get { return true; }
        }

        public void RealizeChildren()
        {
            // Do nothing.
        }
    }
}