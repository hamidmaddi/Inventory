using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServersInventoryAPI
{
    public class InstalledAppplication
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public int ServiceUpdate { get; set; }
        public string Vendor { get; set; }
        public DateTime InstallDate { get; set; }
    }
}