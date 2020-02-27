using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Toolchest.Models;

namespace Toolchest
{
    public class FaqJsonParser
    {
        public static List<FaqIndexItem> GetFaqContent(string indexPath, string faqPath)
        {
            // parse json
            var indexJson = File.ReadAllText(indexPath);
            var faqJson = File.ReadAllText(faqPath);
            var categories = JsonConvert.DeserializeObject<List<FaqIndexItem>>(indexJson);
            var faqs = JsonConvert.DeserializeObject<List<FaqItem>>(faqJson);

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
    }
}
