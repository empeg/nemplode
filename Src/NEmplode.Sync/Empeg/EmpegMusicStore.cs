using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NEmplode.EmpegCar.Database;
using NEmplode.EmpegCar.Database.Sources;

namespace NEmplode.Sync.Empeg
{
    internal class EmpegMusicStore : ISynchronizationStore
    {
        private readonly string _deviceUri;

        public EmpegMusicStore(string deviceUri)
        {
            _deviceUri = deviceUri;
        }

        public IEnumerable<SynchronizationItem> GetCurrentItems()
        {
            Trace.TraceInformation("Getting current items in {0}", _deviceUri);

            Uri baseUri = new Uri(_deviceUri);

            // TODO: Factory that recognises http, ftp and empeg address prefixes (schemes).
            IEmpegCarDatabaseSource source = new HijackDatabaseSource(baseUri);
            var reader = new EmpegCarDatabaseReader(source);
            var database = reader.ReadDatabase();
            var playlists = reader.ReadPlaylists(database.Items);

            return database.Items.Select(x => new EmpegSynchronizationItem(playlists, x));
        }
    }
}