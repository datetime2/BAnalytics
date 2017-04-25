using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using BAnalytics.MessageHandling.Dao;
using BAnalytics.MessageHandling.Entity;
using BAnalytics.MessageHandling.Model;
using BAnalytics.MessageHandling.Util;
using BAnalytics.MQHelp;
using log4net;
using RestSharp;
using Himall.Core.Helper;
using Newtonsoft.Json;
using UAParser;

namespace BAnalytics.MessageHandling
{
    public class PageViewProcess
    {
        private readonly NameValueCollection _kv = ConfigurationManager.GetSection("PageType") as NameValueCollection;
        private static readonly ILog LoggerPageView = LogManager.GetLogger("ub_pageview_log");
        static readonly Parser UaParser = Parser.GetDefault();
        private readonly string _clientId;
        private readonly WindowsCounter _counter = new WindowsCounter("pv");

        public PageViewProcess(string clientId)
        {
            _clientId = clientId;
        }

        public void Receive()
        {
            MessageQuery.Receive(_clientId,"pv", Handler);
        }

        private void Handler(string message)
        {
            try
            {
                RevMessage(message);
                _counter.Add();
            }
            catch (Exception ex)
            {
                LoggerPageView.Error(ex.Message, ex);
            }
        }

        private void RevMessage(string message)
        {
            PageView pv = JsonConvert.DeserializeObject<PageView>(message);
            //todo:ip地址解析
            AddressInfo addressInfo = ParserCity(pv.Ip);
            if (addressInfo.Status == 0)
            {
                //地理位置信息
                pv.CityName=addressInfo.Content.Detail.City;
                pv.CityId = addressInfo.Content.Detail.CityCode;
                pv.ProvinceName = addressInfo.Content.Detail.Province;
                pv.DistrictName = addressInfo.Content.Detail.District;
                pv.StreetName = addressInfo.Content.Detail.Street;
                pv.StreetNumber = addressInfo.Content.Detail.StreetNumber;
            }
            //todo:userid解析
            string[] loginUser = ParserUser(pv.User);
            if (loginUser[0] == "Web")
            {
                pv.LoginChannel = 1;
            }
            else if (loginUser[0] == "Mobile")
            {
                pv.LoginChannel = 2;
            }
            else
            {
                if (loginUser[1] == "-1")
                {
                    pv.LoginChannel = -1;
                }
                else
                {
                    pv.LoginChannel = 0;
                }
            }
            pv.MemberId = Convert.ToInt64(loginUser[1]);

            //todo:ua解析
            ClientInfo clientInfo = ParserUa(pv.UserAgent);

            //useragent信息
            pv.BrowserFamily=clientInfo.UserAgent.Family;
            pv.BrowserMajor=clientInfo.UserAgent.Major;
            pv.BrowserMinor=clientInfo.UserAgent.Minor;
            pv.OsFamily=clientInfo.OS.Family;
            pv.OsMajor=clientInfo.OS.Major;
            pv.OsMinor=clientInfo.OS.Minor;
            pv.DeviceFamily=clientInfo.Device.Family;
            pv.DeviceBrand=clientInfo.Device.Brand;
            pv.DeviceModel=clientInfo.Device.Model;

            //访问渠道
            int visitChannel = 1;
            Uri uri = new Uri(pv.Url);
            string path = uri.LocalPath.ToLower();
            if (Regex.IsMatch(path, string.Format("^({0})$",ConfigurationManager.AppSettings["MobileUrlPrefix"]))||
                Regex.IsMatch(path, string.Format("^({0})",ConfigurationManager.AppSettings["MobileUrlPrefix"].Replace("|","/|")+"/")))
            {
                visitChannel = 2;
            }
            pv.VisitChannel = visitChannel;

            //访问来源
            pv.VisitSource = 0;

            //判断页面类型
            bool flag = true;
            foreach (string key in _kv.Keys)
            {
                if (Regex.IsMatch(path, _kv[key].Replace("{MobileUrlPrefix}", ConfigurationManager.AppSettings["MobileUrlPrefix"]).ToLower()))
                {
                    flag = false;
                    pv.UrlTypeId = Convert.ToInt32(key);
                    break;
                }
            }
            if (flag)
            {
                pv.UrlTypeId = 0;
            }
            /*
            var session = new Session
            {
                UserId = Convert.ToInt64(loginUser[1]),
                AddressInfo = addressInfo,
                Ip = pv.Ip,
                Id = pv.Sid.ToString(),
                ClientInfo = clientInfo,
                SessionId = Convert.ToInt32(pv.Sid),
                UserAgent = pv.UserAgent,
                CreateTime = DateTime.Now,
                EncryptUserId = pv.User,
                FirstVisitTime = Convert.ToDateTime(pv.Time),
                LoginChannel = loginUser[0],
                UpdateTime = DateTime.Now,
                VisitorId = Convert.ToInt32(pv.Uid)
            };
            var visitor = new Visitor
            {
                Id = pv.Uid.ToString(),
                VisitorId = Convert.ToInt32(pv.Uid),
                VisitFirstTime = Convert.ToDateTime(pv.Time),
                VisitPreviousTime = Convert.ToDateTime(pv.Time),
                VisitLastTime = Convert.ToDateTime(pv.Time),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };*/
            //写日志、记录到mongodb
            WriteLog(pv);
            //SaveVisitorAsync(session, visitor);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="messageList"></param>
        /// <returns></returns>
        private void WriteLog(PageView pv)
        {
            WriteBehaviorLog(pv);
        }

        /// <summary>
        /// 写浏览行为日志
        /// </summary>
        /// <param name="messageList"></param>
        private void WriteBehaviorLog(PageView pv)
        {
            LoggerPageView.Info(pv);
        }

        private void SaveVisitorAsync(Session session, Visitor visitor)
        {
            var sessionDao = new SessionDao();
            sessionDao.GetAndSave(session.Id, () =>
            {
                //维护访问者数据
                var visitorDao = new VisitorDao();
                visitorDao.GetAndSave(visitor.Id, () =>
                {
                    //插入访问者
                    visitor.VisitCount = 1;
                    visitorDao.MongoHelper.InsertOne(visitor);
                    session.IsNewUser = true;
                    //执行插入会话数据
                    sessionDao.MongoHelper.InsertOne(session);
                }, v =>
                {
                    //修改访问者数据
                    v.VisitPreviousTime = v.VisitLastTime;
                    v.VisitLastTime = visitor.VisitLastTime;
                    v.UpdateTime = DateTime.Now;
                    v.VisitCount++;
                    visitorDao.MongoHelper.ReplaceOne(v);
                    session.IsNewUser = false;
                    //执行插入会话数据
                    sessionDao.MongoHelper.InsertOne(session);
                });
            }, s =>
            {
                //执行修改，第一次登录状态时将本次会话修改为已登录
                if (s.UserId == -1 && session.UserId != -1)
                {
                    s.UserId = session.UserId;
                    s.ClientInfo = session.ClientInfo;
                    s.UpdateTime = session.UpdateTime;
                    sessionDao.MongoHelper.ReplaceOne(session);
                }
            });
        }

        /// <summary>
        /// 调用百度ip定位api
        /// </summary>
        /// <param name="ip"></param>
        private static AddressInfo ParserCity(string ip)
        {
            ip = ip == "::1" ? "122.224.197.218" : ip;
            object data = HttpRuntime.Cache.Get(ip);
            if (data != null)
            {
                return (AddressInfo)data;
            }
            var client = new RestClient(ConfigurationManager.AppSettings["IpApiHost"]);
            var request = new RestRequest(ConfigurationManager.AppSettings["IpApiPath"], Method.GET);
            request.AddParameter("ak", ConfigurationManager.AppSettings["IpApiAccessKey"],
                ParameterType.UrlSegment);
            request.AddParameter("ip", ip, ParameterType.UrlSegment);
            var response = client.Execute<AddressInfo>(request);
            HttpRuntime.Cache.Insert(ip, response.Data, null, Cache.NoAbsoluteExpiration, new TimeSpan(0,0,1800));
            return response.Data;
        }

        private static string[] ParserUser(string user)
        {
            if (String.IsNullOrWhiteSpace(user))
            {
                return new[] {string.Empty, "-1"};
            }
            string u = SecureHelper.AESDecrypt(SecureHelper.DecodeBase64(user), ConfigurationManager.AppSettings["UserCookieDecryptKey"]);
            string[] us = u.Split(',');
            if (us.Length == 2)
            {
                long l;
                if (Int64.TryParse(us[1], out l))
                {
                    return u.Split(',');
                }
                return new[] {string.Empty, "-1"};
            } 
            return new[] {string.Empty, "-1"};
        }

        private static ClientInfo ParserUa(string ua)
        {
            ClientInfo c = UaParser.Parse(ua);
            return c;

        }
    }
}