using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI1
{
    public class Service
    {
	    public string Name { get; set; }
	    public string Status { get; set; }
	    public string PathName { get; set; }
	    public string ServiceType { get; set; }
	    public string StartMode { get; set; }
	    public bool AcceptPause { get; set; }
	    public bool AcceptStop { get; set; }
	    public string Description { get; set; }
	    public string DisplayName { get; set; }
	    public int ProcessId { get; set; }
	    public bool Started { get; set; }
	    public string StartName { get; set; }
	    public string State { get; set; }
        public string Path { get; set; }
    }
}
