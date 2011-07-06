using System.Collections.Generic;

namespace NEmplode.EmpegCar.Database
{
    public class DatabaseItem
    {
        private readonly int _id;
        private readonly int _length;
        private readonly Dictionary<string, string> _fields;

        public DatabaseItem(int id, int length, Dictionary<string, string> fields)
        {
            _id = id;
            _length = length;
            _fields = fields;
        }

        public int Id
        {
            get { return _id; }
        }

        public string Type
        {
            get { return GetValueOrDefault("type"); }
        }

        public string Artist
        {
            get { return GetValueOrDefault("artist"); }
        }

        public string Title
        {
            get { return GetValueOrDefault("title"); }
        }

        public string Source
        {
            get { return GetValueOrDefault("source"); }
        }

        public int TrackNumber { get; set; }

        public string Genre
        {
            get { return GetValueOrDefault("genre"); }
        }

        public int Length
        {
            get { return _length; }
        }

        private string GetValueOrDefault(string fieldName)
        {
            string value;
            if (_fields.TryGetValue(fieldName, out value))
                return value;

            return null;
        }

        public override string ToString()
        {
            return string.Format("{0:X} ({1}): {2} - {3} - {4} - {5}", Id, Type, Source, TrackNumber, Artist, Title);
        }
    }
}