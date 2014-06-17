using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class SQL_CPUStat
    {
        public string servername { get; set; }
        public string instancename { get; set; }
        public DateTime stattime { get; set; }
        public int cpuidle { get; set; }
        public int cpusql { get; set; }
        public int cpuother { get; set; }
        public DateTime eventtime { get; set; }
    }
}
