using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Toolchest.Models;

namespace Toolchest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Call whatever methods need to be called here to
            // get some grunt-work done!
        }


        // Call this method to find all markdown files (recursively) in a directory and
        // create a CSV with all file metadata
        static void GetPageMetadataFromDirectory(string directoryPath, string outputFilePath)
        {
            var allMarkdownFiles = new DirectoryCrawler().GetFilesRecursively(directoryPath);

            var metaExtractor = new MarkdownMetaExtractor();
            List<PageMeta> metas = new List<PageMeta>();
            int metaCount = 0;
            foreach (var f in allMarkdownFiles)
            {
                var meta = metaExtractor.ExtractMeta(f);
                if (meta != null)
                {
                    metas.Add(meta);
                    metaCount++;
                }
            }

            using (var writer = new StreamWriter(outputFilePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(metas);
            }

            Console.WriteLine($"Extracted meta from {metaCount}/{allMarkdownFiles.Count} documents");
        }
    }
}
