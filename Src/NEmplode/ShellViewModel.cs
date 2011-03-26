using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Events;
using NEmplode.Model;

namespace NEmplode
{
    [Export]
    public class ShellViewModel
    {
        [ImportingConstructor]
        public ShellViewModel(IEventAggregator eventAggregator, IFolderPath folderPath)
        {
            folderPath.Changed += (sender, e) => eventAggregator.GetEvent<FolderItemSelectedEvent>().Publish(e.Top);
        }
    }
}