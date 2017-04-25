using System;
using Newtonsoft.Json;

namespace BAnalytics.Collect.Model
{
    [JsonObject]
    public class Event
    {
        [JsonProperty(PropertyName = "eid")]
        public string Eid { get; set; }

        [JsonProperty(PropertyName = "vid")]
        public string Vid { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }

        [JsonProperty(PropertyName = "uid")]
        public int Uid { get; set; }

        [JsonProperty(PropertyName = "sid")]
        public int Sid { get; set; }

        [JsonProperty(PropertyName = "eventCategory")]
        public string EventCategory { get; set; }

        [JsonProperty(PropertyName = "eventAction")]
        public string EventAction { get; set; }

        [JsonProperty(PropertyName = "eventLabel")]
        public string EventLabel { get; set; }

        [JsonProperty(PropertyName = "eventValue")]
        public string EventValue { get; set; }

        [JsonProperty(PropertyName = "eventNodeId")]
        public string EventNodeId { get; set; }
    }
}