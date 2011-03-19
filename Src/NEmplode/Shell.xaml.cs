using System.ComponentModel.Composition;
using System.Windows;

namespace NEmplode
{
    [Export]
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
        }
    }
}
