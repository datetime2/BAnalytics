using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BAnalytics.MessageHandling.Dao;
using BAnalytics.MessageHandling.Entity;
using BAnalytics.MessageHandling.Model;
using BAnalytics.MessageHandling.Util;
using BAnalytics.MessageHandling.Model;
using BAnalytics.MQHelp;
using log4net;
using RestSharp;
using Himall.Core.Helper;
using Newtonsoft.Json;
using UAParser;

namespace BAnalytics.MessageHandling
{
    public class EventProcess
    {
        private static readonly ILog LoggerEvent = LogManager.GetLogger("ub_event_log");
        private readonly string _clientId;
        private readonly WindowsCounter _counter = new WindowsCounter("event");

        public EventProcess(string clientId)
        {
            _clientId = clientId;
        }

        public void Receive()
        {
            MessageQuery.Receive(_clientId, "event", Handler);
        }

        private void Handler(string message)
        {
            try
            {
                Event e = JsonConvert.DeserializeObject<Event>(message);
                RevMessage(e);
                _counter.Add();
            }
            catch (Exception ex)
            {
                LoggerEvent.Error(ex.Message, ex);
            }
        }

        private void RevMessage(Event e)
        {
            WriteLog(e);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="messageList"></param>
        /// <returns></returns>
        private void WriteLog(Event e)
        {
            LoggerEvent.Info(e);
        }
    }
}