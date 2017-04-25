using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAnalytics.MQHelp
{
    public class MessageQuery
    {
        public static void Send(string filter, string value)
        {
            IMqHelp mq = MqHelpFactory.GetMqHelpInstance();
            mq.Send(filter, value);
        }

        public static void Receive(string clientId,string filter, Action<string> revMessage)
        {
            IMqHelp mq = MqHelpFactory.GetMqHelpInstance();
            mq.Receive(clientId, filter, revMessage);
        }
    }
}
