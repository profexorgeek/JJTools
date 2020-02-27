using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Toolchest.Logging;
using Toolchest.Models;
using YamlDotNet.Serialization;

namespace Toolchest
{
    // This class extracts the YAML metadata from the top
    // of markdown pages in Microsoft's open-source doc files
    // Useful for getting a CSV of all pages in a doc set
    public class MarkdownMetaExtractor
    {
        // This method returns a PageMeta object from the provided
        // markdown page's YAML header
        public static PageMeta ExtractMeta(string path)
        {

            var deserializer = new Deserializer();
            PageMeta meta = null;
            var fileText = File.ReadAllText(path).Replace("\r", "");

            // get just the yaml metadata from a page:
            var yamlRegex = new Regex(@"---\n(.*)---\n+# ", RegexOptions.Singleline);
            var match = yamlRegex.Match(fileText);
            var yaml = match.Groups[1].Value;

            // deserialize the metadata
            meta = deserializer.Deserialize<PageMeta>(yaml);
            if(meta == null)
            {
                meta = new PageMeta();
            }
            meta.Path = path;

            return meta;
        }

        // Call this method to find all markdown files (recursively) in a directory and
        // create a CSV with all file metadata
        static void GetPageMetadataFromDirectory(string directoryPath, string outputFilePath)
        {
            var allMarkdownFiles = new DirectoryCrawler().GetFilesRecursively(directoryPath);
            List<PageMeta> metas = new List<PageMeta>();
            int metaCount = 0;
            foreach (var f in allMarkdownFiles)
            {
                var meta = ExtractMeta(f);
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

            ConsoleLog.Instance.Info($"Extracted meta from {metaCount}/{allMarkdownFiles.Count} documents");
        }

    }
}
