using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BAnalytics.MessageHandling.Model
{
    public class PageView
    {
        public PageView()
        {
            CityId = -1;
            LoginChannel = -1;
            MemberId = -1;
            VisitChannel = -1;
            VisitSource = -1;
            UrlTypeId = -1;
        }

        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }
        public string FormatTime
        {
            get { return Time.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        [JsonProperty(PropertyName = "uid")]
        public int Uid { get; set; }

        [JsonProperty(PropertyName = "sid")]
        public int Sid { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "ip")]
        public string Ip { get; set; }

        public string CityName { get; set; }

        public int CityId { get;set; }

        public string ProvinceName { get; set; }

        public string DistrictName { get; set; }

        public string StreetName { get; set; }

        public string StreetNumber { get; set; }

        [JsonProperty(PropertyName = "vid")]
        public string Vid { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public string RefUrl { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        public Int16 LoginChannel { get; set; }
        public Int64 MemberId { get; set; }

        [JsonProperty(PropertyName = "ua")]
        public string UserAgent { get; set; }

        public string BrowserFamily { get; set; }

        public string BrowserMajor { get; set; }

        public string BrowserMinor { get; set; }

        public string OsFamily { get; set; }

        public string OsMajor { get; set; }

        public string OsMinor { get; set; }

        public string DeviceFamily { get; set; }

        public string DeviceBrand { get; set; }

        public string DeviceModel { get; set; }

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

        public int VisitChannel { get; set; }
        public int VisitSource { get; set; }
        public int UrlTypeId { get; set; }
    }
}
