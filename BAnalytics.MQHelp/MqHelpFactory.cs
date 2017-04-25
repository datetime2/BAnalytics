using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAnalytics.MQHelp
{
    public class MqHelpFactory
    {
        public static BaseMqHelp GetMqHelpInstance()
        {
            switch (ConfigurationManager.AppSettings["MQType"])
            {
                case "ActiveMQ":
                    return ActiveMqHelp.CreateInstance();
                default:
                    return null;
            }
        }
    }
}
