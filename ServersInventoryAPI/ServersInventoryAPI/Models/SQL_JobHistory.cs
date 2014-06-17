using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class SQL_JobHistory
    {
        public string job_server { get; set; }
        public string instance_name { get; set; }
        public int instance_id { get; set; }
        public string job_id { get; set; }
        public string job_name { get; set; }
        public int step_id { get; set; }
        public string step_name { get; set; }
        public int sql_message_id { get; set; }
        public int sql_severity { get; set; }
        public string job_step_message { get; set; }
        public int run_status { get; set; }
        public int run_date { get; set; }
        public int run_time { get; set; }
        public int run_duration { get; set; }
        public string operator_emailed { get; set; }
        public string operator_netsent { get; set; }
        public string operator_paged { get; set; }
        public int retries_attempted { get; set; }
        public DateTime job_start_datetime { get; set; }
    }
}


