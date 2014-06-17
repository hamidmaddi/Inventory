using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI1
{
    public class NetworkAdapaterConfiguration
    {
        public string DefaultIPGateway { get; set; }
        public string Description { get; set; }
        public bool DHCPEnabled { get; set; }
        public string DHCPServer { get; set; }
        public string DNSDomain { get; set; }
        public string DNSDomainSuffixSearchOrder { get; set; }
        public bool DNSEnabledForWINSResolution { get; set; }
        public string DNSServerSearchOrder { get; set; }
        public bool DomainDNSRegistrationEnabled { get; set; }
        public bool FUllDNSRegistrationEnabled { get; set; }
        public int Index { get; set; }
        public string IPAddress { get; set; }
        public int IPConnectionMetric { get; set; }
        public string IPSubnet { get; set; }
        public bool IPEnabled { get; set; }
        public string IPXAddress { get; set; }
        public bool IPXEnabled { get; set; }
        public string MACAddress { get; set; }
        public string ServiceName { get; set; }
        public string SettingId { get; set; }
        public int TCPIPNetBIOSOptions { get; set; }
        public bool WINSEnableLMHostsLookup { get; set; }
        public string WINSPrimaryServer { get; set; }
        public string WINSSecondaryServer { get; set; }
    }
}
