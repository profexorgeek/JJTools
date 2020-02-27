using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Toolchest.Logging;
using Toolchest.Models;

namespace Toolchest
{
    // This class was created to transform a bunch of JSON files containing FAQs
    // into individual markdown files
    public class FaqJsonParser
    {
        public string MsAuthor { get; set; } = "";
        public string Author { get; set; } = "";

        public List<FaqIndexItem> GetFaqContent(string indexPath, string faqPath)
        {
            // parse json
            var indexJson = File.ReadAllText(indexPath);
            var faqJson = File.ReadAllText(faqPath);

            var categories = JsonConvert.DeserializeObject<List<FaqIndexItem>>(indexJson);
            var faqs = JsonConvert.DeserializeObject<List<FaqItem>>(faqJson);

            Log.Instance.Info($"Found {categories.Count} categories with {faqs.Count} total items.");

            // build runtime tree
            foreach (var cat in categories)
            {
                if (cat.RuntimeChildren == null)
                {
                    cat.RuntimeChildren = new List<FaqItem>();
                }

                foreach (var childId in cat.Children)
                {
                    var childRuntime = faqs.FirstOrDefault(c => c.Id == childId);

                    if (childRuntime != null)
                    {
                        cat.RuntimeChildren.Add(childRuntime);
                        faqs.Remove(childRuntime);
                    }
                }
            }

            return categories;
        }

        public void CreateMarkdownFiles(List<FaqIndexItem> items, string outputDirectory)
        {
            var yamlSerializer = new YamlDotNet.Serialization.Serializer();

            if(!Directory.Exists(outputDirectory))
            {
                throw new Exception("Invalid output directory. Can't save files here!");
            }

            foreach(var parent in items)
            {
                var dir = Path.Combine(outputDirectory, parent.Id);
                if(!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var parentPath = Path.Combine(dir, "index.md");
                var parentSb = new StringBuilder();
                var parentMeta = new PageMeta
                {
                    Title = parent.Text,
                    Description = "",
                    Date = DateTime.Now,
                    MsAuthor = this.MsAuthor,
                    Author = this.Author,
                    AssetId = Guid.NewGuid().ToString("D")
                };
                var parentMetaString = yamlSerializer.Serialize(parentMeta);
                parentSb.Append($"---\n{parentMetaString}---\n\n");

                foreach (var child in parent.RuntimeChildren)
                {
                    var childFilename = $"{child.Id}.md";
                    var childPath = Path.Combine(dir, childFilename);
                    var childSb = new StringBuilder();

                    var childMeta = new PageMeta
                    {
                        Title = child.Faq,
                        Description = "",
                        Date = DateTime.Now,
                        MsAuthor = this.MsAuthor,
                        Author = this.Author,
                        AssetId = Guid.NewGuid().ToString("D")
                    };

                    // build the markdown file
                    var childMetaString = yamlSerializer.Serialize(childMeta);
                    childSb.Append($"---\n{childMetaString}---\n\n");
                    childSb.Append($"## {child.Faq}\n\n");
                    childSb.Append($"{child.Ans}\n");
                    File.WriteAllText(childPath, childSb.ToString());

                    // include file in parent
                    parentSb.Append($"[!INCLUDE [{childMeta.Title}]({childFilename})]\n\n\n");
                }

                // write index file:
                File.WriteAllText(parentPath, parentSb.ToString());
            }

        }
    }
}
