using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Events;

namespace NEmplode.Panes.Result
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
                        folderItem.RealizeChildren();
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
