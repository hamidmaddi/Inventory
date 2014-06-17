using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class SymantecEndpointProtection
    {
        public bool IsInstalled { get; set; }
        public string Version { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime SyncDate { get; set; }
    }
}
