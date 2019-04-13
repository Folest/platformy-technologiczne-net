using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PlatformyTechnologiczne
{

    class Program
    { 
        static string PrintDirectory(string directory, bool recursive = false, int depth = 1, int overlap = 1)
        {
            var prefix = new string('\t', depth);
            var sb = new StringBuilder();

            // The size of directory overlap can be changed by the number in take last here.
            var fileNames = Directory.EnumerateFiles(directory)
                                     .Select(filenameWithPath =>
                                     {
                                         var fileInfo = new FileInfo(filenameWithPath);
                                         return filenameWithPath.Split('\\').TakeLast(overlap).Aggregate("", (acc, next) => $"{acc}\\{next}") + $"\t{fileInfo.Length} bajtów";
                                     })
                                     .ToList();

            var directoryNames = Directory.EnumerateDirectories(directory)
                                          .Select(directoryName => recursive 
                                                    ? PrintDirectory(directoryName,recursive ,depth + 1) 
                                                    : directoryName)
                                          .ToList();

            var allNames = fileNames.Concat(directoryNames).ToList();

            allNames.Sort();
            directory = directory.Split('\\').TakeLast(overlap)
                .Aggregate("", (acc, next) => 
                {
                    var dirInfo = new DirectoryInfo(directory);
                    return $"{acc}\\{next} ({dirInfo.GetFiles().Count()})";
                });
            sb.Append($"{(allNames.Count > 0 ? $"{directory} \n" : directory)}");

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

            Console.Write("\n\nZadanie 4. \n\n");

            dirInfo = new DirectoryInfo(directory);
            var sortedCollection = new SortedDictionary<string, long>(Comparer<string>.Create((a, b) => a.Length == b.Length ?
                                                                                                      a.CompareTo(b) :
                                                                                                      a.Length > b.Length ? 1 : -1));

            Console.Write("\n\nZadanie 5.a \n\n");

            dirInfo.EnumerateFileSystemInfos()
                .ToList()
                .ForEach(x =>
                {
                    var isDirectory = File.GetAttributes(x.FullName).HasFlag(FileAttributes.Directory);
                    // Adding name and amount of children if it's a directory and file size otherwise
                    sortedCollection.Add(x.FullName, isDirectory ?
                                                             Directory.EnumerateFileSystemEntries(x.FullName).LongCount() :
                                                             new FileInfo(x.FullName).Length);
                }
                );


            Console.WriteLine("Before serialization: \n");
            sortedCollection.ToList().ForEach(kv => Console.WriteLine($"{kv.Key} : {kv.Value}"));

            Console.WriteLine("\n\n Zadanie 5.b\n\n");

            var binaryFormatter = new BinaryFormatter();

            var kilo = (int)Math.Pow(2, 10);

            var serializedFilePath = "serializedCollection";

            using (var serializedDest = File.Create("serializedCollection"))
            {
                try
                {
                    binaryFormatter.Serialize(serializedDest, sortedCollection.ToList());
                    serializedDest.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to serialize cuz: ${e.Message}");
                }
            }

            using (var serializedSource = File.OpenRead(serializedFilePath))
            {
                try
                {

                    var deserialized = (List<KeyValuePair<string, long>>)binaryFormatter.Deserialize(serializedSource);
                    Console.WriteLine("After serialization");
                    sortedCollection.Clear();
                    Console.WriteLine($"Initial collection size: {sortedCollection.Count()}");
                    deserialized.ForEach(x => sortedCollection.Add(x.Key, x.Value));
                    sortedCollection.ToList().ForEach(x => Console.WriteLine($"{x.Key} -> {x.Value}"));

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }

}
