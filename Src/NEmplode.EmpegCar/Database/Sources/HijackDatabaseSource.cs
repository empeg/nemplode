using System;
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
            // TODO: If this file doesn't exist, try the /empeg/var/database file. In fact, we should probably figure out what version of the player you're using. Interesting problem.
            WebClient client = new WebClient();
            return client.DownloadData(_baseUri + "/empeg/var/database3");
        }

        public byte[] DownloadTags()
        {
            WebClient client = new WebClient();
            return client.DownloadData(_baseUri + "/empeg/var/tags");
        }

        public byte[] DownloadPlaylists()
        {
            WebClient client = new WebClient();
            return client.DownloadData(_baseUri + "/empeg/var/playlists");
        }
    }
}
