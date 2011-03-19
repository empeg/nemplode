using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NEmplode.Download
{
    static class Program
    {
        static void Main(string[] args)
        {
            Uri baseUri = new Uri("http://10.0.0.99/");

            WebClient client = new WebClient();

            // TODO: Figure this out from the database version.
            Encoding encoding = Encoding.UTF8;

            // The 'tags' file is ASCII (but we might as well treat it as UTF8), LF line-endings.
            var tagsBytes = client.DownloadData(baseUri + "/empeg/var/tags");
            var tagsString = encoding.GetString(tagsBytes);
            var tags = tagsString.Split('\n');
            foreach (var tag in tags)
            {
                if (!string.IsNullOrWhiteSpace(tag))
                    Console.WriteLine(tag);
            }

            // TODO: If this file doesn't exist, try the /empeg/var/database file. In fact, we should probably figure out what version of the player you're using. Interesting problem.

            var databaseBytes = client.DownloadData(baseUri + "/empeg/var/database3");
            var databaseStream = new MemoryStream(databaseBytes);
            var databaseReader = new BinaryReader(databaseStream, encoding);

            // We can deal with playlists at the same time as we deal with the tunes.
            var playlistsBytes = client.DownloadData(baseUri + "/empeg/var/playlists");

            // Loop over the records.
            var items = new List<ItemBase>();
            int id = 0x0;
            while (databaseReader.BaseStream.Position < databaseReader.BaseStream.Length)
            {
                var dictionary = new Dictionary<string, string>();

                // Loop over the fields.
                for (;;)
                {
                    byte tagIndex = databaseReader.ReadByte();
                    if (tagIndex == 0xFF)
                        break;

                    // TODO: Or we could index the dictionary by tag index and do this later.
                    string tagName = tags[tagIndex];
                    byte tagLength = databaseReader.ReadByte();
                    byte[] tagData = databaseReader.ReadBytes(tagLength);

                    string tagValue = encoding.GetString(tagData);

                    dictionary.Add(tagName, tagValue);

                    // TODO: If we don't find an FF, this item is incorrectly terminated.
                }

                // TODO: Could use reflection and attributes for this, but it'd expose the names outside the reader.
                var setters = new Dictionary<string, Action<ItemBase, string>>();
                setters["type"] = (i, value) => { /* do nothing. */ };
                setters["length"] = (i, value) => i.Length = Convert.ToInt64(value);
                setters["title"] = (i, value) => i.Title = value;
                setters["options"] = (i, value) => i.Options = Convert.ToInt32(value, 16);
                    // IIRC, options is interesting, because it's in hex, but might not have a 0x prefix.
                setters["pickn"] = (i, value) => i.PickN = Convert.ToInt32(value);
                setters["pickpercent"] = (i, value) => i.PickPercent = Convert.ToInt32(value);
                setters["ctime"] = (i, value) => i.CreationTime = ParseCreationTime(value);
                setters["artist"] = (i, value) => i.Artist = value;
                setters["bitrate"] = (i, value) => i.Bitrate = value;
                setters["codec"] = (i, value) => i.Codec = value;
                setters["duration"] = (i, value) => i.Duration = ParseDuration(value);
                setters["genre"] = (i, value) => i.Genre = value;
                setters["offset"] = (i, value) => i.Offset = Convert.ToInt64(value);
                setters["samplerate"] = (i, value) => i.SampleRate = Convert.ToInt32(value);
                setters["source"] = (i, value) => i.Source = value;
                setters["tracknr"] = (i, value) => i.TrackNumber = Convert.ToInt32(value);
                setters["year"] = (i, value) => i.Year = Convert.ToInt32(value);

                // OK. We've got the item. Turn it into something a bit more domain-specific.
                // TODO: Items with id >= 0x100 are required to have a valid type. We should probably enforce that.
                ItemBase item = null;
                string type;
                if (dictionary.TryGetValue("type", out type))
                {
                    if (type == "playlist")
                        item = new PlaylistItem(id);
                    else if (type == "tune")
                        item = new TuneItem(id);
                    else if (type == "illegal")
                    {
                        // Ignore it.
                    }
                    else
                        throw new Exception(string.Format("Unrecognised type: {0}", dictionary["type"]));
                }

                if (item != null)
                {
                    foreach (var pair in dictionary)
                    {
                        Action<ItemBase, string> setter;
                        if (setters.TryGetValue(pair.Key, out setter))
                            setter(item, pair.Value);
                        else
                            item.Add(pair.Key, pair.Value);
                    }

                    Console.WriteLine(item);
                    items.Add(item);
                }

                id += 0x10;
            }
        }

        private static TimeSpan ParseDuration(string value)
        {
            long milliseconds = Convert.ToInt64(value);
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        private static DateTime ParseCreationTime(string value)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0);

            long seconds = Convert.ToInt64(value);
            DateTime result = epoch.AddSeconds(seconds);
            return result;
        }
    }

    internal sealed class TuneItem : ItemBase
    {
        internal TuneItem(int id)
            : base(id)
        {
        }
    }

    internal sealed class PlaylistItem : ItemBase
    {
        internal PlaylistItem(int id)
            : base(id)
        {
        }
    }

    internal class ItemBase
    {
        private Dictionary<string, string> _tags = new Dictionary<string, string>();

        protected ItemBase(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public string Artist { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public int Year { get; set; }
        public int TrackNumber { get; set; }
        public string Genre { get; set; }

        public TimeSpan Duration { get; set; }
        public long Length { get; set; }

        public int Options { get; set; }
        public int PickN { get; set; }
        public int PickPercent { get; set; }

        public DateTime CreationTime { get; set; }

        public string Codec { get; set; }
        public string Bitrate { get; set; }
        public int SampleRate { get; set; }

        public long Offset { get; set; }

        public override string ToString()
        {
            return string.Format("{0:X}: {1} - {2} - {3} - {4}", Id, Source, TrackNumber, Artist, Title);
        }

        public void Add(string key, string value)
        {
            _tags[key] = value;
        }

        // TODO: I could store the stuff in a dictionary anyway, and make the properties do the conversions.
    }
}
