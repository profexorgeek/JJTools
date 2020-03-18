using System;
using YamlDotNet.Serialization;

namespace Toolchest.Models
{
    // This is the metadata allowed on pages in
    // Microsoft's open-source documentation.
    // This is probably an incomplete implementation
    // of the YAML spec
    public class PageMeta
    {
        [YamlMember(Alias = "title")]
        public string Title { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlIgnore]
        public string Path { get; set; }

        [YamlMember(Alias = "ms.topic")]
        public string Topic { get; set; }

        [YamlMember(Alias = "ms.prod")]
        public string Product { get; set; }

        [YamlMember(Alias = "ms.custom")]
        public string Custom { get; set; }

        [YamlMember(Alias = "ms.assetid")]
        public string AssetId { get; set; }

        [YamlMember(Alias = "ms.technology")]
        public string Technology { get; set; }

        [YamlMember(Alias = "author")]
        public string Author { get; set; }

        [YamlMember(Alias = "ms.author")]
        public string MsAuthor { get; set; }

        [YamlMember(Alias = "manager")]
        public string Manager { get; set; }

        [YamlMember(Alias ="robots")]
        public string Robots { get; set; }

        [YamlMember(Alias = "ms.date")]
        public string Date { get; set; }


        // Custom Fields
        [YamlMember(Alias = "ms.faqid")]
        public string FaqId { get; set; }
    }
}
