using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;

namespace BAnalytics.MessageHandling.Model
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
        public string FormatTime {
            get { return Time.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        [JsonProperty(PropertyName = "uid")]
        public int Uid { get; set; }

        [JsonProperty(PropertyName = "sid")]
        public int Sid { get; set; }
        
        public int EventCategoryId
        {
            get
            {
                Type type = typeof(EventCategory);
                foreach (FieldInfo field in type.GetFields())
                {
                    DescriptionAttribute[] curDesc =
                        (DescriptionAttribute[]) field.GetCustomAttributes(typeof (DescriptionAttribute), false);
                    if (curDesc.Length > 0)
                    {
                        if (curDesc[0].Description == EventCategory)
                            return (int)(EventCategory)field.GetValue(null);
                    }
                }
                return 0;
            }
        }

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

    public enum EventCategory
    {
        [Description("login")]
        Login = 1,
        [Description("search")]
        Search = 2,
        [Description("logout")]
        Logout = 3,
        [Description("cart")]
        Cart = 4,
        [Description("favorite")]
        Favorite = 5
    }
}