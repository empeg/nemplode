using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism;
using NEmplode.Core;

namespace NEmplode.Model
{
    [Export(typeof(IFolderPath))]
    public class FolderPath : IFolderPath, INotifyPropertyChanged
    {
        private readonly ObservableCollection<IFolderItem> _components = new ObservableCollection<IFolderItem>();

        public void Set(IFolderItem folderItem)
        {
            var path = folderItem.GetPath();
            var prefix = _components.CommonPrefix(path, (x, y) => x.Id == y.Id).ToArray();

            // If there's no change, don't bother.
            if (path.Count == prefix.Length && prefix.Length == _components.Count)
                return;

            // Back up _components until we get to the prefix
            while (_components.Count > prefix.Length)
                _components.RemoveAt(_components.Count - 1);

            _components.AddRange(path.Skip(prefix.Length));

            if (Changed != null)
                Changed(this, new FolderPathChangedEventArgs { Top = Top });

            OnPropertyChanged("Top");
        }

        public IFolderItem Top
        {
            get { return _components.LastOrDefault(); }
        }

        public event EventHandler<FolderPathChangedEventArgs> Changed;

        public IEnumerable<IFolderItem> Components
        {
            get { return _components; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return String.Join("\\", Components);
        }
    }
}