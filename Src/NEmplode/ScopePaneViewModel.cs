using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism;
using NEmplode.Model;

namespace NEmplode
{
    [Export]
    public class ScopePaneViewModel
    {
        [ImportingConstructor]
        public ScopePaneViewModel(IMediaLibrary mediaLibrary)
        {
            // TODO: Use Continuous Linq for this?
            RootFolders = new ObservableCollection<ScopeItemViewModel>();
            RootFolders.AddRange(mediaLibrary.RootFolders.Select(x => new ScopeItemViewModel(null, x)));
        }

        public ObservableCollection<ScopeItemViewModel> RootFolders { get; set; }
    }
}