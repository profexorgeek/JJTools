using Newtonsoft.Json;
using System.Collections.Generic;

namespace Toolchest.Models
{
    public class FaqIndexItem
    {
        public string Text { get; set; }
        public string Id { get; set; }
        public List<string> Children { get; set; }

        [JsonIgnore]
        public List<FaqItem> RuntimeChildren { get; set; }
    }

    public class FaqItem
    {
        public string Id { get; set; }
        public string Faq { get; set; }
        public string Ans { get; set; }
    }
}
