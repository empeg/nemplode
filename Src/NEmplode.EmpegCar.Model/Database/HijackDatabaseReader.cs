using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace NEmplode.EmpegCar.Model.Database
{
    internal class HijackDatabaseReader
    {
        private static readonly Dictionary<string, Action<ItemBase, string>> _setters;

        static HijackDatabaseReader()
        {
            _setters = new Dictionary<string, Action<ItemBase, string>>();
            _setters["type"] = (i, value) => { /* do nothing. */ };
            _setters["length"] = (i, value) => i.Length = Convert.ToInt64(value);
            _setters["title"] = (i, value) => i.Title = value;
            _setters["options"] = (i, value) => i.Options = Convert.ToInt32(value, 16);
            _setters["pickn"] = (i, value) => i.PickN = Convert.ToInt32(value);
            _setters["pickpercent"] = (i, value) => i.PickPercent = Convert.ToInt32(value);
            _setters["ctime"] = (i, value) => i.CreationTime = ParseCreationTime(value);
            _setters["artist"] = (i, value) => i.Artist = value;
            _setters["bitrate"] = (i, value) => i.Bitrate = value;
            _setters["codec"] = (i, value) => i.Codec = value;
            _setters["duration"] = (i, value) => i.Duration = ParseDuration(value);
            _setters["genre"] = (i, value) => i.Genre = value;
            _setters["offset"] = (i, value) => i.Offset = Convert.ToInt64(value);
            _setters["samplerate"] = (i, value) => i.SampleRate = Convert.ToInt32(value);
            _setters["source"] = (i, value) => i.Source = value;
            _setters["tracknr"] = (i, value) => i.TrackNumber = Convert.ToInt32(value);
            _setters["year"] = (i, value) => i.Year = Convert.ToInt32(value);
        }

        private readonly Uri _empegUri;

        public HijackDatabaseReader(Uri empegUri)
        {
            _empegUri = empegUri;
        }

        public MediaDatabase ReadDatabase()
        {
            WebClient client = new WebClient();

            string[] tags = DownloadTags(client);
            List<ItemBase> items = ReadDatabase(client, tags);

            var playlistsBytes = client.DownloadData(_empegUri + "/empeg/var/playlists");

            // TODO: Put the playlist children in the items.
            return new MediaDatabase(tags, items);
        }

        private List<ItemBase> ReadDatabase(WebClient client, string[] tags)
        {
            // TODO: If this file doesn't exist, try the /empeg/var/database file. In fact, we should probably figure out what version of the player you're using. Interesting problem.
            var databaseBytes = client.DownloadData(_empegUri + "/empeg/var/database3");
            var databaseStream = new MemoryStream(databaseBytes);

            // TODO: Figure this out from the database version.
            Encoding encoding = Encoding.UTF8;
            var databaseReader = new BinaryReader(databaseStream, encoding);

            // Loop over the records.
            var items = new List<ItemBase>();
            int id = 0x0;
            while (databaseReader.BaseStream.Position < databaseReader.BaseStream.Length)
            {
                Dictionary<string, string> dictionary = ReadItemFields(tags, databaseReader, encoding);

                ItemBase item = BuildItem(id, dictionary);
                if (item != null)
                    items.Add(item);

                id += 0x10;
            }

            return items;
        }

        private ItemBase BuildItem(int id, Dictionary<string, string> dictionary)
        {
            // OK. We've got the item. Turn it into something a bit more domain-specific.
            // TODO: Items with id >= 0x100 are required to have a valid type. We should probably enforce that.
            ItemBase item = null;
            string type;
            if (dictionary.TryGetValue("type", out type))
            {
                switch (type)
                {
                    case "playlist":
                        item = new PlaylistItem(id);
                        break;
                    case "tune":
                        item = new TuneItem(id);
                        break;
                    case "illegal":
                        // ignore it.
                        break;
                    default:
                        throw new Exception(string.Format("Unrecognised type: {0}", dictionary["type"]));
                }
            }

            if (item != null)
            {
                foreach (var pair in dictionary)
                {
                    Action<ItemBase, string> setter;
                    if (_setters.TryGetValue(pair.Key, out setter))
                        setter(item, pair.Value);
                    else
                        item.Add(pair.Key, pair.Value);
                }
            }
            return item;
        }

        private static Dictionary<string, string> ReadItemFields(string[] tags, BinaryReader databaseReader, Encoding encoding)
        {
            var dictionary = new Dictionary<string, string>();

            // Loop over the fields.
            for (; ; )
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

            return dictionary;
        }

        private string[] DownloadTags(WebClient client)
        {
            var tagsBytes = client.DownloadData(_empegUri + "/empeg/var/tags");

            // The 'tags' file is ASCII, LF line-endings.
            var tagsString = Encoding.ASCII.GetString(tagsBytes);
            return tagsString.Split('\n');
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
}