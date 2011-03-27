using System;
using System.Collections.ObjectModel;
using NEmplode.Model;

namespace NEmplode.EmpegCar.Model
{
    public class EmpegResultFolderItem : IFolderItem
    {
        public EmpegResultFolderItem(IFolderItem parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public IFolderItem Parent { get; private set; }
        public string Name { get; private set; }

        public string SortKey
        {
            get { return Name; }
        }

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
            // Nothing.
        }
    }
}