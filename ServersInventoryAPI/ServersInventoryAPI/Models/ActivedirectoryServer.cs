using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class ActivedirectoryServer
    {
        public string CN { get; set; }
	    public string RidSetReferences { get; set; }
	    public string OperatingSystemServicepack { get; set; }
	    public string OperatingSystem { get; set; }
	    public string OperatingSystemVersion { get; set; }
	    public DateTime LastLogonTimestamp { get; set; }
	    public DateTime WhenCreated { get; set; }
	    public string ADSpath { get; set; }
	    public DateTime WhenChanged { get; set; }
	    public string Memberof { get; set; }
	    public string DNShostname { get; set; }
	    public string DistinguishedName { get; set; }
	    public DateTime PWlastset { get; set; }
	    public string Publishedat { get; set; }
    }
}
