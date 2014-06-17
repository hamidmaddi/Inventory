using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI1
{
    public class WUsettings
    {
        public DateTime DownloadLastSuccessTime { get; set; }
        public DateTime InstallLastSuccessTime { get; set; }
        public DateTime wuDetectLastSuccessTime { get; set; }
        public int wuDetectionFrequency { get; set; }
        public string wuAutomaticUpdatesNotification { get; set; }
        public string wuInstallFrequency { get; set; }
        public string wuInstallTime { get; set; }
        public string wuWSUSserver { get; set; }
        public string wuWSUSstatusURL { get; set; }
        public string wuTargetGroup { get; set; }
        public bool wuOptedinMicrosoftUpdate { get; set; }
        public bool wuTargetGroupEnabled { get; set; }
        public bool wuAutomaticUpdateEnabled { get; set; }
        public bool UseWSUSserver { get; set; }
        
   }
}
