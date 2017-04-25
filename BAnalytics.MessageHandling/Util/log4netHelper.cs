using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAnalytics.MessageHandling.Util
{
    /// <summary>
    /// log4net 日志处理
    /// </summary>
    public class log4netHelper
    {
        /// <summary>
        /// Error 信息记录
        /// </summary>
        /// <param name="strMessage">记录内容</param>
        public static void Error(string strMessage)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Error");
            if (log.IsErrorEnabled)
            {
                log.Error(strMessage);
            }
            log = null;
        }

        /// <summary>
        /// Debug 信息记录
        /// </summary>
        /// <param name="strMessage">记录内容</param>
        public static void Debug(string strMessage)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Debug");
            if (log.IsErrorEnabled)
            {
                log.Error(strMessage);
            }
        }

        /// <summary>
        /// Info 信息记录
        /// </summary>
        /// <param name="message">记录内容</param>
        public static void Info(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Info");
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
            log = null;
        }
    }
}
