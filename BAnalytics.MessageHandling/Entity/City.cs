﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAnalytics.MessageHandling.Entity
{
    public class City : BaseEntity
    {
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
    }
}
