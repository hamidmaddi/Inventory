using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class SQL_Backup
    {
        public string servername { get; set; }
        public string instancename { get; set; }
        public string dbname { get; set; }
        public DateTime statdate { get; set; }
        public int ident_type_id { get; set; }
        public string statvalue { get; set; }
    }
}
    