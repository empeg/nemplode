using System;
using System.Windows;
using System.Windows.Controls;
using NEmplode.Model;

namespace NEmplode.Panes.Result
{
    public class ResultPaneContentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            IFolderItem folderItem = item as IFolderItem;
            if (folderItem != null)
            {
                // TODO: How do I find the data template that goes with 'item'?
                // For now, I'll just have a view template name attached to the IFolderItem
                // How do I find resources?
                var resourceDictionary = new ResourceDictionary();
                resourceDictionary.Source = new Uri("pack://application:,,,/ResultTemplates.xaml");
                
                var dataTemplate = (DataTemplate)resourceDictionary["DefaultResultView"];
                return dataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}