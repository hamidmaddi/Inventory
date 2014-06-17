using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class SystemInformation
    {
        public DateTime InventoryDate { get; set; }
        public string server_id { get; set; }
        public string EnclosureSerialNumber { get; set; }
        public string EnclosureChassisType { get; set; }
        public string EnclosurePath { get; set; }
        public string EnclosureManufacturer { get; set; }
        public string BiosName { get; set; }
        public string BiosVersion { get; set; }
        public DateTime BiosReleaseDate { get; set; }
        public string BiosSMBIOSBIOSVersion { get; set; }
        public string BiosSMBIOSMajorVersion { get; set; }
        public string BiosSMBIOSMinorVersion { get; set; }
        public int PhysicalMemorytotalDIMMs { get; set; }
        public int PhysicalMemorytotalDIMMsFree { get; set; }
        public int PhysicalMemoryTotal { get; set; }
        public int PhysicalMemoryFree { get; set; }
        public float PhysicalMemoryPercentUsed { get; set; }
        public int VirtualMemoryTotal { get; set; }
        public int VirtualMemoryFree { get; set; }
        public int CPUCorecount { get; set; }
        public int CPUCount { get; set; }
        public string SystemName { get; set; }
        public string SystemDomain { get; set; }
        public string SystemFQDN { get; set; }
        public string SystemIPAddress { get; set; }
        public string SystemDomainRole { get; set; }
        public string SystemUUID { get; set; }
        public string SystemType { get; set; }
        public string SystemDescription { get; set; }
        public string SystemManufacturer { get; set; }
        public string SystemModel { get; set; }
        public string systemProductID { get; set; }
        public string SystemTimeZone { get; set; }
        public bool SystemPendingReboot { get; set; }
        public string systemRDPconfiguration { get; set; }
	    public string systemCurrentCulture { get; set; }
	    public bool systemFirewallEnabled { get; set; }
	    public string systemMonitoringAgent { get; set; }
	    public string systemBackupAgent { get; set; }
        //public DateTime SystemWSUSSync { get; set; }
        public string SystemUptime { get; set; }
        public bool IsVirtual { get; set; }
        public string VM_Type { get; set; }
        public string VM_PhysicalHostName { get; set; }
        public string Cluster { get; set; }
        public string IPConfig { get; set; }
        public string Routes { get; set; } 
    }
}
