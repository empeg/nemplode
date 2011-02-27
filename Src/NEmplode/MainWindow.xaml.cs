using System.Collections.ObjectModel;
using System.Windows;

namespace NEmplode
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel viewModel = new MainWindowViewModel();
            DataContext = viewModel;
        }
    }

    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            MediaLibraries = new ObservableCollection<MediaLibrary>();
        }

        public ObservableCollection<MediaLibrary> MediaLibraries { get; private set; }
    }

    public class MediaLibrary
    {
    }
}
