using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class NetworkStat

    {
        public string Name { get; set; }
        public int PID { get; set; }
        public string ProcessName { get; set; }
        public string Protocol { get; set; }
        public string LocalAddress { get; set; }
        public int LocalPort { get; set; }
        public string RemoteAddress { get; set; }
        public int RemotePort{ get; set; }
        public string State { get; set; }
    }
}
