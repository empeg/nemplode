using System.ComponentModel.Composition;
using System.Windows.Controls;
using NEmplode.Behaviors;

namespace NEmplode
{
    [ViewExport(RegionName = "ResultRegion")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ResultView : UserControl
    {
        public ResultView()
        {
            InitializeComponent();
        }
    }
}
