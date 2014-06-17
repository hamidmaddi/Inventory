using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class IPV4RouteTable
    {
        public string NetworkAddress { get; set; }
        public string Netmask { get; set; }
        public string GatewayAddress { get; set; }
        public string Metric { get; set; }
    }
}
