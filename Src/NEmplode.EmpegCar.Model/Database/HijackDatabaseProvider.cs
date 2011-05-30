using System;
using System.Net;

namespace NEmplode.EmpegCar.Model.Database
{
    internal class HijackDatabaseProvider : IEmpegDatabaseProvider
    {
        private readonly Uri _empegUri;

        public HijackDatabaseProvider(Uri empegUri)
        {
            _empegUri = empegUri;
        }

        public byte[] DownloadDatabase()
        {
            // TODO: If this file doesn't exist, try the /empeg/var/database file. In fact, we should probably figure out what version of the player you're using. Interesting problem.
            WebClient client = new WebClient();
            return client.DownloadData(_empegUri + "/empeg/var/database3");
        }

        public byte[] DownloadTags()
        {
            WebClient client = new WebClient();
            return client.DownloadData(_empegUri + "/empeg/var/tags");
        }

        public byte[] DownloadPlaylists()
        {
            WebClient client = new WebClient();
            return client.DownloadData(_empegUri + "/empeg/var/playlists");
        }
    }
}