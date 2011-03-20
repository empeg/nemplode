using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.Prism;
using NEmplode.Model;

namespace NEmplode
{
    public class FakeFolderItem : IFolderItem
    {
        public FakeFolderItem(string name)
        {
            Name = name;

            Children = new ObservableCollection<IFolderItem>();
        }

        public IFolderItem Parent
        {
            get { return null; }
        }

        public string Name { get; private set; }

        public string Id
        {
            get { return Name; }
        }

        public ObservableCollection<IFolderItem> Children { get; private set; }

        public bool CanContainFolders
        {
            get { return true; }
        }

        public void RealizeChildren()
        {
            Children.AddRange(
                Enumerable.Range(1, 10)
                    .Select(i => new FakeFolderItem(string.Format("Folder {0}", i)))
                    .Cast<IFolderItem>());
        }

        public override string ToString()
        {
            return Name;
        }
    }
}