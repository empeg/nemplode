using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using NEmplode.Behaviors;

namespace NEmplode
{
    [ViewExport("DefaultResultPane", RegionName = "ResultRegion")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DefaultResultPane : UserControl, INavigationAware, IRegionMemberLifetime
    {
        public DefaultResultPane()
        {
            InitializeComponent();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // TODO: Something.
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // TODO: Consider re-using views where possible...
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Do nothing.
        }

        public bool KeepAlive
        {
            get { return false; }
        }
    }
}
