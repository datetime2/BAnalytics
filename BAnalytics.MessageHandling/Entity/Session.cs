using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAnalytics.MessageHandling.Model;
using UAParser;

namespace BAnalytics.MessageHandling.Entity
{
    public class Session : BaseEntity
    {
        public int VisitorId { get; set; }
        public int SessionId { get; set; }
        public string Ip { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public string UserAgent { get; set; }
        public ClientInfo ClientInfo { get; set; }
        public string EncryptUserId { get; set; }
        public long UserId { get; set; }
        public string LoginChannel { get; set; }
        public DateTime FirstVisitTime { get; set; }
        public bool IsNewUser { get; set; }
    }
}
