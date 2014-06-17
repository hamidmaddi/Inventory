using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class OperatingSystem
    {
        public string Name { get; set; }
        public string SID { get; set; }
        public string Manufacturer { get; set; }
        public string  Caption { get; set; }
        public string  Version { get; set; }
        public string  CSDVersion { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime LastBOotUpTime {get; set;}
        public string SerialNumber { get; set; }
        public int ServicePackMajorVersion { get; set; }
        public string ProductType { get; set; }
        public string OSProductSuite {get; set;}
        public string OtherTypeDescription  { get; set; }
        public string Description { get; set; }
        public string OperatingSystemSKU  { get; set; }
        public string OSArchitecture  { get; set; }
        public int BuildNumber { get; set; }
        public string SystemDrive { get; set; }
        public string SystemDirectory { get; set; }
        public string WindowsDirectory { get; set; }
        public string  Organization{ get; set; }
        public DateTime LocalDateTime { get; set; }
        public string  OSType { get; set; }
        public string ActivationStatus { get; set; }
        public bool osRecoveryAutoReboot { get; set; }
        public string osRecoveryDebugInfoType { get; set; }
        public bool osRecoveryOverwriteExistingDebugFile { get; set; }
        public string osRecoveryExpandedDebugFilePath { get; set; }
        public string osRecoveryExpandedMiniDumpDirectory { get; set; }
   }
}
