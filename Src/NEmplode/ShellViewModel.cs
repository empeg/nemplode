using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;
using NEmplode.Model;

namespace NEmplode
{
    [Export]
    public class ShellViewModel
    {
        [ImportingConstructor]
        public ShellViewModel(IRegionManager regionManager, IFolderPath folderPath)
        {
            folderPath.Changed += (sender, e) =>
                                      {
                                          // TODO: How to figure out view type from folder item type? Who to ask?
                                          // This one is the default -- it simply displays the names in a list view.
                                          // TODO: Apparently, you can navigate to a viewmodel, and the view is sorted out (somehow).
                                          var viewName = "DefaultResultPane";
                                          regionManager.RequestNavigate("ResultRegion", new Uri(viewName, UriKind.Relative));
                                      };
        }
    }
}