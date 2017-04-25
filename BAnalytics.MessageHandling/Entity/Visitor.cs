using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAnalytics.MessageHandling.Entity
{
    public class Visitor : BaseEntity
    {
        public int VisitorId { get; set; }

        /// <summary>
        /// 第一次访问时间
        /// </summary>
        public DateTime VisitFirstTime { get; set; }

        /// <summary>
        /// 前一次访问时间
        /// </summary>
        public DateTime VisitPreviousTime { get; set; }

        /// <summary>
        /// 最后一次访问时间
        /// </summary>
        public DateTime VisitLastTime { get; set; }

        /// <summary>
        /// 访问次数
        /// </summary>
        public int VisitCount { get; set; }
    }
}
