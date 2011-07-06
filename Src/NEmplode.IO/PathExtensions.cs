using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NEmplode.Core;

namespace NEmplode.IO
{
    public static class PathExtensions
    {
        private const string DirectorySeparatorString = @"\";
        private const string AltDirectorySeparatorString = @"/";

        public static string GetRelativePath(this DirectoryInfo ancestor, FileSystemInfo descendant)
        {
            return GetRelativePath(ancestor.FullName, descendant.FullName);
        }

        public static string GetRelativePath(string ancestor, string descendant)
        {
            // Given something like C:\Foo\Bar and C:\Foo\Bar\Baz\Quux, we need Baz\Quux.
            if (descendant.IndexOf(ancestor, StringComparison.InvariantCultureIgnoreCase) != 0)
                throw new Exception(string.Format("{0} is not a descendant of {1}", descendant, ancestor));

            if (string.Compare(ancestor, descendant, StringComparison.InvariantCultureIgnoreCase) == 0)
                return string.Empty;

            ancestor = ancestor.TrimEnd('\\');
            return descendant.Substring(ancestor.Length + 1);
        }

        public static string[] Split(string path)
        {
            if (!Path.IsPathRooted(path))
                throw new ArgumentOutOfRangeException("path", path, "Expected an absolute path starting with a drive letter.");
            if (!(path.Length >= 3 && path[1] == Path.VolumeSeparatorChar && path[2] == Path.DirectorySeparatorChar))
                throw new ArgumentOutOfRangeException("path", path, "Expected an absolute path starting with a drive letter.");

            return path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private static string[] GetRelativePath(string[] from, string[] to)
        {
            var relative = new List<string>();

            string[] prefix = from.CommonPrefix(to, StringComparer.InvariantCultureIgnoreCase.Equals).ToArray();
            if (prefix.Length == 0)
                throw new IOException("GetRelativePath works only on paths which share a common prefix.");

            for (int i = from.Length; i > prefix.Length; --i)
                relative.Add("..");

            for (int i = prefix.Length; i < to.Length; ++i)
                relative.Add(to[i]);

            return relative.ToArray();
        }

        public static bool HasInvalidFileNameCharacters(string name)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            return name.IndexOfAny(invalidFileNameChars) != -1;
        }
    }
}
