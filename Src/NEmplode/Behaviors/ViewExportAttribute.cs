using System;
using System.ComponentModel.Composition;

namespace NEmplode.Behaviors
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [MetadataAttribute]
    public class ViewExportAttribute : ExportAttribute, IViewRegionRegistration
    {
        public ViewExportAttribute()
            : base(typeof(object))
        {
        }

        public ViewExportAttribute(string viewName)
            : base(viewName, typeof(object))
        {
        }

        public string RegionName { get; set; }
    }
}