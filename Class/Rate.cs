using System;
using System.Collections.Generic;
using System.Text;

namespace Exchange.Class
{
    public class Rate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public double ask { get; set; }
        public double bid { get; set; }
    }
}
