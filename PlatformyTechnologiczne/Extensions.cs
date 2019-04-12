using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PlatformyTechnologiczne
{
    static class DirectoryInfoExtensions
    {
        public static DateTime OldestChildrenDate(this DirectoryInfo directory,
                                                  Func<FileSystemInfo,
                                                  DateTime> strategy, bool wasCalledRecursively = false)
        {

            var oldestSubdirectory = directory.EnumerateDirectories()
                                              .ToList()
                                              .Select(x => x.OldestChildrenDate(strategy, true))
                                              .Aggregate(wasCalledRecursively ?
                                                            directory.LastWriteTime :
                                                            DateTime.MaxValue, (acc, next) => acc < next ? acc : next);

            var oldestFile = directory.EnumerateFiles()
                                      .ToList()
                                      .Aggregate(DateTime.MaxValue, (acc, next) => acc < strategy(next) ?
                                                                                         acc :
                                                                                         strategy(next));

            return oldestSubdirectory < oldestFile ? oldestSubdirectory : oldestFile;
        }

        public static string GetAttributesRahs(this FileSystemInfo fileSystemInfo)
        {
            var attributes = fileSystemInfo.Attributes;
            var sb = new StringBuilder();

            sb.Append( (attributes.HasFlag(FileAttributes.ReadOnly) ? "r" : "-") 
                      + (attributes.HasFlag(FileAttributes.Archive) ? "a" : "-")
                      + (attributes.HasFlag(FileAttributes.System) ? "s" : "-")
                      + (attributes.HasFlag(FileAttributes.Hidden) ? "h" : "-"));

            return sb.ToString();
        }
    }
}
