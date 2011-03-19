using System.Collections.Generic;

namespace NEmplode.Download
{
    internal class MediaDatabase
    {
        private readonly string[] _tags;
        private readonly List<ItemBase> _items;

        internal MediaDatabase(string[] tags, List<ItemBase> items)
        {
            _tags = tags;
            _items = items;
        }
    }
}