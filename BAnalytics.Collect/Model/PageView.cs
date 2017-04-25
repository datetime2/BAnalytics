using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BAnalytics.Collect.Model
{
    [JsonObject]
    public class PageView
    {
        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }

        [JsonProperty(PropertyName = "uid")]
        public int Uid { get; set; }

        [JsonProperty(PropertyName = "sid")]
        public int Sid { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "ip")]
        public string Ip { get; set; }

        [JsonProperty(PropertyName = "vid")]
        public string Vid { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public string RefUrl { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        [JsonProperty(PropertyName = "ua")]
        public string UserAgent { get; set; }

        [JsonProperty(PropertyName = "sh")]
        public string ScreenHeight { get; set; }

        [JsonProperty(PropertyName = "sw")]
        public string ScreenWidth { get; set; }

        [JsonProperty(PropertyName = "cd")]
        public string ColorDepth { get; set; }

        [JsonProperty(PropertyName = "lang")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "os")]
        public string Os { get; set; }

        [JsonProperty(PropertyName = "cookie")]
        public string Cookie { get; set; }

        [JsonProperty(PropertyName = "rid")]
        public long Rid { get; set; }

        [JsonProperty(PropertyName = "did")]
        public long Did { get; set; }

        [JsonProperty(PropertyName = "sourceid")]
        public long SourceId { get; set; }
    }
}
