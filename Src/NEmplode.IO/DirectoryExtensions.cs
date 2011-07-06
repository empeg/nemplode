using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NEmplode.Core;

namespace NEmplode.IO
{
    public static class DirectoryExtensions
    {
        public static IEnumerable<FileSystemInfo> EnumerateFileSystemEntries<TKey>(this DirectoryInfo directoryInfo, Func<FileSystemInfo, TKey> keySelector)
        {
            return DescendantExtensions.EnumerateDescendants<FileSystemInfo>(
                directoryInfo,
                item => item is DirectoryInfo,
                item => ((DirectoryInfo)item).EnumerateFileSystemInfos().OrderBy(keySelector));
        }
    }
}