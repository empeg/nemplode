using System.ComponentModel.Composition;
using System.Windows.Controls;
using NEmplode.Behaviors;

namespace NEmplode
{
    [ViewExport(RegionName = "ResultRegion")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ResultPane : UserControl
    {
        public ResultPane()
        {
            InitializeComponent();
        }

        [Import]
        public ResultPaneViewModel ViewModel
        {
            set { DataContext = value; }
            get { return (ResultPaneViewModel)DataContext; }
        }
    }
}
