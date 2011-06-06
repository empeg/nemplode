using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using NEmplode.EmpegCar.Database;
using NEmplode.EmpegCar.Database.Sources;
using NEmplode.Model;

namespace NEmplode.EmpegCar.Model
{
    [Export(typeof(IMediaLibrary))]
    public class EmpegMediaLibrary : IMediaLibrary
    {
        private readonly EmpegCarDatabase _database;

        public EmpegMediaLibrary()
        {
            Uri baseUri = new Uri("http://10.0.0.99/");

            var source = new HijackDatabaseSource(baseUri);
            var reader = new EmpegCarDatabaseReader(source);
            _database = reader.ReadDatabase();
        }

        public IEnumerable<IFolderItem> RootFolders
        {
            get
            {
                return new[]
                           {
                               new EmpegQueryFolderItem(null, _database, "Artists", x => x.Artist),
                               new EmpegQueryFolderItem(null, _database, "Albums", x => x.Source),
                               new EmpegQueryFolderItem(null, _database, "Genres", x => x.Genre),
//                               new EmpegQueryFolderItem(null, _database, "Years", x => x.Year),
                           };
            }
        }
    }
}