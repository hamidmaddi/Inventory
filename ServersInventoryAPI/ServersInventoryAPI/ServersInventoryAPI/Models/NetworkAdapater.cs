using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class NetworkAdapater
    {
        public string Name { get; set; }
        public int DeviceID { get; set; }
        public string NetConnectionID { get; set; }
        public string PNPDeviceId { get; set; }
        public string AdapterType { get; set; }
        public string MACAddress { get; set; }
        public string Manufacturer { get; set; }
        public bool PromiscuousMode { get; set; }
        public string ConnectionStatus { get; set; }

    }
}
