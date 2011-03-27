using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism;
using NEmplode.Model;

namespace NEmplode.Panes.Scope
{
    [Export]
    public class ScopePaneViewModel
    {
        [ImportingConstructor]
        public ScopePaneViewModel(IMediaLibrary mediaLibrary, IFolderPath folderPath)
        {
            RootFolders = new ObservableCollection<ScopeItemViewModel>();
            RootFolders.AddRange(mediaLibrary.RootFolders.Select(x => new ScopeItemViewModel(null, x, folderPath)));

            if (RootFolders.Count != 0)
            {
                RootFolders[0].IsSelected = true;
                RootFolders[0].IsExpanded = true;
            }
        }

        public ObservableCollection<ScopeItemViewModel> RootFolders { get; set; }
    }
}