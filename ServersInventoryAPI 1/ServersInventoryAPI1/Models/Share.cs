using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI1
{
    public class Share
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Int32 MaximumAllowed { get; set; }
        public string Path { get; set; }
        public Boolean AllowMaximum { get; set; }
        public string ShareType { get; set; }
    }
}
