using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BAnalytics.MessageHandling.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(ConfigurationManager.AppSettings["TraceFile"])); 
            Trace.AutoFlush = true;//每次写入日志后是否都将其保存到磁盘中
            //Console.WriteLine("正在启动");
            string _categoryName = "MQ_Process";
            string _counterTPSName = "TPS";
            //Console.WriteLine("创建性能计数器");
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

            //Console.WriteLine("创建MQ监听服务");
            PageViewProcess mp = new PageViewProcess("PVListener");
            EventProcess ep = new EventProcess("EventListener");
            Task t1 = Task.Factory.StartNew(delegate { mp.Receive(); });
            Task t2 = Task.Factory.StartNew(delegate { ep.Receive(); });
            //Console.WriteLine("开始处理消息");
            //Task.WaitAll(t1, t2);
            //Console.Read();
        }



        protected override void OnStop()
        {
        }
    }
}
