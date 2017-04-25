using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAnalytics.MQHelp
{
    public abstract class BaseMqHelp : IMqHelp
    {
        public abstract void Send(string filter, string value);

        public abstract void Receive(string clientId,string filter, Action<string> revMessage);
    }
}