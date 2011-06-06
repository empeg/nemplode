using System.Collections.Generic;

namespace NEmplode.EmpegCar.Database
{
    public class EmpegCarDatabase
    {
        private readonly string[] _tags;
        private readonly List<DatabaseItem> _items;

        public EmpegCarDatabase(string[] tags, List<DatabaseItem> items)
        {
            _tags = tags;
            _items = items;
        }

        public IEnumerable<DatabaseItem> Items
        {
            get { return _items; }
        }
    }
}