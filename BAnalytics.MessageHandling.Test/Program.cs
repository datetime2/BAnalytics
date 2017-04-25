using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Himall.Core.Helper;

namespace BAnalytics.MessageHandling.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine("正在启动");
            string _categoryName = "MQ_Process";
            string _counterTPSName = "TPS";
            Console.WriteLine("创建性能计数器");
            if (PerformanceCounterCategory.Exists(_categoryName))
            {
                PerformanceCounterCategory.Delete(_categoryName);
            }
            CounterCreationDataCollection ccdc = new CounterCreationDataCollection
            {
                new CounterCreationData(_counterTPSName, "TPS", PerformanceCounterType.RateOfCountsPerSecond32)
            };

            PerformanceCounterCategory.Create(_categoryName, "MQ_Process", PerformanceCounterCategoryType.MultiInstance,
                ccdc);

            Console.WriteLine("创建MQ监听服务");
            PageViewProcess mp = new PageViewProcess("PVListener");
            EventProcess ep = new EventProcess("EventListener");
            Task t1 = Task.Factory.StartNew(delegate { mp.Receive(); });
            Task t2 = Task.Factory.StartNew(delegate { ep.Receive(); });
            Console.WriteLine("开始处理消息");
            Task.WaitAll(t1, t2);
            Console.Read();*/
            Console.WriteLine(SecureHelper.AESDecrypt(SecureHelper.DecodeBase64("dWRPVnh2bnVwYXVmYmIwSytIdEd3QT09"), "d1b31e1b3176cf3aa8993428061c8af2"));
        }
    }
}
