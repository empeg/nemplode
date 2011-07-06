using System;
using System.Diagnostics;
using System.Net;

namespace NEmplode.EmpegCar.Database.Sources
{
    public class HijackDatabaseSource : IEmpegCarDatabaseSource
    {
        private readonly Uri _baseUri;

        public HijackDatabaseSource(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public byte[] DownloadDatabase()
        {
            Trace.TraceInformation("Downloading /empeg/var/database3...");

            // TODO: If this file doesn't exist, try the /empeg/var/database file. In fact, we should probably figure out what version of the player you're using. Interesting problem.
            WebClient client = new WebClient();
            return client.DownloadData(_baseUri + "/empeg/var/database3");
        }

        public byte[] DownloadTags()
        {
            Trace.TraceInformation("Downloading /empeg/var/tags...");

            WebClient client = new WebClient();
            return client.DownloadData(_baseUri + "/empeg/var/tags");
        }

        public byte[] DownloadPlaylists()
        {
            Trace.TraceInformation("Downloading /empeg/var/playlists...");

            WebClient client = new WebClient();
            return client.DownloadData(_baseUri + "/empeg/var/playlists");
        }
    }
}
