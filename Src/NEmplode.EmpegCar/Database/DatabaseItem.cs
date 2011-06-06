using System.Collections.Generic;

namespace NEmplode.EmpegCar.Database
{
    public class DatabaseItem
    {
        private readonly int _id;
        private readonly Dictionary<string, string> _fields;

        public DatabaseItem(int id, Dictionary<string, string> fields)
        {
            _id = id;
            _fields = fields;
        }

        public int Id { get { return _id; } }

        public string Artist { get { return GetValueOrDefault("artist"); } }
        public string Title { get { return GetValueOrDefault("title"); } }
        public string Source { get { return GetValueOrDefault("source"); } }
        public int TrackNumber { get; set; }
        public string Genre { get { return GetValueOrDefault("genre"); } }

        private string GetValueOrDefault(string fieldName)
        {
            string value;
            if (_fields.TryGetValue(fieldName, out value))
                return value;

            return null;
        }

        public override string ToString()
        {
            return string.Format("{0:X}: {1} - {2} - {3} - {4}", Id, Source, TrackNumber, Artist, Title);
        }
    }
}