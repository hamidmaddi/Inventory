using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class SQL_InstanceStat
    {
        public string servername  { get; set; }
        public string instancename { get; set; }
        public DateTime statdate { get; set; }
        public int ident_type_id { get; set; }
        public string statvalue { get; set; }
    }
}
