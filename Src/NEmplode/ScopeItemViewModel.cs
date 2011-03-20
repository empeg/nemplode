using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ContinuousLinq;
using NEmplode.Model;

namespace NEmplode
{
    public class ScopeItemViewModel : INotifyPropertyChanged
    {
        private static readonly ObservableCollection<ScopeItemViewModel> DeferredChildren =
            new ObservableCollection<ScopeItemViewModel>(new ScopeItemViewModel[] { null });

        private readonly ScopeItemViewModel _parent;
        private readonly IFolderItem _folderItem;
        private readonly IFolderPath _folderPath;

        private bool _isSelected;
        private bool _isExpanded;
        private ICollection<ScopeItemViewModel> _children;
        private readonly ReadOnlyContinuousCollection<ScopeItemViewModel> _realChildren;

        public ScopeItemViewModel(ScopeItemViewModel parent, IFolderItem folderItem, IFolderPath folderPath)
        {
            // parent can be null.
            _parent = parent;

            if (folderItem == null)
                throw new ArgumentNullException("folderItem");

            _folderItem = folderItem;

            if (folderPath == null)
                throw new ArgumentNullException("folderPath");

            _folderPath = folderPath;

            // TODO: Use the WeakPropertyChangedEventHandler (in CLINQ) to bind to some interesting properties on the IFolderItem?
            _realChildren = _folderItem.Children.Select(x => new ScopeItemViewModel(this, x, folderPath));

            // If it can contain folders, but we don't know what they are yet, put a fake child in.
            if (_folderItem.CanContainFolders && _realChildren.Count == 0)
                Children = DeferredChildren;
            else
                Children = _realChildren;
        }

        public string Name
        {
            get { return _folderItem.Name; }
        }

        public ICollection<ScopeItemViewModel> Children
        {
            get { return _children; }
            set
            {
                if (_children == value)
                    return;

                _children = value;
                OnPropertyChanged("Children");
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                OnPropertyChanged("IsSelected");

                if (_isSelected)
                    _folderPath.Set(_folderItem);
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value)
                    return;

                if (value)
                {
                    if (_parent != null)
                        _parent.IsExpanded = true;

                    // TODO: Allow this to happen asynchronously.
                    _folderItem.RealizeChildren();
                    Children = _realChildren;
                }

                _isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}