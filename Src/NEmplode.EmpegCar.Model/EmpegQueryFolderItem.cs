using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism;
using NEmplode.EmpegCar.Model.Database;
using NEmplode.Model;

namespace NEmplode.EmpegCar.Model
{
    public class EmpegQueryFolderItem : IFolderItem
    {
        private readonly MediaDatabase _database;
        private readonly Func<ItemBase, string> _selector;
        private readonly ObservableCollection<IFolderItem> _children = new ObservableCollection<IFolderItem>();

        public EmpegQueryFolderItem(IFolderItem parent, MediaDatabase database, string name, Func<ItemBase, string> selector)
        {
            _database = database;
            _selector = selector;

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
            get { return _children; }
        }

        public bool CanContainFolders
        {
            get { return true; }
        }

        public void RealizeChildren()
        {
            var childNames = _database.Items
                .Select(i => _selector(i))
                .Distinct()
                .ToList();

            Children.AddRange(childNames
                .Select(x => new EmpegResultFolderItem(this, x))
                .Cast<IFolderItem>());
        }
    }
}