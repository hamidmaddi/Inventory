using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class Processor
    {
        public string DeviceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Family { get; set; }
        public int AddressWidth { get; set; }
        public string ProcessorId { get; set; }
        public string Status { get; set; }
        public int DataWidth { get; set; }
        public int ExternalClock { get; set; }
        public int L2CacheSize { get; set; }
        public int MaxClockSpeed { get; set; }
        public int Revision { get; set; }
        public string SocketDesignation { get; set; }
        public string Architecture { get; set; }
        public int CurrentClockSpeed { get; set; }
        public int NumberOfCores { get; set; }
    }
}
