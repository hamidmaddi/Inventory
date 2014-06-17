using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class Security
    {
        public Security()
        {
            AdminGroup = new List<string>();
            PowerUser = new List<string>();
            RemoteDesktopUser = new List<string>();
        }

        public List<string> AdminGroup { get; set; }
        public List<string> PowerUser { get; set; }
        public List<string> RemoteDesktopUser { get; set; }
    }
}
