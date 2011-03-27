using System.Collections.Generic;
using System.ComponentModel.Composition;
using NEmplode.Model;

namespace NEmplode.EmpegCar.Model
{
    [Export(typeof(IMediaLibrary))]
    public class EmpegMediaLibrary : IMediaLibrary
    {
        public EmpegMediaLibrary()
        {
        }

        public IEnumerable<IFolderItem> RootFolders
        {
            get
            {
                return new[]
                           {
                               new EmpegQueryFolderItem(null, "Artists"),
                               new EmpegQueryFolderItem(null, "Albums"),
                               new EmpegQueryFolderItem(null, "Genres"),
                               new EmpegQueryFolderItem(null, "Years"),
                           };
            }
        }
    }
}