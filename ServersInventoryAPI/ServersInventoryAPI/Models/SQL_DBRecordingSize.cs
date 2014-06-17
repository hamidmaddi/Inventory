using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class SQL_DBRecordingSize
    {
        public string servername { get; set; }
        public string instancename { get; set; }
        public string databasename { get; set; }
        public int databaseid { get; set; }
        public int totalrecordingsize { get; set; }
        public DateTime statdate { get; set; }
    }
}