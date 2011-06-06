using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NEmplode.EmpegCar.Database;
using NEmplode.EmpegCar.Database.Sources;

namespace NEmplode.EmpegCar.Model.Database
{
#if false
    internal class DatabaseReader : IEmpegDatabaseReader
    {
        private static readonly Dictionary<string, Action<ItemBase, string>> _setters;

        static DatabaseReader()
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

        private List<ItemBase> ReadDatabase(string[] tags)
        {
            var databaseBytes = _databaseSource.DownloadDatabase();
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
#endif
}