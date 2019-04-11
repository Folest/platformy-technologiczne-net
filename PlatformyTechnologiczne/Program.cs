using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Text;

namespace PlatformyTechnologiczne
{
    class Program
    {
        static void PrintOutDirectoryInitializer(string directory, bool recursive = true, int depth = 0)
        {
            Console.WriteLine(DirectoryContentAlphabetical(directory));
        }

        static string AllDirectoryChildren(string directory)
        {
            return Directory.EnumerateFiles(directory)
                .Concat(Directory.EnumerateDirectories(directory))
                .Aggregate("", (acc, next) => acc + '\n' + next);
        }

        static string DirectoryContentAlphabetical(string directory, int depth = 1)
        {
            var prefix = new string('-', 3 * depth);
            var sb = new StringBuilder();

            var fileNames = Directory.EnumerateFiles(directory)
                                     .Select(x => x.Split('\\').TakeLast(2).Aggregate("", (acc, next) => acc + '\\' + next))
                                     .ToList();

            var directoryNames = Directory.EnumerateDirectories(directory)
                                          .Select(dirName => DirectoryContentAlphabetical(dirName, depth + 1))
                                          .ToList();

                            
           //var directoryNames = Directory.EnumerateDirectories(directory).Select(_ => "Folder").ToList();


            var allNames = fileNames.Concat(directoryNames).ToList();

            allNames.Sort();
            directory = directory.Split('\\').TakeLast(2).Aggregate("", (acc, next) => acc + '\\' + next);
            sb.Append(allNames.Count() > 0 ? directory + '\n' : directory);

            allNames.ForEach(x =>
            {
                //var split = x.Split('\\');
                //// Reducing the last elements of string list separated by \ into a single string 
                //var lastElements = split.TakeLast(depth).ToList();
                //// restoring the original formatting with \ in between directories
                //var lastElementsStringified = lastElements.Aggregate("", (acc, str) => acc + '\\' + str);
                //sb.AppendLine(prefix + lastElementsStringified);
                //------------------------------------
                if (x != allNames.Last())
                    sb.AppendLine(prefix + x);
                else
                    sb.Append(prefix + x);
            });
            return sb.ToString();
        }


        static void Main(string[] args)
        {
            //var sourceDirectory = "C:\\Users\\Karol\\Desktop\\PG\\Semestr4\\Metody Probabilistyczne\\aaaaaaaaaaaaa";
            var sourceDirectory = args[0];
            Console.WriteLine($"Elements in {sourceDirectory}");


            //Console.WriteLine(AllDirectoryChildren(sourceDirectory));

            PrintOutDirectoryInitializer(sourceDirectory, true);
        }
    }
}
