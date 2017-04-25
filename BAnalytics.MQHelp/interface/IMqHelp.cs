using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAnalytics.MQHelp
{
    public interface IMqHelp
    {
        /// <summary>
        /// 发送消息到消息队列
        /// </summary>
        /// <param name="value"></param>
        void Send(string filter, string value);
        void Receive(string clientId,string filter, Action<string> revMessage);
    }
}
