using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PlatformyTechnologiczne
{
    class Program
    { 
        static string PrintDirectory(string directory, bool recursive = false, int depth = 1)
        {
            var prefix = new string('-', 3 * depth);
            var sb = new StringBuilder();

            // The size of directory overlap can be changed by the number in take last here.
            var fileNames = Directory.EnumerateFiles(directory)
                                     .Select(filenameWithPath => filenameWithPath.Split('\\').TakeLast(2).Aggregate("", (acc, next) => acc + '\\' + next))
                                     .ToList();

            var directoryNames = Directory.EnumerateDirectories(directory)
                                          .Select(directoryName => recursive 
                                                    ? PrintDirectory(directoryName,recursive ,depth + 1) 
                                                    : directoryName)
                                          .ToList();

            var allNames = fileNames.Concat(directoryNames).ToList();

            allNames.Sort();
            directory = directory.Split('\\').TakeLast(2).Aggregate("", (acc, next) => acc + '\\' + next);
            sb.Append(allNames.Count() > 0 ? directory + '\n' : directory);

            allNames.ForEach(x =>
            {
                if (x != allNames.Last())
                    sb.AppendLine(prefix + x);
                else
                    sb.Append(prefix + x);
            });
            return sb.ToString();
        }


        static void Main(string[] args)
        {
            Console.Write("Zadanie 1. \n\n");

            var directory = args[0];

            Console.WriteLine($"Elements in {directory}");
            Console.Write(PrintDirectory(directory, true));


            Console.Write("\n\nZadanie 2. \n\n");

            var dirInfo = new DirectoryInfo(@"C:\Users\Karol\Downloads\new");
            Console.WriteLine($"Looking for the file/directory which was modified the longest time ago in {dirInfo}");



            var oldestChildren = dirInfo.OldestChildrenDate(fsi => fsi.LastWriteTime);
            Console.WriteLine(oldestChildren != DateTime.MaxValue 
                                                ? oldestChildren.ToString() 
                                                : "Directory with given path is empty or the file system is corrupted");


            Console.Write("\n\nZadanie 3. \n\n");

            var info = new FileInfo(@"C:\Users\Karol\Downloads\extra_warranties.pdf");
            Console.WriteLine(info.GetAttributesRahs());
        }
    }

}
