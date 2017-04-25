using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAnalytics.MessageHandling.Util
{
    public class WindowsCounter
    {
        private readonly PerformanceCounter _counterTps;
        private readonly string _categoryName = "MQ_Process";
        private readonly string _counterTPSName = "TPS";
        private string _instanceName;

        public WindowsCounter(string instanceName)
        {
            _instanceName = instanceName;
            //初始化计数器实例
            _counterTps = new PerformanceCounter
            {
                CategoryName = _categoryName,
                CounterName = _counterTPSName,
                InstanceName = instanceName,
                InstanceLifetime = PerformanceCounterInstanceLifetime.Process,
                ReadOnly = false,
                RawValue = 0
            };
            _counterTps.NextSample();
        }

        public void Add()
        {
            _counterTps.Increment();
        }
    }
}