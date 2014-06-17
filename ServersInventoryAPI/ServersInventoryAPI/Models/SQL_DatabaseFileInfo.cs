using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class SQL_DatabaseFileInfo
    {
        public string servername { get; set; }
        public string instancename { get; set; }
        public int database_id { get; set; }
        public int db_file_id { get; set; }
        public string db_file_type { get; set; }
        public string database_name { get; set; }
        public string db_file_name { get; set; }
        public int db_file_size { get; set; }
        public int db_file_used { get; set; }
        public DateTime stat_time { get; set; }
        public int autogrow  { get; set; }
        public bool autogrow_pct { get; set; }
    }
}

