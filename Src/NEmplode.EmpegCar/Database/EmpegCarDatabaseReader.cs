using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NEmplode.EmpegCar.Database.Sources;

namespace NEmplode.EmpegCar.Database
{
    public class EmpegCarDatabaseReader
    {
        private readonly IEmpegCarDatabaseSource _source;

        public EmpegCarDatabaseReader(IEmpegCarDatabaseSource source)
        {
            _source = source;
        }

        private string[] FetchTags()
        {
            Trace.TraceInformation("Fetching tags...");

            byte[] tagsBytes = _source.DownloadTags();

            // Parse the tags.
            string tagsText = Encoding.UTF8.GetString(tagsBytes);
            string[] tagsLines = tagsText.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            return tagsLines;
        }

        public EmpegCarDatabase ReadDatabase()
        {
            string[] tags = FetchTags();
            var items = FetchDatabase(tags);

            return new EmpegCarDatabase(tags, items.ToList());
        }

        private IEnumerable<DatabaseItem> FetchDatabase(string[] tags)
        {
            Trace.TraceInformation("Fetching database...");

            byte[] databaseBytes = _source.DownloadDatabase();

            var databaseStream = new MemoryStream(databaseBytes);

            // TODO: Figure this out from the database version. In fact, have DownloadDatabase be called OpenDatabase and return the correct reader with encoding?
            Encoding encoding = Encoding.UTF8;
            var databaseReader = new BinaryReader(databaseStream, encoding);

            // Loop over the records.
            List<DatabaseItem> items = new List<DatabaseItem>();
            int id = 0x0;
            while (databaseReader.BaseStream.Position < databaseReader.BaseStream.Length)
            {
                Dictionary<string, string> dictionary = ReadItemFields(tags, databaseReader, encoding);
                if (dictionary.Count != 0)
                {
                    string type = dictionary["type"];
                    if (type == "tune" || type == "playlist")
                    {
                        int length = int.Parse(dictionary["length"]);

                        DatabaseItem item = BuildItem(id, length, dictionary);
                        if (item != null)
                            items.Add(item);
                    }
                }

                id += 0x10;
            }

            return items;
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

        private static DatabaseItem BuildItem(int id, int length, Dictionary<string, string> dictionary)
        {
            // TODO: Items with id >= 0x100 are required to have a valid type. We should probably enforce that.
            DatabaseItem item = new DatabaseItem(id, length, dictionary);
            return item;
        }

        public EmpegCarPlaylists ReadPlaylists(IEnumerable<DatabaseItem> databaseItems)
        {
            return new EmpegCarPlaylists(databaseItems, _source.DownloadPlaylists());
        }
    }
}
