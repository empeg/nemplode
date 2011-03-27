using System.ComponentModel.Composition;
using System.Windows.Controls;
using NEmplode.Behaviors;

namespace NEmplode.Panes.Scope
{
    [ViewExport(RegionName = "ScopeRegion")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ScopePane : UserControl
    {
        public ScopePane()
        {
            InitializeComponent();
        }

        [Import]
        public ScopePaneViewModel ViewModel
        {
            set { DataContext = value; }
            get { return (ScopePaneViewModel)DataContext; }
        }
    }
}
