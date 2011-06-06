using System;
using System.Collections.Generic;
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

                DatabaseItem item = BuildItem(id, dictionary);
                if (item != null)
                    items.Add(item);

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

        private static DatabaseItem BuildItem(int id, Dictionary<string, string> dictionary)
        {
            // TODO: Items with id >= 0x100 are required to have a valid type. We should probably enforce that.
            DatabaseItem item = new DatabaseItem(id, dictionary);
#if false
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
#endif

            return item;
        }

    }
}
