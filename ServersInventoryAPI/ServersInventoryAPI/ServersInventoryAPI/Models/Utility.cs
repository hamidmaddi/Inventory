using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServersInventoryAPI
{
    public static class Utility
    {
        public static DateTime DefaultDate = new DateTime(0001, 1, 1); // 1/1/1753 
        public static int DefaultInt = -1;
        public static bool DefaultBoolean = false;
        public static float DefaultFloat = -1;

        public static T ConvertValue<T>(string value, T defaultValue)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return defaultValue;
                else
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch
            {
                return defaultValue;
            }

        }

        public static float? CheckFloatValue(float value)
        {
            try
            {
                if (value == -1)
                    return null;
                else
                {
                    return value;
                }
            }
            catch
            {
                return null;
            }

        }

        public static Int32? CheckIntValue(Int32 value)
        {
            try
            {
                if (value == -1)
                    return null;
                else
                {
                    return value;
                }
            }
            catch
            {
                return null;
            }

        }

        public static DateTime? CheckDateTimeValue(DateTime value)
        {
            try
            {
                if (value == DateTime.MinValue)
                    return null;
                else
                {
                    return value;
                }
            }
            catch
            {
                return null;
            }

        }
       
        public static Int32 DiscoverServer(SystemInformation sysInfo, OperatingSystem osInfo)
        {
            Int32 srvId = -1;
          try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var p = new DynamicParameters();
                        p.Add("@server_FQDN", sysInfo.SystemFQDN);
                        p.Add("@os_SID", osInfo.SID);
                        p.Add("@out",dbType:  System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                        connection.Execute(sql: "usp_DiscoverServer", param: p, commandType: System.Data.CommandType.StoredProcedure);
                        srvId = p.Get<Int32>("@out");
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_DiscoverServer>", ex);
            }
            return srvId;
        }

        public static bool InsertSystemInfo(SystemInformation sysInfo, OperatingSystem osInfo, WUsettings WUsettings, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    //ServerInformation
                    var si = new DynamicParameters();
                    si.Add("@server_id", serverId);
                    si.Add("@enclosureSerialNumber", sysInfo.EnclosureSerialNumber);
                    si.Add("@enclosureManufacturer", sysInfo.EnclosureManufacturer);
                    si.Add("@enclosureChassisType", sysInfo.EnclosureChassisType);
                    si.Add("@enclosurePath", sysInfo.EnclosurePath);
                    si.Add("@biosName", sysInfo.BiosName);
                    si.Add("@biosVersion", sysInfo.BiosVersion);
                    si.Add("@biosReleaseDate", Utility.CheckDateTimeValue(sysInfo.BiosReleaseDate));
                    si.Add("@biosSMBIOSBIOSVersion", sysInfo.BiosSMBIOSBIOSVersion);
                    si.Add("@biosSMBIOSMajorVersion", sysInfo.BiosSMBIOSMajorVersion);
                    si.Add("@biosSMBIOSMinorVersion", sysInfo.BiosSMBIOSMinorVersion);
                    si.Add("@systemTimeZone", sysInfo.SystemTimeZone);
                    si.Add("@systemManufacturer", sysInfo.SystemManufacturer);
                    si.Add("@systemModel", sysInfo.SystemModel);
                    si.Add("@systemProductID", sysInfo.systemProductID);
                    si.Add("@systemName", sysInfo.SystemName);
                    si.Add("@systemDomain", sysInfo.SystemDomain);
                    si.Add("@systemFQDN", sysInfo.SystemFQDN);
                    si.Add("@systemIPAddress", sysInfo.SystemIPAddress);
                    si.Add("@systemDomainRole", sysInfo.SystemDomainRole);
                    si.Add("@systemType", sysInfo.SystemType);
                    si.Add("@systemDescription", sysInfo.SystemDescription);
                    si.Add("@systemUUID", sysInfo.SystemUUID);
                    si.Add("@systemUpTime", sysInfo.SystemUptime);
                    si.Add("@systemPendingReboot", sysInfo.SystemPendingReboot);
                    si.Add("@systemRDPconfiguration", sysInfo.systemRDPconfiguration);
                    si.Add("@systemCurrentCulture", sysInfo.systemCurrentCulture);
                    si.Add("@systemFirewallEnabled", sysInfo.systemFirewallEnabled);
                    si.Add("@systemMonitoringAgent", sysInfo.systemMonitoringAgent);
                    si.Add("@systemBackupAgent", sysInfo.systemBackupAgent);
                    si.Add("@isVirtual", sysInfo.IsVirtual);
                    si.Add("@vm_Type", sysInfo.VM_Type);
                    si.Add("@vm_Physicalhostname", sysInfo.VM_PhysicalHostName);
                    si.Add("@parentCluster", sysInfo.Cluster);
                    si.Add("@PhysicalMemorytotalDIMMs", Utility.CheckIntValue(sysInfo.PhysicalMemorytotalDIMMs));
                    si.Add("@PhysicalMemorytotalDIMMsFree", Utility.CheckIntValue(sysInfo.PhysicalMemorytotalDIMMsFree));
                    si.Add("@physicalMemorytotal", Utility.CheckIntValue(sysInfo.PhysicalMemoryTotal));
                    si.Add("@physicalMemoryfree", Utility.CheckIntValue(sysInfo.PhysicalMemoryFree));
                    si.Add("@physicalMemorypercentused", Utility.CheckFloatValue(sysInfo.PhysicalMemoryPercentUsed));
                    si.Add("@virtualMemorytotal", Utility.CheckIntValue(sysInfo.VirtualMemoryTotal));
                    si.Add("@virtualMemoryfree", Utility.CheckIntValue(sysInfo.VirtualMemoryFree));
                    si.Add("@cpuCorecount", Utility.CheckIntValue(sysInfo.CPUCorecount));
                    si.Add("@cpuCount", Utility.CheckIntValue(sysInfo.CPUCount));
                    si.Add("@IPConfig", sysInfo.IPConfig.Replace("|","\n"));
                    si.Add("@Routes", sysInfo.Routes.Replace("|","\n"));
                    
                    //Operating System
                    si.Add("@osManufacturer", osInfo.Manufacturer);
                    si.Add("@osName", osInfo.Name);
                    si.Add("@os_SID", osInfo.SID);
                    si.Add("@osCaption", osInfo.Caption);
                    si.Add("@osVersion", osInfo.Version);
                    si.Add("@osCSDVersion", osInfo.CSDVersion);
                    si.Add("@osInstallDate", Utility.CheckDateTimeValue(osInfo.InstallDate));
                    si.Add("@osLastBootUpTime", Utility.CheckDateTimeValue(osInfo.LastBOotUpTime));
                    si.Add("@osSerialNumber", osInfo.SerialNumber);
                    si.Add("@osServicePackMajorVersion", osInfo.ServicePackMajorVersion);
                    si.Add("@osProductType", osInfo.ProductType);
                    si.Add("@osProductSuite", osInfo.OSProductSuite);
                    si.Add("@osOtherTypeDescription", osInfo.OtherTypeDescription);
                    si.Add("@osDescription", osInfo.Description);
                    si.Add("@osOperatingSystemSKU", osInfo.OperatingSystemSKU);
                    si.Add("@osOSArchitecture", osInfo.OSArchitecture);
                    si.Add("@osBuildNumber", Utility.CheckIntValue(osInfo.BuildNumber));
                    si.Add("@osSystemDrive", osInfo.SystemDrive);
                    si.Add("@osSystemDirectory", osInfo.SystemDirectory);
                    si.Add("@osWindowsDirectory", osInfo.WindowsDirectory);
                    si.Add("@osOrganization", osInfo.Organization);
                    si.Add("@osLocalDateTime", Utility.CheckDateTimeValue(osInfo.LocalDateTime));
                    si.Add("@osType", osInfo.OSType);
                    si.Add("@osActivationStatus", osInfo.ActivationStatus);
                    si.Add("@osRecoveryAutoReboot", osInfo.osRecoveryAutoReboot);
                    si.Add("@osRecoveryDebugInfoType", osInfo.osRecoveryDebugInfoType);
                    si.Add("@osRecoveryOverwriteExistingDebugFile", osInfo.osRecoveryOverwriteExistingDebugFile);
                    si.Add("@osRecoveryExpandedDebugFilePath", osInfo.osRecoveryExpandedDebugFilePath);
                    si.Add("@osRecoveryExpandedMiniDumpDirectory", osInfo.osRecoveryExpandedMiniDumpDirectory);
                    
                    //Windows Update Settings
                    si.Add("@wuUseWSUSserver", WUsettings.UseWSUSserver);
                    si.Add("@wuDownloadLastSuccessTime", Utility.CheckDateTimeValue(WUsettings.DownloadLastSuccessTime));
                    si.Add("@wuInstallLastSuccessTime", Utility.CheckDateTimeValue(WUsettings.InstallLastSuccessTime));
                    si.Add("@wuDetectionFrequency", Utility.CheckIntValue(WUsettings.wuDetectionFrequency));
                    si.Add("@wuAutomaticUpdatesNotification", WUsettings.wuAutomaticUpdatesNotification);
                    si.Add("@wuDetectLastSuccessTime", Utility.CheckDateTimeValue(WUsettings.wuDetectLastSuccessTime));
                    si.Add("@wuAutomaticUpdateEnabled", WUsettings.wuAutomaticUpdateEnabled);
                    si.Add("@wuInstallFrequency", WUsettings.wuInstallFrequency);
                    si.Add("@wuInstallTime", WUsettings.wuInstallTime);
                    si.Add("@wuWSUSserver", WUsettings.wuWSUSserver);
                    si.Add("@wuWSUSstatusURL", WUsettings.wuWSUSstatusURL);
                    si.Add("@wuTargetGroupEnabled", WUsettings.wuTargetGroupEnabled);
                    si.Add("@wuTargetGroup", WUsettings.wuTargetGroup);
                    si.Add("@wuOptedinMicrosoftUpdate", WUsettings.wuOptedinMicrosoftUpdate);

                    connection.Execute("usp_ins_ServerInformation", si, null, null, System.Data.CommandType.StoredProcedure);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_ServerInformation>", ex);
            }
            return result;
        }

        public static bool InsertProcessors(List<Processor> procs, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var Processor = new DynamicParameters();
                    foreach (var cpu in procs)
                    {
                        Processor.Add("@server_id", serverId);
                        Processor.Add("@DeviceID", cpu.DeviceID);
                        Processor.Add("@Name", cpu.Name);
                        Processor.Add("@Description", cpu.Description);
                        Processor.Add("@Manufacturer", cpu.Manufacturer);
                        Processor.Add("@Family", cpu.Family);
                        Processor.Add("@ProcessorId", cpu.ProcessorId);
                        Processor.Add("@Status", cpu.Status);
                        Processor.Add("@AddressWidth", Utility.CheckIntValue(cpu.AddressWidth));
                        Processor.Add("@DataWidth", Utility.CheckIntValue(cpu.DataWidth));
                        Processor.Add("@ExternalClock", Utility.CheckIntValue(cpu.ExternalClock));
                        Processor.Add("@L2CacheSize", Utility.CheckIntValue(cpu.L2CacheSize));
                        Processor.Add("@MaxClockSpeed", Utility.CheckIntValue(cpu.MaxClockSpeed));
                        Processor.Add("@Revision", Utility.CheckIntValue(cpu.Revision));
                        Processor.Add("@SocketDesignation", cpu.SocketDesignation);//string
                        Processor.Add("@Architecture", cpu.Architecture);
                        Processor.Add("@CurrentClockSpeed", Utility.CheckIntValue(cpu.CurrentClockSpeed));
                        Processor.Add("@NumberOfCores", Utility.CheckIntValue(cpu.NumberOfCores));
                        connection.Execute("usp_ins_Processor", Processor, null, null, System.Data.CommandType.StoredProcedure);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_Processor>", ex);
            }
            return result;
        }

        public static bool InsertPageFiles(List<PageFile> pagefiles, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var pagefile = new DynamicParameters();
                    foreach (var pf in pagefiles)
                    {
                        pagefile.Add("@server_id" , serverId);
                        pagefile.Add("@Name", pf.Name);
                        pagefile.Add("@FileSize", Utility.CheckIntValue(pf.FileSize));
	                    pagefile.Add("@FileType", pf.FileType);
	                    pagefile.Add("@Compressed", pf.Compressed);
	                    pagefile.Add("@CompressionMethod", pf.CompressionMethod);
	                    pagefile.Add("@Encrypted", pf.Encrypted);
	                    pagefile.Add("@EncryptionMethod", pf.EncryptionMethod);
	                    pagefile.Add("@Hidden", pf.Hidden);
	                    pagefile.Add("@InstallDate", Utility.CheckDateTimeValue(pf.InstallDate));
	                    pagefile.Add("@LastAccessed", Utility.CheckDateTimeValue(pf.LastAccessed));
	                    pagefile.Add("@LastModified", Utility.CheckDateTimeValue(pf.LastModified));
                        connection.Execute("usp_ins_PageFile", pagefile, null, null, System.Data.CommandType.StoredProcedure);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_PageFile>", ex);
            }
            return result;
        }

        public static bool InsertDisks(List<Disk> disks, Int32 serverId)
        {
             bool result = false;
             try
             {
                 using (var connection = ConnectionManager.Create())
                 {
                     var Disk = new DynamicParameters();
                     foreach (var disk in disks)
                     {
                         Disk.Add("@server_id", serverId);
                         Disk.Add("@Disk", disk.PhysicalDisk);
                         Disk.Add("@Model", disk.Model);
                         Disk.Add("@Partition", disk.Partition);
                         Disk.Add("@Description", disk.Description);
                         Disk.Add("@PrimaryPartition", disk.PrimaryPartition);
                         Disk.Add("@VolumeName", disk.VolumeName);
                         Disk.Add("@Drive", disk.Drive);
                         Disk.Add("@DiskSize", Utility.CheckIntValue(disk.DiskSize));
                         Disk.Add("@FreeSpace", Utility.CheckIntValue(disk.FreeSpace));
                         Disk.Add("@PercentageFree", Utility.CheckFloatValue(disk.PercentageFree));
                         Disk.Add("@DiskType", disk.DiskType);
                         Disk.Add("@SerialNumber", disk.SerialNumber);
                         connection.Execute("usp_ins_Disk", Disk, null, null, System.Data.CommandType.StoredProcedure);
                     }
                     result = true;
                 }
             }
             catch (Exception ex)
             {
                 LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_Disk>", ex);
             }
             return result;
        }

        public static bool InsertMPIOpaths(List<MPIO> mpiopaths, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var mpio = new DynamicParameters();
                    foreach (var nb in mpiopaths)
                    {
                        mpio.Add("@server_id", serverId);
                        mpio.Add("@Name", nb.Name);
                        mpio.Add("@Numberpaths", Utility.CheckIntValue(nb.Numberpaths));
                        connection.Execute("usp_ins_MPIO", mpio, null, null, System.Data.CommandType.StoredProcedure);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_MPIO>", ex);
            }
            return result;
        }
        
        public static bool InsertSCSIControllers(List<SCSIController> SCSIControllers, Int32 serverId )
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var scsiContrl = new DynamicParameters();
                    foreach (var scsi in SCSIControllers)
                    {
                        scsiContrl.Add("@server_id", serverId);
                        scsiContrl.Add("@DeviceID", scsi.DeviceID);
                        scsiContrl.Add("@Name", scsi.Name);
                        scsiContrl.Add("@Manufacturer", scsi.Manufacturer);
                        scsiContrl.Add("@DriverName", scsi.DriverName);
                        connection.Execute("usp_ins_SCSIController", scsiContrl, null, null, System.Data.CommandType.StoredProcedure);
                    }
                }
                    result = true;
            }
            
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_SCSIController>", ex);
            }
            return result;
        }

        public static bool InsertShares(List<Share> shares, Int32 serverId)
        {
             bool result = false;
                try
                {
                    using (var connection = ConnectionManager.Create())
                    {
                        var Share = new DynamicParameters();
                        foreach (var share in shares)
                        {
                            Share.Add("@server_id", serverId);
	                        Share.Add("@Name", share.Name);
	                        Share.Add("@Path", share.Path);
	                        Share.Add("@Description", share.Description);
                            Share.Add("@MaximumAllowed", Utility.CheckIntValue(share.MaximumAllowed));
	                        Share.Add("@AllowMaximum", share.AllowMaximum);
	                        Share.Add("@ShareType", share.ShareType);

                            connection.Execute("usp_ins_Share", Share, null, null, System.Data.CommandType.StoredProcedure);
                        }
                    }
                        result = true;
                   }
                        catch (Exception ex)
                        {
                            LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_Share>", ex);
                        }
                        return result;
            }

        public static bool InsertVideoController(VideoController videocontroller, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var vc = new DynamicParameters();
                    vc.Add("@server_id", serverId);
                    vc.Add("@Name", videocontroller.Name);
                    vc.Add("@DeviceID", videocontroller.DeviceID);
                    vc.Add("@AdapterCompatibility", videocontroller.AdapterCompatibility);
                    vc.Add("@InstalledDisplayDrivers", videocontroller.InstalledDisplayDrivers);
                    vc.Add("@DriverVersion", videocontroller.DriverVersion);
                    vc.Add("@DriverDate", Utility.CheckDateTimeValue(videocontroller.DriverDate));
                    vc.Add("@InfFilename", videocontroller.InfFilename);
                    vc.Add("@PNPDeviceID", videocontroller.PNPDeviceID);
                    connection.Execute("usp_ins_VideoController", vc, null, null, System.Data.CommandType.StoredProcedure);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_VideoController>", ex);
            }
            return result;
        }

        public static bool InsertNetworkAdapters(List<NetworkAdapater> networksadapters, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var netadp = new DynamicParameters();
                    foreach (var na in networksadapters)
                    {
                        netadp.Add("@server_id", serverId);
                        netadp.Add("@Name", na.Name);
                        netadp.Add("@DeviceID", Utility.CheckIntValue(na.DeviceID));
                        netadp.Add("@NetConnectionID", na.NetConnectionID);
                        netadp.Add("@PNPDeviceId", na.PNPDeviceId);
                        netadp.Add("@AdapterType", na.AdapterType);
                        netadp.Add("@MACAddress", na.MACAddress);
                        netadp.Add("@Manufacturer", na.Manufacturer);
                        netadp.Add("@PromiscuousMode", na.PromiscuousMode);
                        netadp.Add("@ConnectionStatus", na.ConnectionStatus);

                        connection.Execute("usp_ins_NetworkAdapter", netadp, null, null, System.Data.CommandType.StoredProcedure);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_NetworkAdapters>", ex);
            }
            return result;
        }

        public static bool InsertNICBinding(List<NICbinding> nicbindingorders, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var nicb = new DynamicParameters();
                    foreach (var nb in nicbindingorders)
                    {
                        nicb.Add("@server_id", serverId);
                        nicb.Add("@BindingOrder", Utility.CheckIntValue(nb.BindingOrder));
                        nicb.Add("@Name", nb.Name);
                        nicb.Add("@NICenabled", nb.NICenabled);
                        nicb.Add("@GUID", nb.GUID);
                        connection.Execute("usp_ins_NICbinding", nicb, null, null, System.Data.CommandType.StoredProcedure);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_NICbinding>", ex);
            }
            return result;
        }

        public static bool InsertNetworkAdapterConfigurations(List<NetworkAdapaterConfiguration> networksadapterconfiguration, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var nasConf = new DynamicParameters();
                    foreach (var naconf in networksadapterconfiguration)
                    {
                        nasConf.Add("@server_id", serverId);
                        nasConf.Add("@DefaultIPGateway", naconf.DefaultIPGateway);
                        nasConf.Add("@Description", naconf.Description);
                        nasConf.Add("@DHCPEnabled", naconf.DHCPEnabled);
                        nasConf.Add("@DHCPServer", naconf.DHCPServer);
                        nasConf.Add("@DNSDomain", naconf.DNSDomain);
                        nasConf.Add("@DNSDomainSuffixSearchOrder", naconf.DNSDomainSuffixSearchOrder);
                        nasConf.Add("@DNSEnabledForWINSResolution", naconf.DNSEnabledForWINSResolution);
                        nasConf.Add("@DNSServerSearchOrder", naconf.DNSServerSearchOrder);
                        nasConf.Add("@DomainDNSRegistrationEnabled", naconf.DomainDNSRegistrationEnabled);
                        nasConf.Add("@FullDNSRegistrationEnabled", naconf.FUllDNSRegistrationEnabled);
                        nasConf.Add("@Index", Utility.CheckIntValue(naconf.Index));
                        nasConf.Add("@IPaddress", naconf.IPAddress);
                        nasConf.Add("@IPConnectionMetric", Utility.CheckIntValue(naconf.IPConnectionMetric));
                        nasConf.Add("@IPSubnet", naconf.IPSubnet);
                        nasConf.Add("@IPEnabled", naconf.IPEnabled);
                        nasConf.Add("@IPXAddress", naconf.IPXAddress);
                        nasConf.Add("@IPXEnabled", naconf.IPXEnabled);
                        nasConf.Add("@MACAddress", naconf.MACAddress);
                        nasConf.Add("@ServiceName", naconf.ServiceName);
                        nasConf.Add("@SettingId", naconf.SettingId);
                        nasConf.Add("@TCPIPNetBIOSOptions", Utility.CheckIntValue(naconf.TCPIPNetBIOSOptions));
                        nasConf.Add("@WINSEnableLMHostsLookup", naconf.WINSEnableLMHostsLookup);
                        nasConf.Add("@WINSPrimaryServer", naconf.WINSPrimaryServer);
                        nasConf.Add("@WINSSecondaryServer", naconf.WINSSecondaryServer);
                        
                        connection.Execute("usp_ins_NetworkAdapterConfiguration", nasConf, null, null, System.Data.CommandType.StoredProcedure);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_NetworkAdapterConfiguration>", ex);
            }
            return result;
        }

        public static bool InsertRouteTable(List<IPV4RouteTable> networksadapterconfiguration, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var routetbl = new DynamicParameters();
                    foreach (var route in networksadapterconfiguration)
                    {
                        routetbl.Add("@server_id", serverId);
                        routetbl.Add("@NetworkAddress", route.NetworkAddress);
                        routetbl.Add("@Netmask", route.Netmask);
                        routetbl.Add("@GatewayAddress", route.GatewayAddress);
                        routetbl.Add("@Metric", route.Metric);

                        connection.Execute("usp_ins_IPV4Route", routetbl, null, null, System.Data.CommandType.StoredProcedure);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_IPV4Route>", ex);
            }
            return result;
        }

        public static bool InsertInstalledFeatures(List<Feature> features, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var feature = new DynamicParameters();
                    foreach (var featr in features)
                    {
                        feature.Add("@server_id", serverId);
                        feature.Add("@DisplayName",featr.Name);
                        connection.Execute("usp_ins_InstalledFeature", feature, null, null, System.Data.CommandType.StoredProcedure);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_InstalledFeature>", ex);
            }
            return result;
        }

        public static bool InsertInstalledUpdates(List<InstalledUpdate> installedupdates, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var instupd = new DynamicParameters();
                    foreach (var instUPD in installedupdates)
                    {
                        instupd.Add("@server_id", serverId);
                        instupd.Add("@HotFixID", instUPD.HotFixID);
                        instupd.Add("@Title", instUPD.Title);
                        instupd.Add("@InstallDate", Utility.CheckDateTimeValue(instUPD.InstalledDate));
                        connection.Execute("usp_ins_InstalledUpdate", instupd, null, null, System.Data.CommandType.StoredProcedure);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_InstalledUpdate>", ex);
            }
            return result;
        }

        public static bool InsertPendingUpdates(List<PendingUpdate> pendingupdates, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var pendupd1 = new DynamicParameters();
                    pendupd1.Add("@server_id", serverId);
                    connection.Execute("usp_del_PendingUpdates", pendupd1, null, null, System.Data.CommandType.StoredProcedure); 
                    
                    var pendupd = new DynamicParameters();
                    foreach (var pendUPD in pendingupdates)
                    {
                      pendupd.Add("@server_id", serverId);
                      pendupd.Add("@HotFixID", pendUPD.HotFixID);
                      pendupd.Add("@Title", pendUPD.Title);
                      pendupd.Add("@ReleaseDate", Utility.CheckDateTimeValue(pendUPD.ReleaseDate));
                      pendupd.Add("@Severity", pendUPD.Severity);
                      connection.Execute("usp_ins_PendingUpdate", pendupd, null, null, System.Data.CommandType.StoredProcedure); 
                    }
                }
                result = true;
             }   
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_PendingUpdate>", ex);
            }
            return result;
        }

        public static bool InsertSEPInfo(SymantecEndpointProtection SEP, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var s = new DynamicParameters();
                    s.Add("@server_id", serverId);
                    s.Add("@isSEPInstalled", SEP.IsInstalled);
                    s.Add("@Install_Date", Utility.CheckDateTimeValue(SEP.InstallDate));
                    s.Add("@Version", SEP.Version);
                    s.Add("@SyncDate", Utility.CheckDateTimeValue(SEP.SyncDate));
                    
                    connection.Execute("usp_ins_AntiVirusInformation", s, null, null, System.Data.CommandType.StoredProcedure);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_AntiVirusInformation>", ex);
            }
            return result;
        }

        public static bool InsertInstalledApplications(List<InstalledAppplication> installedapplications, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var instapp = new DynamicParameters();
                    foreach (var instAPP in installedapplications)
                    {
                        instapp.Add("@server_id", serverId);
                        instapp.Add("@Name", instAPP.Name);
                        instapp.Add("@Version", instAPP.Version);
                        instapp.Add("@ServiceUpdate", Utility.CheckIntValue(instAPP.ServiceUpdate));
                        instapp.Add("@Vendor", instAPP.Vendor);
                        instapp.Add("@InstallDate", Utility.CheckDateTimeValue(instAPP.InstallDate));
                        connection.Execute("usp_ins_InstalledApplication", instapp, null, null, System.Data.CommandType.StoredProcedure);
                    }                       
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_InstalledApplication>", ex);
            }
            return result;
        }

        public static bool InsertLocalAdministratorGroup(List<LocalAdministratorGroup> lagMembers, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var lagmem = new DynamicParameters();
                    foreach (var mem in lagMembers)
                    {
                        lagmem.Add("@server_id", serverId);
                        //lagmem.Add("@Member", String.IsNullOrEmpty(mem.Member) ? null : mem.Member);
                        lagmem.Add("@Member", mem.Member);
                        if (!String.IsNullOrEmpty(mem.Member))
                        {
                            connection.Execute("usp_ins_AdministratorGroupMember", lagmem, null, null, System.Data.CommandType.StoredProcedure);
                        }
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_AdministratorGroupMember>", ex);
            }
            return result;
        }

        public static bool InsertLocalPowerUsersGroup(List<LocalPowerUsersGroup> lpugMembers, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var lpugmem = new DynamicParameters();
                    foreach (var mem in lpugMembers)
                    {
                        lpugmem.Add("@server_id", serverId);
                        lpugmem.Add("@Member", mem.Member);
                        if (!String.IsNullOrEmpty(mem.Member))
                        {
                            connection.Execute("usp_ins_PowerUsersGroupMember", lpugmem, null, null, System.Data.CommandType.StoredProcedure);
                        }
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_PowerUsersGroupMember>", ex);
            }
            return result;
        }

        public static bool InsertLocalRemoteDesktopUsersGroup(List<LocalRemoteDesktopUserGroup> lrduMembers, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var lrdugmem = new DynamicParameters();
                    foreach (var mem in lrduMembers)
                    {
                        lrdugmem.Add("@server_id", serverId);
                        lrdugmem.Add("@Member", mem.Member);
                        if (!String.IsNullOrEmpty(mem.Member))
                        {
                            connection.Execute("usp_ins_RemoteDesktopUsersGroupMember", lrdugmem, null, null, System.Data.CommandType.StoredProcedure);
                        }
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_RemoteDesktopUsersGroupMember>", ex);
            }
            return result;
        }

        public static bool InsertServices(List<Service> services, Int32 serverId)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var service = new DynamicParameters();
                    foreach (var srvc in services)
                    {
                        service.Add("@server_id", serverId);
                        service.Add("@Name", srvc.Name);
                        service.Add("@Status", srvc.Status);
	                    service.Add("@PathName", srvc.PathName);
	                    service.Add("@ServiceType", srvc.ServiceType);
	                    service.Add("@StartMode", srvc.StartMode);
	                    service.Add("@AcceptPause", srvc.AcceptPause);
	                    service.Add("@AcceptStop", srvc.AcceptStop);
	                    service.Add("@Description", srvc.Description);
	                    service.Add("@DisplayName", srvc.DisplayName);
                        service.Add("@ProcessId", srvc.ProcessId);
                        service.Add("@Started", srvc.Started);
	                    service.Add("@StartName", srvc.StartName);
                        service.Add("@State", srvc.State);
                        service.Add("@Path", srvc.Path);

                        connection.Execute("usp_ins_Service", service, null, null, System.Data.CommandType.StoredProcedure);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_Service>", ex);
            }
            return result;
        }

        public static bool InsertActivedirectoryServers(List<ActivedirectoryServer> adservers)
        {
            bool result = false;
            try
            {
                using (var connection = ConnectionManager.Create())
                {
                    var server = new DynamicParameters();
                    foreach (var srv in adservers)
                    {
                        server.Add("@cn", srv.CN);
                        server.Add("@ridsetreferences", srv.RidSetReferences);
                        server.Add("@operatingsystemservicepack", srv.OperatingSystemServicepack);
                        server.Add("@operatingsystem", srv.OperatingSystem);
                        server.Add("@operatingsystemversion", srv.OperatingSystemVersion);
                        server.Add("@lastlogontimestamp", Utility.CheckDateTimeValue(srv.LastLogonTimestamp));
                        server.Add("@whencreated", Utility.CheckDateTimeValue(srv.WhenCreated));
                        server.Add("@adspath", srv.ADSpath);
                        server.Add("@whenchanged", Utility.CheckDateTimeValue(srv.WhenChanged));
                        server.Add("@memberof", srv.Memberof);
                        server.Add("@dnshostname", srv.DNShostname);
                        server.Add("@distinguishedname", srv.DistinguishedName);
                        server.Add("@pwdlastset", Utility.CheckDateTimeValue(srv.PWlastset));
                        server.Add("@publishedat", srv.Publishedat);

                        connection.Execute("usp_ins_ActivedirectoryServer", server, null, null, System.Data.CommandType.StoredProcedure);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Error executing stored proc <usp_ins_ActivedirectoryServer>", ex);
            }
            return result;
        }
    }
}