using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace BAnalytics.MessageHandling.Model
{
    /// <summary>
    /// 地址信息
    /// </summary>
    public class AddressInfo
    {
        /// <summary>
        /// 地址
        /// </summary>
        [DeserializeAs(Name = "address")]
        public string Address { get; set; }
        /// <summary>
        /// 详细内容
        /// </summary>
        [DeserializeAs(Name = "content")]
        public AddressContent Content { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        [DeserializeAs(Name = "status")]
        public short Status { get; set; }
    }

    /// <summary>
    /// 地址详细内容
    /// </summary>
    public class AddressContent
    {
        /// <summary>
        /// 简要地址
        /// </summary>
        [DeserializeAs(Name = "address")]
        public string Address { get; set; }
        /// <summary>
        /// 详细地址信息
        /// </summary>
        [DeserializeAs(Name = "address_detail")]
        public AddressDetail Detail { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        [DeserializeAs(Name = "point")]
        public AddressPoint Point { get; set; }

    }

    /// <summary>
    /// 地址详细信息
    /// </summary>
    public class AddressDetail
    {
        /// <summary>
        /// 城市
        /// </summary>
        [DeserializeAs(Name = "city")]
        public string City { get; set; }
        /// <summary>
        /// 百度城市代码
        /// </summary>
        [DeserializeAs(Name = "city_code")]
        public int CityCode { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        [DeserializeAs(Name = "district")]
        public string District { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [DeserializeAs(Name = "province")]
        public string Province { get; set; }
        /// <summary>
        /// 街道
        /// </summary>
        [DeserializeAs(Name = "street")]
        public string Street { get; set; }
        /// <summary>
        /// 门址
        /// </summary>
        [DeserializeAs(Name = "street_number")]
        public string StreetNumber { get; set; }
    }

    /// <summary>
    /// 坐标
    /// </summary>
    public class AddressPoint
    {
        [DeserializeAs(Name = "x")]
        public string X { get; set; }

        [DeserializeAs(Name = "y")]
        public string Y { get; set; }
    }
}
