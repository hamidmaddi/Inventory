using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI1
{
    public class Update
    {
        public string HotFixID { get; set; }
        public string Title { get; set; }
    }

    public class PendingUpdate : Update
    {
        public string Severity { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class InstalledUpdate : Update
    {
        public DateTime InstalledDate { get; set; }
    }
}
