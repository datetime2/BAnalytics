using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAnalytics.MessageHandling.Model;

namespace BAnalytics.MessageHandling.Entity
{
    public class IpMapping : BaseEntity
    {
        public string Ip { get; set; }
        public AddressInfo Address { get; set; }
    }
}
