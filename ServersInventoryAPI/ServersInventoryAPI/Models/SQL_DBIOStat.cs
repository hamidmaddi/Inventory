using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class SQL_DBIOStat
    {
        public string servername { get; set; }
        public string instancename { get; set; }
        public string database_name { get; set; }
        public int database_id { get; set;}
        public int read_wait_ms { get; set;}
        public int reads { get; set;}
        public int write_wait_ms { get; set;}
        public int writes { get; set;}
        public DateTime stat_time { get; set; }
    }
}