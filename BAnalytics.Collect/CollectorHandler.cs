using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BAnalytics.Collect.Model;
using log4net;
using BAnalytics.MQHelp;
using Newtonsoft.Json;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace BAnalytics.Collect
{
    public class CollectorHandler : IHttpHandler
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CollectorHandler));
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// 30分钟过期的会话标识
        /// </summary>
        private string Ubso
        {
            get
            {
                if (HttpContext.Current.Request.Cookies.Get("_ubso") == null)
                {
                    return null;
                }
                if (HttpContext.Current.Request.Cookies.Get("_ubso").Value == string.Empty)
                {
                    return null;
                }
                return HttpContext.Current.Request.Cookies.Get("_ubso").Value;
            }
        }

        /// <summary>
        /// 浏览器关闭过期的会话标识
        /// </summary>
        private string Ubsc
        {
            get
            {
                if (HttpContext.Current.Request.Cookies.Get("_ubsc") == null)
                {
                    return null;
                }
                if (HttpContext.Current.Request.Cookies.Get("_ubsc").Value == string.Empty)
                {
                    return null;
                }
                return HttpContext.Current.Request.Cookies.Get("_ubsc").Value;
            }
        }

        /// <summary>
        /// 会话标识
        /// </summary>
        private string SessionId
        {
            get
            {
                if (Ubso == null || Ubsc == null || Ubso != Ubsc)
                {
                    return null;
                }
                return Ubso;
            }
        }

        private string ClientIp
        {
            get
            {
                string userHostAddress = string.Empty;
                //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0].Trim();
                }
                //否则直接读取REMOTE_ADDR获取客户端IP地址
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = HttpContext.Current.Request.UserHostAddress;
                }
                //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
                if (!string.IsNullOrEmpty(userHostAddress) && Regex.IsMatch(userHostAddress, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                {
                    return userHostAddress;
                }
                return "127.0.0.1";
            }
        }

        private Uri _url;
        private Uri Url
        {
            get
            {
                if (_url == null)
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["url"]))
                    {
                        try
                        {
                            _url = new Uri(HttpContext.Current.Request.QueryString["url"].ToLower());
                        }
                        catch (UriFormatException)
                        {
                        }
                    }
                }
                return _url;
            }
        }

        private Uri _refUrl;
        private Uri RefUrl
        {
            get
            {
                if (_refUrl == null)
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ref"]))
                    {
                        try
                        {
                            _refUrl = new Uri(HttpContext.Current.Request.QueryString["ref"].ToLower());
                        }
                        catch (UriFormatException)
                        {
                        }
                    }
                }
                return _refUrl;
            }
        }

        private Uri _eventUrl;
        private Uri EventUrl
        {
            get
            {
                if (_eventUrl == null)
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["eventURL"]))
                    {
                        try
                        {
                            _eventUrl = new Uri(HttpContext.Current.Request.QueryString["eventURL"].ToLower());
                        }
                        catch (UriFormatException)
                        {
                        }
                    }
                }
                return _eventUrl;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.AddHeader("P3P", "CP=\"CAO PSA OUR\"");
                //初始化用户标识
                var uid = context.Request.Cookies.Get("_ubvi") == null ? HashMd5(Guid.NewGuid().ToString()) : Convert.ToInt32(context.Request.Cookies.Get("_ubvi").Value);
                context.Response.Cookies.Add(new HttpCookie("_ubvi", uid.ToString())
                {
                    Expires = DateTime.Now.AddYears(2)
                });
                var expTime = DateTime.Compare(DateTime.Now.AddMinutes(30).Date, DateTime.Now.Date) == 0
                    ? DateTime.Now.AddMinutes(30)
                    : DateTime.Now.Date.AddDays(1); 

                //初始化会话标识
                var sid = SessionId == null ? HashMd5(Guid.NewGuid().ToString()) : Convert.ToInt32(SessionId);
                context.Response.Cookies.Add(new HttpCookie("_ubso", sid.ToString())
                {
                    Expires = expTime
                });
                context.Response.Cookies.Add(new HttpCookie("_ubsc", sid.ToString())
                {
                    Expires = DateTime.MinValue
                });

                if (context.Request.QueryString["t"] == "v")
                {
                    PageView pv = new PageView()
                    {
                        Time = DateTime.Now,
                        Uid = uid,
                        Sid = sid,
                        Url = Url == null ? string.Empty : FomartUrl(Url).Replace("\t", " "),
                        Ip = ClientIp,
                        Vid = (context.Request.QueryString["vid"] ?? string.Empty).Replace("\t", " "),
                        Title = (context.Request.QueryString["title"] ?? string.Empty).Replace("\t", " "),
                        RefUrl = RefUrl == null ? string.Empty : FomartUrl(RefUrl).Replace("\t", " "),
                        User = (context.Request.QueryString["user"] ?? string.Empty).Replace("\t", " "),
                        UserAgent = (context.Request.UserAgent ?? string.Empty).Replace("\t", " "),
                        ScreenHeight = (context.Request.QueryString["sh"] ?? string.Empty).Replace("\t", " "),
                        ScreenWidth = (context.Request.QueryString["sw"] ?? string.Empty).Replace("\t", " "),
                        ColorDepth = (context.Request.QueryString["cd"] ?? string.Empty).Replace("\t", " "),
                        Language = (context.Request.QueryString["lang"] ?? string.Empty).Replace("\t", " "),
                        Os = (context.Request.QueryString["os"] ?? string.Empty).Replace("\t", " "),
                        Cookie = (context.Request.QueryString["cookie"] ?? string.Empty).Replace("\t", " "),
                        Rid =
                            Convert.ToInt64(
                                (string.IsNullOrWhiteSpace(context.Request.QueryString["rid"])
                                    ? "0"
                                    : context.Request.QueryString["rid"]).Replace("\t", " ")),
                        Did =
                            Convert.ToInt64(
                                (string.IsNullOrWhiteSpace(context.Request.QueryString["did"])
                                    ? "0"
                                    : context.Request.QueryString["did"]).Replace("\t", " ")),
                        SourceId =
                            Convert.ToInt64(
                                (string.IsNullOrWhiteSpace(context.Request.QueryString["sourceid"])
                                    ? "0"
                                    : context.Request.QueryString["sourceid"]).Replace("\t", " "))
                    };
                    MessageQuery.Send("pv", JsonConvert.SerializeObject(pv));
                }
                else if (context.Request.QueryString["t"] == "e")
                {
                    Event e = new Event()
                    {
                        Time = DateTime.Now,
                        Uid = uid,
                        Sid = sid,
                        Url = EventUrl == null ? string.Empty : FomartUrl(EventUrl).Replace("\t", " "),
                        Vid = (context.Request.QueryString["vid"] ?? string.Empty).Replace("\t", " "),
                        Eid = Guid.NewGuid().ToString(),
                        EventCategory = (context.Request.QueryString["category"] ?? string.Empty).Replace("\t", " "),
                        EventAction = (context.Request.QueryString["action"] ?? string.Empty).Replace("\t", " "),
                        EventLabel = (context.Request.QueryString["label"] ?? string.Empty).Replace("\t", " "),
                        EventValue = (context.Request.QueryString["value"] ?? string.Empty).Replace("\t", " "),
                        EventNodeId = (context.Request.QueryString["nodeid"] ?? string.Empty).Replace("\t", " ")
                    };
                    MessageQuery.Send("event", JsonConvert.SerializeObject(e));
                }
                context.Response.Write("ok");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message + "\n" + ex.Source + "\n" + ex.TargetSite);
                context.Response.Write(ex.Message + "\n" + ex.Source + "\n" + ex.TargetSite);
            }
        }

        public int HashMd5(string value)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var hash = BitConverter.ToInt32(md5.ComputeHash(Encoding.Default.GetBytes(value)), 0);
            return hash;
        }

        private string FomartUrl(Uri url)
        {
            if (url == null)
            {
                throw new NullReferenceException("url is null");
            }
            string path = url.LocalPath.TrimEnd('/');
            if (path == "/home/index")
            {
                path = "";
            }
            if (path.EndsWith("/index"))
            {
                path = path.Remove(path.Length - 6);
            }
            return url.Scheme + "://" + url.Authority + path + url.Query;
        }
    }
}
