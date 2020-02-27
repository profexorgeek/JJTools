using System.IO;
using System.Text.RegularExpressions;
using Toolchest.Models;
using YamlDotNet.Serialization;

namespace Toolchest
{
    // This class extracts the YAML metadata from the top
    // of markdown pages in Microsoft's open-source doc files
    // Useful for getting a CSV of all pages in a doc set
    public class MarkdownMetaExtractor
    {
        Deserializer deserializer;

        public MarkdownMetaExtractor()
        {
            deserializer = new Deserializer();
        }

        public PageMeta ExtractMeta(string path)
        {
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

    }
}
