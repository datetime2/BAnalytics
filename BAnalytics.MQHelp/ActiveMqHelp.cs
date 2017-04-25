using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace BAnalytics.MQHelp
{
    public class ActiveMqHelp : BaseMqHelp, IMqHelp
    {
        private readonly IConnectionFactory _factory;
        private static volatile ActiveMqHelp _instance = null;
        private static readonly object _lockHelper = new object();

        public static ActiveMqHelp CreateInstance()
        {
            if (_instance == null)
            {
                lock (_lockHelper)
                {
                    if (_instance == null)
                        _instance = new ActiveMqHelp();
                }
            }
            return _instance;
        }

        private ActiveMqHelp()
        {
            _factory = new ConnectionFactory(ConfigurationManager.AppSettings["ActiveMQConnectionString"]);
        }

        public override void Send(string filter, string value)
        {
            //通过工厂建立连接
            using (IConnection connection = _factory.CreateConnection())
            {
                //通过连接创建Session会话
                using (ISession session = connection.CreateSession())
                {
                    //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                    IMessageProducer prod =
                        session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(filter));
                    //创建一个发送的消息对象
                    ITextMessage message = prod.CreateTextMessage();
                    //给这个对象赋实际的消息
                    message.Text = value;
                    //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                    //message.Properties.SetString("filter", filter);
                    //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消息优先级别，发送最小单位，当然还有其他重载
                    prod.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
                }
            }
        }

        public override void Receive(string clientId, string filter, Action<string> revMessage)
        {
            //通过工厂建立连接
            using (IConnection connection = _factory.CreateConnection())
            {
                connection.ClientId = clientId;
                connection.Start();
                //通过连接创建Session会话
                using (ISession session = connection.CreateSession())
                {
                    IMessageConsumer consumer =
                        session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(filter)/*,"filter='" + filter + "'"*/);
                    //注册监听事件
                    consumer.Listener += (message => revMessage(((ITextMessage)message).Text));
                    Thread.Sleep(Timeout.Infinite);
                }
                connection.Stop();
                connection.Close();  
            }
        }
    }
}
