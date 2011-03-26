using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Events;

namespace NEmplode
{
    [Export]
    public class ResultPaneViewModel : INotifyPropertyChanged
    {
        private object _content;

        [ImportingConstructor]
        public ResultPaneViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<FolderItemSelectedEvent>().Subscribe(
                folderItem =>
                    {
                        Content = folderItem;
                    });
        }

        public object Content
        {
            get { return _content; }
            set
            {
                if (_content == value)
                    return;

                _content = value;
                OnPropertyChanged("Content");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
