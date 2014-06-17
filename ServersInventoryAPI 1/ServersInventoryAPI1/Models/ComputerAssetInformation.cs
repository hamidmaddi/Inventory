using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServersInventoryAPI1
{
    public class ComputerAssetInformation
    {
        public SystemInformation ServerInformation { get; set; }
        public OperatingSystem OperatingSystem { get; set; }
        public Processor[] Processors { get; set; }
        public Disk[] Disks { get; set; }        
        public Share[] Shares { get; set; }
        public SCSIController[] SCSIControllers { get; set; }
        public VideoController VideoController { get; set; }
        public NetworkAdapater[] NetworkAdapters { get; set; }              
        public NetworkAdapaterConfiguration[] NetworkAdapterConfigurations { get; set; }
        public Security Security { get; set; }
        public Update[] InstalledUpdates { get; set; }
        public Update[] PendingUpdates { get; set; }
        public SymantecEndpointProtection SEP { get; set; }
        public InstalledAppplication[] InstalledApplications { get; set; }
        public Feature[] Features { get; set; }
        public LocalAdministratorGroup[] lagMembers { get; set; }
        public LocalRemoteDesktopUserGroup[] lrduMembers { get; set; }
        public LocalPowerUsersGroup[] lpugMembers { get; set; }
    }
}