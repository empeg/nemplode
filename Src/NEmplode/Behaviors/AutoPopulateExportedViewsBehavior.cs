using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;

namespace NEmplode.Behaviors
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class AutoPopulateExportedViewsBehavior : RegionBehavior, IPartImportsSatisfiedNotification
    {
        protected override void OnAttach()
        {
            AddRegisteredViews();
        }

        public void OnImportsSatisfied()
        {
            AddRegisteredViews();
        }

        private void AddRegisteredViews()
        {
            if (Region == null)
                return;

            foreach (var registeredView in RegisteredViews)
            {
                if (registeredView.Metadata.RegionName == Region.Name)
                {
                    var view = registeredView.Value;
                    if (!Region.Views.Contains(view))
                        Region.Add(view);
                }
            }
        }

        [ImportMany(AllowRecomposition = true)]
        public Lazy<object, IViewRegionRegistration>[] RegisteredViews { get; set; }
    }
}