using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NEmplode.Core;

namespace NEmplode.EmpegCar.Database
{
    public class EmpegCarPlaylists
    {
        private readonly DatabaseItem[] _databaseItems;
        private readonly List<Tuple<int, int>> _edges = new List<Tuple<int, int>>();

        public EmpegCarPlaylists(IEnumerable<DatabaseItem> databaseItems, byte[] bytes)
        {
            _databaseItems = databaseItems.ToArray();

            // Create a directed graph of FIDs.
            BinaryReader reader = new BinaryReader(new MemoryStream(bytes));

            foreach (var databaseItem in _databaseItems)
            {
                if (databaseItem.Type != "playlist")
                    continue;

                int parentId = databaseItem.Id;
                int childCount = databaseItem.Length / sizeof(Int32);

                for (int childIndex = 0; childIndex < childCount; ++childIndex)
                {
                    int childId = reader.ReadInt32();
                    _edges.Add(Tuple.Create(parentId, childId));
                }
            }
        }

        public string[] GetPathTo(DatabaseItem databaseItem)
        {
            var result = DescendantExtensions.EnumerateDescendants(
                0x100,
                fid => _edges.Any(e => e.Item1 == fid),
                fid => _edges.Where(e => e.Item1 == fid).Select(e => e.Item2));

            // TODO: Add a call to .First or .Single here.

            return result
                .Select(fid => _databaseItems.Single(x => x.Id == fid))
                .Select(item => item.Title)
                .ToArray();
        }
    }
}