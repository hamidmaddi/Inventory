using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class VideoController
    {
        public string Name { get; set; }
        public string DeviceID { get; set; }
        public string AdapterCompatibility { get; set; }
        public string InstalledDisplayDrivers { get; set; }
        public string DriverVersion { get; set; }
        public DateTime DriverDate { get; set; }
        public string InfFilename { get; set; }
        public string PNPDeviceID { get; set; }
    }
}
