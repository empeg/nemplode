using System.Collections.Generic;

namespace NEmplode.EmpegCar.Model.Database
{
    public class MediaDatabase
    {
        private readonly string[] _tags;
        private readonly List<ItemBase> _items;

        internal MediaDatabase(string[] tags, List<ItemBase> items)
        {
            _tags = tags;
            _items = items;
        }

        public IEnumerable<ItemBase> Items
        {
            get { return _items; }
        }
    }
}