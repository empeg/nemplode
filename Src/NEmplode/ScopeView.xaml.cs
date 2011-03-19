using System.ComponentModel.Composition;
using System.Windows.Controls;
using NEmplode.Behaviors;

namespace NEmplode
{
    [ViewExport(RegionName = "ScopeRegion")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ScopeView : UserControl
    {
        public ScopeView()
        {
            InitializeComponent();
        }
    }
}
