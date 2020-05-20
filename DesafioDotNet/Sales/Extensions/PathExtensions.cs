using System.IO;

namespace Sales.Extensions
{
    public static class PathExtensions
    {
        public static string ToNormalizedPath(this string path)
        {
            return Path.DirectorySeparatorChar == '/'
                ? path.Replace('\\', '/').TrimEnd('/')
                : path.Replace('/', '\\').TrimEnd('\\');
        }
    }
}
