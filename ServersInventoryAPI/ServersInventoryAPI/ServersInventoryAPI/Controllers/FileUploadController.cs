using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml.Linq;

namespace ServersInventoryAPI
{
    public class FileUploadController : ApiController
    {
        [HttpPost]
        public async Task<FileResult> UploadFile()
        {
            string uploadFolder = WebConfigurationManager.AppSettings["Source"];

            // Verify that this is an HTML Form file upload request
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
            }
            // Create a stream provider for setting up output streams
            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(uploadFolder);

            // Read the MIME multipart asynchronously content using the stream provider we just created.
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            string localfileName = streamProvider.FileData.Select(entry => entry.LocalFileName).FirstOrDefault();
            string filename = HttpContext.Current.Request.QueryString["filename"];
            string uploadType = "inventory";
            uploadType = HttpContext.Current.Request.QueryString["uploadtype"];
            LoggingManager.Instance.GetLogger.InfoFormat("uploadType:====>{1}<==== FileName: ====>{0}<====", filename, uploadType);

            FileResult result = new FileResult();
            if (string.IsNullOrEmpty(filename))
            {
                LoggingManager.Instance.GetLogger.Error("Submitted File Name does not exist..Aborting.");
                result.Success = "Failure";
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(localfileName))
                    {
                        throw new ArgumentNullException();
                    }

                    switch (uploadType)
                    {
                        case "inventory":
                            ProcessInventoryFile(localfileName, filename);
                            break;
                        case "cluster":
                            break;
                        case "activedirectory":
                            ProcessActivedirectoryFile(localfileName, filename);
                            break;
                        default:
                            throw new NotSupportedException("Upload Type is not supported");
                    }
                    
                    result.Success = "Success";
                }
                catch (Exception ex)
                {
                    LoggingManager.Instance.GetLogger.Error("Fatal exception thrown while processing file. Aborting...", ex);
                    result.Success = "Failure";
                }
            }
            return result;
        }

        #region Extract and process Inventory file ----------------------------

        private SystemInformation ExtractSystemInformation(XElement element)
        {
            SystemInformation sysInfo = new SystemInformation();

            if (element != null)
            {
                sysInfo.InventoryDate = Utility.ConvertValue<DateTime>(element.Element("InventoryDate").Value, Utility.DefaultDate);
                sysInfo.EnclosureSerialNumber = element.Element("enclosureSerialNumber").Value;
                sysInfo.EnclosureChassisType = element.Element("enclosureChassisType").Value;
                sysInfo.EnclosurePath = element.Element("enclosurePath").Value;
                sysInfo.EnclosureManufacturer = element.Element("enclosureManufacturer").Value;
                sysInfo.BiosName = element.Element("biosName").Value;
                sysInfo.BiosVersion = element.Element("biosVersion").Value;
                sysInfo.BiosReleaseDate = Utility.ConvertValue<DateTime>(element.Element("biosReleaseDate").Value, Utility.DefaultDate);
                sysInfo.BiosSMBIOSBIOSVersion = element.Element("biosSMBIOSBIOSVersion").Value;
                sysInfo.BiosSMBIOSMajorVersion = element.Element("biosSMBIOSMajorVersion").Value;
                sysInfo.BiosSMBIOSMinorVersion = element.Element("biosSMBIOSMinorVersion").Value;
                sysInfo.PhysicalMemorytotalDIMMs = Utility.ConvertValue<int>(element.Element("PhysicalMemoryTotalDIMMs").Value, Utility.DefaultInt);
                sysInfo.PhysicalMemorytotalDIMMsFree = Utility.ConvertValue<int>(element.Element("PhysicalMemoryTotalDIMMsFree").Value, Utility.DefaultInt);
                sysInfo.PhysicalMemoryTotal = Utility.ConvertValue<int>(element.Element("PhysicalMemoryTotal").Value, Utility.DefaultInt);
                sysInfo.PhysicalMemoryFree = Utility.ConvertValue<int>(element.Element("PhysicalMemoryFree").Value, Utility.DefaultInt);
                sysInfo.PhysicalMemoryPercentUsed = Utility.ConvertValue<float>(element.Element("PhysicalMemoryPercentUsed").Value, Utility.DefaultFloat); // u can also use float.Parce("value to convert");
                sysInfo.VirtualMemoryTotal = Utility.ConvertValue<int>(element.Element("VirtualMemoryTotal").Value, Utility.DefaultInt);
                sysInfo.VirtualMemoryFree = Utility.ConvertValue<int>(element.Element("VirtualMemoryFree").Value, Utility.DefaultInt);
                sysInfo.CPUCorecount = Utility.ConvertValue<int>(element.Element("cpuCoreCount").Value, Utility.DefaultInt);
                sysInfo.CPUCount = Utility.ConvertValue<int>(element.Element("cpuCount").Value, Utility.DefaultInt);
                sysInfo.SystemName = element.Element("systemName").Value;
                sysInfo.SystemDomain = element.Element("systemDomain").Value;
                sysInfo.SystemFQDN = element.Element("systemFQDN").Value;
                sysInfo.SystemIPAddress = element.Element("systemIPAddress").Value;
                sysInfo.SystemDomainRole = element.Element("systemDomainRole").Value;
                sysInfo.SystemUUID = element.Element("systemUUID").Value;
                sysInfo.SystemType = element.Element("systemType").Value;
                sysInfo.SystemDescription = element.Element("systemDescription").Value;
                sysInfo.SystemManufacturer = element.Element("systemManufacturer").Value;
                sysInfo.SystemModel = element.Element("systemModel").Value;
                sysInfo.systemProductID = element.Element("systemProductID").Value;
                sysInfo.SystemTimeZone = element.Element("systemTimeZone").Value;
                sysInfo.SystemPendingReboot = Utility.ConvertValue<bool>(element.Element("systemPendingReboot").Value, Utility.DefaultBoolean);
                sysInfo.systemRDPconfiguration = element.Element("systemRDPconfiguration").Value;
                sysInfo.systemCurrentCulture = element.Element("systemCurrentCulture").Value;
                sysInfo.systemFirewallEnabled = element.Element("systemFirewallEnabled").Value;
                sysInfo.systemMonitoringAgent = element.Element("systemMonitoringAgent").Value;
                sysInfo.systemBackupAgent = element.Element("systemBackupAgent").Value;
                sysInfo.SystemUptime = element.Element("systemUptime").Value;
                sysInfo.IsVirtual = Utility.ConvertValue<bool>(element.Element("IsVirtual").Value, Utility.DefaultBoolean);
                sysInfo.VM_Type = element.Element("vm_Type").Value;
                sysInfo.VM_PhysicalHostName = element.Element("vm_PhysicalHostName").Value;
                sysInfo.Cluster = element.Element("Cluster").Value;
                sysInfo.IPConfig = element.Element("IPconfig").Value;
                sysInfo.Routes = element.Element("Routes").Value;
            }
            return sysInfo;
        }

        private List<PageFile> ExtractPageFiles(XElement element)
        {
            List<PageFile> pfs = new List<PageFile>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string name = app.Element("Name").Value; ;
                    int filefize = Utility.ConvertValue<int>(app.Element("FileSize").Value, Utility.DefaultInt);
                    string filetype = app.Element("FileType").Value;
                    bool compressed = Utility.ConvertValue<bool>(app.Element("Compressed").Value, Utility.DefaultBoolean);
                    string compressionmethod = app.Element("CompressionMethod").Value;
                    bool encrypted = Utility.ConvertValue<bool>(app.Element("Encrypted").Value, Utility.DefaultBoolean);
                    string encryptionmethod = app.Element("EncryptionMethod").Value;
                    bool hidden = Utility.ConvertValue<bool>(app.Element("Hidden").Value, Utility.DefaultBoolean);
                    DateTime installdate = Utility.ConvertValue<DateTime>(app.Element("InstallDate").Value, Utility.DefaultDate);
                    DateTime lastaccessed = Utility.ConvertValue<DateTime>(app.Element("LastAccessed").Value, Utility.DefaultDate);
                    DateTime lastmodified = Utility.ConvertValue<DateTime>(app.Element("LastModified").Value, Utility.DefaultDate);

                    pfs.Add(new PageFile()
                    {
                        Name = name,
                        FileSize = filefize,
                        FileType = filetype,
                        Compressed = compressed,
                        CompressionMethod = compressionmethod,
                        Encrypted = encrypted,
                        EncryptionMethod = encryptionmethod,
                        Hidden = hidden,
                        InstallDate = installdate,
                        LastAccessed = lastaccessed,
                        LastModified = lastmodified
                    });
                }
            }
            return pfs;
        }

        private List<Feature> ExtractFeatures(XElement element)
        {
            List<Feature> feature = new List<Feature>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string name = app.Value;

                    feature.Add(new Feature()
                    {
                        Name = name
                    });
                }
            }
            return feature;
        }

        private OperatingSystem ExtractOperatingSystem(XElement element)
        {
            OperatingSystem os = new OperatingSystem();
            if (element != null)
            {
                os.Name = element.Element("Name").Value;
                os.SID = element.Element("SID").Value;
                os.Manufacturer = element.Element("Manufacturer").Value;
                os.Caption = element.Element("Caption").Value;
                os.Version = element.Element("Version").Value;
                os.CSDVersion = element.Element("CSDVersion").Value;
                os.InstallDate = Utility.ConvertValue<DateTime>(element.Element("InstallDate").Value, Utility.DefaultDate);
                os.LastBOotUpTime = Utility.ConvertValue<DateTime>(element.Element("LastBootUpTime").Value, Utility.DefaultDate);
                os.SerialNumber = element.Element("SerialNumber").Value;
                os.ServicePackMajorVersion = Utility.ConvertValue<Int32>(element.Element("ServicePackMajorVersion").Value, Utility.DefaultInt);
                os.ProductType = element.Element("ProductType").Value;
                os.OSProductSuite = element.Element("OSProductSuite").Value;
                os.OtherTypeDescription = element.Element("OtherTypeDescription").Value;
                os.Description = element.Element("Description").Value;
                os.OperatingSystemSKU = element.Element("OperatingSystemSKU").Value;
                os.OSArchitecture = element.Element("OSArchitecture").Value;
                os.BuildNumber = Utility.ConvertValue<Int32>(element.Element("BuildNumber").Value, Utility.DefaultInt);
                os.SystemDrive = element.Element("SystemDrive").Value;
                os.SystemDirectory = element.Element("SystemDirectory").Value;
                os.WindowsDirectory = element.Element("WindowsDirectory").Value;
                os.Organization = element.Element("Organization").Value;
                os.LocalDateTime = Utility.ConvertValue<DateTime>(element.Element("LocalDateTime").Value, Utility.DefaultDate);
                os.OSType = element.Element("OSType").Value;
                os.ActivationStatus = element.Element("ActivationStatus").Value;
                os.osRecoveryAutoReboot = Utility.ConvertValue<bool>(element.Element("osRecoveryAutoReboot").Value, Utility.DefaultBoolean);
                os.osRecoveryDebugInfoType = element.Element("osRecoveryDebugInfoType").Value;
                os.osRecoveryOverwriteExistingDebugFile = Utility.ConvertValue<bool>(element.Element("osRecoveryOverwriteExistingDebugFile").Value, Utility.DefaultBoolean);
                os.osRecoveryExpandedDebugFilePath = element.Element("osRecoveryExpandedDebugFilePath").Value;
                os.osRecoveryExpandedMiniDumpDirectory = element.Element("osRecoveryExpandedMiniDumpDirectory").Value;
            }
            return os;
        }

        private WUsettings ExtractWUsettings(XElement element)
        {
            WUsettings WUs = new WUsettings();
            if (element != null)
            {
                WUs.UseWSUSserver = Utility.ConvertValue<bool>(element.Element("UseWSUSserver").Value, Utility.DefaultBoolean);
                WUs.DownloadLastSuccessTime = Utility.ConvertValue<DateTime>(element.Element("DownloadLastSuccessTime").Value, Utility.DefaultDate);
                WUs.InstallLastSuccessTime = Utility.ConvertValue<DateTime>(element.Element("InstallLastSuccessTime").Value, Utility.DefaultDate);
                WUs.wuDetectionFrequency = Utility.ConvertValue<Int32>(element.Element("DetectionFrequency").Value, Utility.DefaultInt);
                WUs.wuAutomaticUpdatesNotification = element.Element("AutomaticUpdatesNotification").Value;
                WUs.wuDetectLastSuccessTime = Utility.ConvertValue<DateTime>(element.Element("DetectLastSuccessTime").Value, Utility.DefaultDate);
                WUs.wuAutomaticUpdateEnabled = Utility.ConvertValue<bool>(element.Element("AutomaticUpdateEnabled").Value, Utility.DefaultBoolean);
                WUs.wuInstallFrequency = element.Element("InstallFrequency").Value;
                WUs.wuInstallTime = element.Element("InstallTime").Value;
                WUs.wuWSUSserver = element.Element("WSUSserver").Value;
                WUs.wuWSUSstatusURL = element.Element("WSUSstatusURL").Value;
                WUs.wuTargetGroupEnabled = Utility.ConvertValue<bool>(element.Element("TargetGroupEnabled").Value, Utility.DefaultBoolean);
                WUs.wuTargetGroup = element.Element("TargetGroup").Value;
                WUs.wuOptedinMicrosoftUpdate = Utility.ConvertValue<bool>(element.Element("OptedinMicrosoftUpdate").Value, Utility.DefaultBoolean);
            }
            return WUs;
        }

        private List<Processor> ExtractProcessors(XElement element)
        {
            List<Processor> proc = new List<Processor>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string deviceid = app.Element("DeviceID").Value;
                    string name = app.Element("Name").Value;
                    string desc = app.Element("Description").Value;
                    string manuf = app.Element("Manufacturer").Value;
                    string family = app.Element("Family").Value;
                    string procid = app.Element("ProcessorId").Value;
                    string status = app.Element("Status").Value;
                    int addw = Utility.ConvertValue<Int32>(app.Element("AddressWidth").Value, Utility.DefaultInt);
                    int dataw = Utility.ConvertValue<Int32>(app.Element("DataWidth").Value, Utility.DefaultInt);
                    int extclock = Utility.ConvertValue<Int32>(app.Element("ExternalClock").Value, Utility.DefaultInt);
                    int l2cach = Utility.ConvertValue<Int32>(app.Element("L2CacheSize").Value, Utility.DefaultInt);
                    int maxclsp = Utility.ConvertValue<Int32>(app.Element("MaxClockSpeed").Value, Utility.DefaultInt);
                    int rev = Utility.ConvertValue<Int32>(app.Element("Revision").Value, Utility.DefaultInt);
                    string sockdesig = app.Element("SocketDesignation").Value;
                    string arch = app.Element("Architecture").Value;
                    int curclsp = Utility.ConvertValue<Int32>(app.Element("CurrentClockSpeed").Value, Utility.DefaultInt);
                    int numcores = Utility.ConvertValue<Int32>(app.Element("NumberOfCores").Value, Utility.DefaultInt);

                    proc.Add(new Processor()
                    {
                        DeviceID = deviceid,
                        Name = name,
                        Description = desc,
                        Manufacturer = manuf,
                        Family = family,
                        ProcessorId = procid,
                        Status = status,
                        AddressWidth = addw,
                        DataWidth = dataw,
                        ExternalClock = extclock,
                        L2CacheSize = l2cach,
                        MaxClockSpeed = maxclsp,
                        Revision = rev,
                        SocketDesignation = sockdesig,
                        Architecture = arch,
                        CurrentClockSpeed = curclsp,
                        NumberOfCores = numcores
                    });
                }
            }
            return proc;
        }

        private List<Disk> ExtractDisks(XElement element)
        {
            List<Disk> disk = new List<Disk>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string drive = app.Element("Drive").Value;
                    string phydisk = app.Element("Disk").Value;
                    string model = app.Element("Model").Value;
                    string part = app.Element("Partition").Value;
                    string desc = app.Element("Description").Value;
                    bool pripart = Utility.ConvertValue<bool>(app.Element("PrimaryPartition").Value, Utility.DefaultBoolean);
                    string volname = app.Element("VolumeName").Value;
                    int disksize = Utility.ConvertValue<Int32>(app.Element("DiskSize").Value, Utility.DefaultInt);
                    int freespace = Utility.ConvertValue<Int32>(app.Element("FreeSpace").Value, Utility.DefaultInt);
                    float percfree = Utility.ConvertValue<float>(app.Element("PercentageFree").Value, Utility.DefaultFloat);
                    string diskType = app.Element("DiskType").Value;
                    string sn = app.Element("SerialNumber").Value;

                    disk.Add(new Disk()
                    {
                        Drive = drive,
                        PhysicalDisk = phydisk,
                        Model = model,
                        Partition = part,
                        Description = desc,
                        PrimaryPartition = pripart,
                        VolumeName = volname,
                        DiskSize = disksize,
                        FreeSpace = freespace,
                        PercentageFree = percfree,
                        DiskType = diskType,
                        SerialNumber = sn
                    });
                }
            }
            return disk;
        }

        private List<MPIO> ExtractMPIOpaths(XElement element)
        {
            List<MPIO> mpio = new List<MPIO>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string name = app.Element("Name").Value;
                    int numberpaths = Utility.ConvertValue<Int32>(app.Element("Numberpaths").Value, Utility.DefaultInt);
                    
                    mpio.Add(new MPIO()
                    {
                        Name = name,
                        Numberpaths = numberpaths
                    });
                }
            }
            return mpio;
        }

        private List<Share> ExtractShares(XElement element)
        {
            List<Share> share = new List<Share>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string name = app.Element("Name").Value;
                    string desc = app.Element("Description").Value;
                    int maxallowed = Utility.ConvertValue<Int32>(app.Element("MaximumAllowed").Value, Utility.DefaultInt);
                    string path = app.Element("Path").Value;
                    Boolean allowmax = Utility.ConvertValue<bool>(app.Element("AllowMaximum").Value, Utility.DefaultBoolean);
                    string shtype = app.Element("ShareType").Value;

                    share.Add(new Share()
                    {
                        Name = name,
                        Description = desc,
                        MaximumAllowed = maxallowed,
                        Path = path,
                        AllowMaximum = allowmax,
                        ShareType = shtype
                    });
                }
            }
            return share;
        }

        private List<SCSIController> ExtractSCSIControllers(XElement element)
        {
            List<SCSIController> scsiCont = new List<SCSIController>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string name = app.Element("Name").Value;
                    string devID = app.Element("DeviceID").Value;
                    string manufacturer = app.Element("Manufacturer").Value;
                    string devName = app.Element("DriverName").Value;

                    scsiCont.Add(new SCSIController()
                    {
                        Name = name,
                        DeviceID = devID,
                        Manufacturer = manufacturer,
                        DriverName = devName
                    });
                }
            }
            return scsiCont;
        }

        private VideoController ExtractVideoController(XElement element)
        {
            VideoController vid = new VideoController();
            if (element != null)
            {
                vid.Name = element.Element("Name").Value;
                vid.DeviceID = element.Element("DeviceID").Value;
                vid.AdapterCompatibility = element.Element("AdapterCompatibility").Value;
                vid.InstalledDisplayDrivers = element.Element("InstalledDisplayDrivers").Value;
                vid.DriverVersion = element.Element("DriverVersion").Value;
                vid.DriverDate = Utility.ConvertValue<DateTime>(element.Element("DriverDate").Value, Utility.DefaultDate);
                vid.InfFilename = element.Element("InfFilename").Value;
                vid.PNPDeviceID = element.Element("PNPDeviceID").Value;
            }
            return vid;
        }

        private List<NetworkAdapater> ExtractNetworkAdapters(XElement element)
        {
            List<NetworkAdapater> netaddapter = new List<NetworkAdapater>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string name = app.Element("Name").Value;
                    int devid = Utility.ConvertValue<Int32>(app.Element("DeviceID").Value, Utility.DefaultInt);
                    string netconnid = app.Element("NetConnectionID").Value;
                    string pnpdevid = app.Element("PNPDeviceId").Value;
                    string adpType = app.Element("AdapterType").Value;
                    string macaddr = app.Element("MACAddress").Value;
                    string manufacturer = app.Element("Manufacturer").Value;
                    bool promMode = Utility.ConvertValue<bool>(app.Element("DeviceID").Value, Utility.DefaultBoolean);
                    string connStatus = app.Element("ConnectionStatus").Value;

                    netaddapter.Add(new NetworkAdapater()
                    {
                        Name = name,
                        DeviceID = devid,
                        NetConnectionID = netconnid,
                        PNPDeviceId = pnpdevid,
                        AdapterType = adpType,
                        MACAddress = macaddr,
                        Manufacturer = manufacturer,
                        PromiscuousMode = promMode,
                        ConnectionStatus = connStatus

                    });
                }
            }
            return netaddapter;
        }

        private List<NICbinding> ExtractNicBindingOrder(XElement element)
        {
            List<NICbinding> nicbinding = new List<NICbinding>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    int bindingorder = Utility.ConvertValue<Int32>(app.Element("BindingOrder").Value, Utility.DefaultInt);
                    string name = app.Element("Name").Value;
                    bool nicenabled = Utility.ConvertValue<bool>(app.Element("NICenabled").Value, Utility.DefaultBoolean);
                    string guid = app.Element("GUID").Value;

                    nicbinding.Add(new NICbinding()
                    {
                        Name = name,
                        BindingOrder = bindingorder,
                        NICenabled = nicenabled,
                        GUID = guid
                    });
                }
            }
            return nicbinding;
        }

        private List<IPV4RouteTable> ExtractRouteTable(XElement element)
        {
            List<IPV4RouteTable> routetable = new List<IPV4RouteTable>();
            if (element != null)
            {
                var routes = element.Elements().ToList();
                foreach (var rt in routes)
                {
                    string networkaddress = rt.Element("NetworkAddress").Value;
                    string netmask = rt.Element("Netmask").Value;
                    string gatewayaddress = rt.Element("GatewayAddress").Value;
                    string metric = rt.Element("Metric").Value;

                    routetable.Add(new IPV4RouteTable()
                    {
                        NetworkAddress = networkaddress,
                        Netmask = netmask,
                        GatewayAddress = gatewayaddress,
                        Metric = metric
                        
                    });
                }
            }
            return routetable;
        }

        private List<NetworkAdapaterConfiguration> ExtractNetworkAdapterConfiguration(XElement element)
        {
            List<NetworkAdapaterConfiguration> netaddapter = new List<NetworkAdapaterConfiguration>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string defaultIPgw = app.Element("DefaultIPGateway").Value;
                    string desc = app.Element("Description").Value;
                    bool dhcpEna = Utility.ConvertValue<bool>(app.Element("DHCPEnabled").Value, Utility.DefaultBoolean);
                    string dhcpser = app.Element("DHCPServer").Value;
                    string dnsdom = app.Element("DNSDomain").Value;
                    string dnsdomsuffixSO = app.Element("DNSDomainSuffixSearchOrder").Value;
                    bool dnsenawinResolution = Utility.ConvertValue<bool>(app.Element("DNSEnabledForWINSResolution").Value, Utility.DefaultBoolean);
                    string dnsserSO = app.Element("DNSServerSearchOrder").Value;
                    bool domDNSregEna = Utility.ConvertValue<bool>(app.Element("DomainDNSRegistrationEnabled").Value, Utility.DefaultBoolean);
                    bool fUllDNSregEna = Utility.ConvertValue<bool>(app.Element("FullDNSRegistrationEnabled").Value, Utility.DefaultBoolean);
                    int index = Utility.ConvertValue<Int32>(app.Element("Index").Value, Utility.DefaultInt);
                    string IPAdd = app.Element("IPAddress").Value;
                    int IPconnMetric = Utility.ConvertValue<Int32>(app.Element("IPConnectionMetric").Value, Utility.DefaultInt);
                    string IPSub = app.Element("IPSubnet").Value;
                    bool IPEna = Utility.ConvertValue<bool>(app.Element("IPEnabled").Value, Utility.DefaultBoolean);
                    string IPXAdd = app.Element("IPXAddress").Value;
                    bool IPXEna = Utility.ConvertValue<bool>(app.Element("IPXEnabled").Value, Utility.DefaultBoolean);
                    string MACAdd = app.Element("MACAddress").Value;
                    string serviceName = app.Element("ServiceName").Value;
                    string SetId = app.Element("SettingId").Value;
                    int tcpipNetBIOSOpt = Utility.ConvertValue<Int32>(app.Element("TCPIPNetBIOSOptions").Value, Utility.DefaultInt);
                    bool winsEnaLMHostsLookup = Utility.ConvertValue<bool>(app.Element("WINSEnableLMHostsLookup").Value, Utility.DefaultBoolean);
                    string winsPriSer = app.Element("WINSPrimaryServer").Value;
                    string winsSecSer = app.Element("WINSSecondaryServer").Value;

                    netaddapter.Add(new NetworkAdapaterConfiguration()
                    {
                        DefaultIPGateway = defaultIPgw,
                        Description = desc,
                        DHCPEnabled = dhcpEna,
                        DHCPServer = dhcpser,
                        DNSDomain = dnsdom,
                        DNSDomainSuffixSearchOrder = dnsdomsuffixSO,
                        DNSEnabledForWINSResolution = dnsenawinResolution,
                        DNSServerSearchOrder = dnsserSO,
                        DomainDNSRegistrationEnabled = domDNSregEna,
                        FUllDNSRegistrationEnabled = fUllDNSregEna,
                        Index = index,
                        IPAddress = IPAdd,
                        IPConnectionMetric = IPconnMetric,
                        IPSubnet = IPSub,
                        IPEnabled = IPEna,
                        IPXAddress = IPXAdd,
                        IPXEnabled = IPXEna,
                        MACAddress = MACAdd,
                        ServiceName = serviceName,
                        SettingId = SetId,
                        TCPIPNetBIOSOptions = tcpipNetBIOSOpt,
                        WINSEnableLMHostsLookup = winsEnaLMHostsLookup,
                        WINSPrimaryServer = winsPriSer,
                        WINSSecondaryServer = winsSecSer
                    });
                }
            }
            return netaddapter;
        }

        private List<InstalledAppplication> ExtractInstalledApps(XElement element)
        {
            List<InstalledAppplication> installedApplications = new List<InstalledAppplication>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string name = app.Element("Name").Value;
                    string ver = app.Element("Version").Value;
                    int su = Utility.ConvertValue<int>(app.Element("ServiceUpdate").Value, Utility.DefaultInt);
                    string vendor = app.Element("Vendor").Value;
                    DateTime installDate = Utility.ConvertValue<DateTime>(app.Element("InstallDate").Value, Utility.DefaultDate);
                    installedApplications.Add(new InstalledAppplication()
                    {
                        InstallDate = installDate,
                        Name = name,
                        Vendor = vendor,
                        Version = ver,
                        ServiceUpdate = su
                    });

                }
            }
            return installedApplications;
        }

        private List<InstalledUpdate> ExtractIntalledUpdates(XElement element)
        {
            List<InstalledUpdate> installedUpdates = new List<InstalledUpdate>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string hotfixId = app.Element("HotFixID").Value;
                    string title = app.Element("Title").Value;
                    DateTime installDate = Utility.ConvertValue<DateTime>(app.Element("InstallDate").Value, Utility.DefaultDate);
                    installedUpdates.Add(new InstalledUpdate()
                    {
                        InstalledDate = installDate,
                        HotFixID = hotfixId,
                        Title = title
                    });

                }
            }
            return installedUpdates;
        }

        private List<PendingUpdate> ExtractPendingUpdates(XElement element)
        {
            List<PendingUpdate> pendingUpdates = new List<PendingUpdate>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string hotfixId = app.Element("HotFixID").Value;
                    string title = app.Element("Title").Value;
                    string severity = app.Element("Severity").Value;
                    DateTime relDate = Utility.ConvertValue<DateTime>(app.Element("ReleaseDate").Value, Utility.DefaultDate);
                    pendingUpdates.Add(new PendingUpdate()
                    {
                        ReleaseDate = relDate,
                        HotFixID = hotfixId,
                        Title = title,
                        Severity = severity
                    });

                }
            }
            return pendingUpdates;
        }

        private SymantecEndpointProtection ExtractSymantecEP(XElement element)
        {
            SymantecEndpointProtection sep = new SymantecEndpointProtection();
            if (element != null)
            {
                sep.IsInstalled = Utility.ConvertValue<Boolean>(element.Element("isInstalled").Value, Utility.DefaultBoolean);
                sep.Version = element.Element("Version").Value;
                sep.InstallDate = Utility.ConvertValue<DateTime>(element.Element("InstallDate").Value, Utility.DefaultDate);
                sep.SyncDate = Utility.ConvertValue<DateTime>(element.Element("LatestDefinition").Value, Utility.DefaultDate);
            }
            return sep;
        }

        private List<LocalAdministratorGroup> ExtractLocalAdministratorGroup(XElement element)
        {
            List<LocalAdministratorGroup> LocAdminGroup = new List<LocalAdministratorGroup>();

            if (element != null)
            {
                var members = element.Elements().ToList();
                foreach (var mem in members)
                {
                    string member = mem.Value;
                    LocAdminGroup.Add(new LocalAdministratorGroup()
                    {
                        Member = member
                    });
                }
            }
            return LocAdminGroup;
        }

        private List<LocalPowerUsersGroup> ExtractLocalPowerUsersGroup(XElement element)
        {
            List<LocalPowerUsersGroup> LocPUGroup = new List<LocalPowerUsersGroup>();
            if (element != null)
            {
                var members = element.Elements().ToList();
                foreach (var mem in members)
                {
                    string member = mem.Value;

                    LocPUGroup.Add(new LocalPowerUsersGroup()
                    {
                        Member = member
                    });
                }
            }
            return LocPUGroup;
        }

        private List<LocalRemoteDesktopUserGroup> ExtractLocalRemoteDesktopUsersGroup(XElement element)
        {
            List<LocalRemoteDesktopUserGroup> LocRDUGroup = new List<LocalRemoteDesktopUserGroup>();
            if (element != null)
            {
                var members = element.Elements().ToList();
                foreach (var mem in members)
                {
                    string member = mem.Value;

                    LocRDUGroup.Add(new LocalRemoteDesktopUserGroup()
                    {
                        Member = member
                    });
                }
            }
            return LocRDUGroup;
        }

        private List<Service> ExtractServices(XElement element)
        {
            List<Service> service = new List<Service>();
            if (element != null)
            {
                var apps = element.Elements().ToList();
                foreach (var app in apps)
                {
                    string name = app.Element("Name").Value;
                    string status = app.Element("Status").Value;
                    string pathname = app.Element("PathName").Value;
                    string servicetype = app.Element("ServiceType").Value;
                    string startmode = app.Element("StartMode").Value;
                    bool acceptpause = Utility.ConvertValue<bool>(app.Element("AcceptPause").Value, Utility.DefaultBoolean);
                    bool acceptstop = Utility.ConvertValue<bool>(app.Element("AcceptStop").Value, Utility.DefaultBoolean);
                    string description = app.Element("Description").Value;
                    string displayname = app.Element("DisplayName").Value;
                    int processid = Utility.ConvertValue<int>(app.Element("ProcessId").Value, Utility.DefaultInt);
                    bool started = Utility.ConvertValue<bool>(app.Element("StartName").Value, Utility.DefaultBoolean);
                    string startname = app.Element("StartName").Value;
                    string state = app.Element("State").Value;
                    string path = app.Element("Path").Value;

                    service.Add(new Service()
                    {
                        Name = name,
                        Status = status,
                        PathName = pathname,
                        ServiceType = servicetype,
                        StartMode = startmode,
                        AcceptPause = acceptpause,
                        AcceptStop = acceptstop,
                        Description = description,
                        DisplayName = displayname,
                        ProcessId = processid,
                        Started = started,
                        StartName = startname,
                        State = state,
                        Path = path
                    });
                }
            }
            return service;
        }

        private void ProcessInventoryFile(string filePath, string newFileName)
        { //processing
            SystemInformation sysInfo = null;
            OperatingSystem osInfo = null;
            List<Disk> disks = null;
            List<Processor> procs = null;
            List<SCSIController> SCSIControllers = null;
            List<Share> shares = null;
            VideoController videocontroller = null;
            List<NetworkAdapater> networkadapters = null;
            List<NetworkAdapaterConfiguration> networkadapterconfiguration = null;
            List<Feature> features = null;
            List<InstalledUpdate> installedupdates = null;
            List<PendingUpdate> pendingupdates = null;
            SymantecEndpointProtection SEP = null;
            List<InstalledAppplication> installedapplications = null;
            List<LocalAdministratorGroup> lag = null;
            List<LocalPowerUsersGroup> lpug = null;
            List<LocalRemoteDesktopUserGroup> lrdug = null;
            List<Service> services = null;
            WUsettings wusettings = null;
            List<PageFile> pagefiles = null;
            List<NICbinding> nicbindingorders = null;
            List<MPIO> mpiopaths = null;
            List<IPV4RouteTable> routetable = null;
            
            try
            { ////pro try
                if (File.Exists(filePath))
                { //if filepath exists
                    //System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
                    #region Loading Objects
                    Int32 serverId; 
                    bool result = false;
                    XDocument xdoc = XDocument.Load(filePath);
                    IEnumerable<XElement> configs = xdoc.Elements();
                    if (configs.Count() > 0)
                    { // if 2
                        var list = configs.ToList();
                        var configElms = list[0].Elements();

                        foreach (var item in configElms)
                        {
                            switch (item.Name.LocalName)
                            {
                                case "SystemInformation":
                                    sysInfo = ExtractSystemInformation(item);
                                    break;
                                case "PageFiles":
                                    pagefiles = ExtractPageFiles(item);
                                    break;
                                case "Features":
                                    features = ExtractFeatures(item);
                                    break;
                                case "OperatingSystem":
                                    osInfo = ExtractOperatingSystem(item);
                                    break;
                                case "Processors":
                                    procs = ExtractProcessors(item);
                                    break;
                                case "Disks":
                                    disks = ExtractDisks(item);
                                    break;
                                case "MPIO":
                                    mpiopaths = ExtractMPIOpaths(item);
                                    break;
                                case "Shares":
                                    shares = ExtractShares(item);
                                    break;
                                case "SCSIControllers":
                                    SCSIControllers = ExtractSCSIControllers(item);
                                    break;
                                case "VideoController":
                                    videocontroller = ExtractVideoController(item);
                                    break;
                                case "NetworkAdapters":
                                    networkadapters = ExtractNetworkAdapters(item);
                                    break;
                                case "NetworkAdapterConfiguration":
                                    networkadapterconfiguration = ExtractNetworkAdapterConfiguration(item);
                                    break;
                                case "IPv4RouteTable":
                                    routetable = ExtractRouteTable(item);
                                    break;
                                case "NICBindingOrder":
                                    nicbindingorders = ExtractNicBindingOrder(item);
                                    break;
                                case "LocalAdministratorGroup":
                                    lag = ExtractLocalAdministratorGroup(item);
                                    break;
                                case "LocalPowerUsersGroup":
                                    lpug = ExtractLocalPowerUsersGroup(item);
                                    break;
                                case "LocalRemoteDesktopUsersGroup":
                                    lrdug = ExtractLocalRemoteDesktopUsersGroup(item);
                                    break;
                                case "InstalledUpdates":
                                    installedupdates = ExtractIntalledUpdates(item);
                                    break;
                                case "PendingUpdates":
                                    pendingupdates = ExtractPendingUpdates(item);
                                    break;
                                case "SymantecEndpointProtection":
                                    SEP = ExtractSymantecEP(item);
                                    break;
                                case "InstalledApplications":
                                    installedapplications = ExtractInstalledApps(item);
                                    break;
                                case "Services":
                                    services = ExtractServices(item);
                                    break;
                                case "WUsettings":
                                    wusettings = ExtractWUsettings(item);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #region Processing Objects -----------------------

                        serverId = Utility.DiscoverServer(sysInfo, osInfo);
                        if (serverId > 2)
                        { 
                            if (sysInfo != null && osInfo != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert SystemInfo");
                                result = Utility.InsertSystemInfo(sysInfo, osInfo, wusettings, serverId);
                                LoggingManager.Instance.GetLogger.Info("End SystemInfo Insert");
                            }

                            if (pagefiles != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert pagefiles");
                                result = Utility.InsertPageFiles(pagefiles, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert pagefiles End");
                            }
                            if (features != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Installed Features");
                                result = Utility.InsertInstalledFeatures(features, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Installed Features End");
                            }

                            if (disks != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Disks");
                                result = Utility.InsertDisks(disks, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Disks End");
                            }

                            if (mpiopaths != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert MPIO paths");
                                result = Utility.InsertMPIOpaths(mpiopaths, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert MPIO paths End");
                            }

                            if (procs != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Procs");
                                result = Utility.InsertProcessors(procs, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Procs End");
                            }

                            if (SCSIControllers != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert SCSIController");
                                result = Utility.InsertSCSIControllers(SCSIControllers, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert SCSIController End");
                            }

                            if (shares != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Share");
                                result = Utility.InsertShares(shares, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Share End");
                            }

                            if (videocontroller != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Video Controller");
                                result = Utility.InsertVideoController(videocontroller, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Video Controller End");
                            }

                            if (networkadapters != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Network Adapter");
                                result = Utility.InsertNetworkAdapters(networkadapters, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Network Adapter End");
                            }

                            if (nicbindingorders != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Nic Binding");
                                result = Utility.InsertNICBinding(nicbindingorders, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert NNic Binding");
                            }
                            
                            if (networkadapterconfiguration != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Network Adapter Configuration");
                                result = Utility.InsertNetworkAdapterConfigurations(networkadapterconfiguration, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Network Adapter Configuration End");
                            }

                            if (routetable != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Route Table");
                                result = Utility.InsertRouteTable(routetable, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Route Table End");
                            }

                            if (lag != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert LocalAdministratorGroup");
                                result = Utility.InsertLocalAdministratorGroup(lag, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert LocalAdministratorGroup End");
                            }

                            if (lpug != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert LocalPowerUsersGroup");
                                result = Utility.InsertLocalPowerUsersGroup(lpug, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert LocalPowerUsersGroup End");
                            }

                            if (lrdug != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert LocalRemoteDesktopUsersGroup");
                                result = Utility.InsertLocalRemoteDesktopUsersGroup(lrdug, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert LocalRemoteDesktopUsersGroup End");
                            }

                            if (installedupdates != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Installed Updates");
                                result = Utility.InsertInstalledUpdates(installedupdates, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Installed Updates End");
                            }

                            if (pendingupdates != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert pending Updates");
                                result = Utility.InsertPendingUpdates(pendingupdates, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Pending Updates  End");
                            }

                            if (SEP != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert SEP");
                                result = Utility.InsertSEPInfo(SEP, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert SEP End");
                            }

                            if (networkadapterconfiguration != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Installed Application");
                                result = Utility.InsertInstalledApplications(installedapplications, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Installed Application End");
                            }

                            if (services != null)
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Service");
                                result = Utility.InsertServices(services, serverId) && result;
                                LoggingManager.Instance.GetLogger.Info("Insert Services End");
                            }

                        } //if ID > 2

                        else
                        {
                            string destinationPathFailed = WebConfigurationManager.AppSettings["DestinationFailed"];
                            File.Copy(filePath, Path.Combine(destinationPathFailed, newFileName), true);
                            LoggingManager.Instance.GetLogger.Error(String.Format("Duplicate record in DB. error code:{0}", serverId));
                            File.Delete(filePath);
                            Environment.Exit(0);
                        }
                        #endregion end processing Objects

                    } // if 2
                    #endregion

                    if (result)
                    {
                        try
                        {
                            //File.Copy(filePath, Path.Combine(destinationPath, name), true);
                            string destinationPathProcessed = WebConfigurationManager.AppSettings["DestinationProcessed"];
                            File.Copy(filePath, Path.Combine(destinationPathProcessed, newFileName), true);
                            LoggingManager.Instance.GetLogger.Info(string.Format("Deleting file {0}", filePath));
                            File.Delete(filePath);
                        }
                        catch (Exception ex)
                        {
                            LoggingManager.Instance.GetLogger.Error("File copy was not successful.", ex);
                        }
                    }
                    else
                    {
                        string destinationPathFailed = WebConfigurationManager.AppSettings["DestinationFailed"];
                        File.Copy(filePath, Path.Combine(destinationPathFailed, newFileName), true);
                        LoggingManager.Instance.GetLogger.Error(String.Format("Partial data was transfered to DB."));
                        File.Delete(filePath);
                    }

                } // if filepath exists
                else
                {
                    LoggingManager.Instance.GetLogger.Error(String.Format("File <{0}> could not be found. Aborting...", filePath));
                }

            } //end proc try
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Fatal exception thrown. Aborting...", ex);
            }
        } 

        #endregion --------------------------------------------------------------
        
        #region Extract and process activedirectory file ------------------------

        private List<ActivedirectoryServer> ExtractActivedirectoryServers(XElement element)
        {
            List<ActivedirectoryServer> ADservers = new List<ActivedirectoryServer>();
            if (element != null)
            {
                var ADsrvs = element.Elements().ToList();
                foreach (var server in ADsrvs)
                {
                    string cn = server.Element("cn").Value;
                    string ridsetreferences = server.Element("ridsetreferences").Value;
                    string operatingsystem = server.Element("operatingsystem").Value;
                    string operatingsystemservicepack = server.Element("operatingsystemservicepack").Value;
                    string operatingsystemversion = server.Element("operatingsystemversion").Value;
                    DateTime lastlogontimestamp = Utility.ConvertValue<DateTime>(server.Element("lastlogontimestamp").Value, Utility.DefaultDate);
                    DateTime whencreated = Utility.ConvertValue<DateTime>(server.Element("whencreated").Value, Utility.DefaultDate);
                    string adspath = server.Element("adspath").Value;
                    string memberof = server.Element("memberof").Value;
                    string dnshostname = server.Element("dnshostname").Value;
                    string distinguishedname = server.Element("distinguishedname").Value;
                    string publishedat = server.Element("publishedAt").Value;
                    DateTime whenchanged = Utility.ConvertValue<DateTime>(server.Element("whenchanged").Value, Utility.DefaultDate);
                    DateTime pwdlastset = Utility.ConvertValue<DateTime>(server.Element("pwdlastset").Value, Utility.DefaultDate);
                    
                    ADservers.Add(new ActivedirectoryServer()
                    {
                        CN = cn,
	                    RidSetReferences = ridsetreferences,
	                    OperatingSystem = operatingsystem,
                        OperatingSystemServicepack = operatingsystemservicepack,
                        OperatingSystemVersion = operatingsystemversion,
	                    LastLogonTimestamp = lastlogontimestamp,
	                    WhenCreated = whencreated,
	                    ADSpath = adspath,
	                    WhenChanged = whenchanged,
	                    Memberof = memberof,
	                    DNShostname = dnshostname,
	                    DistinguishedName = distinguishedname,
	                    PWlastset = pwdlastset,
	                    Publishedat = publishedat
                    });
                }
            }
            return ADservers;
        }

        private void ProcessActivedirectoryFile(string filePath, string newFileName)
        { //processing
            List<ActivedirectoryServer> adservers= null;

            try
            { ////pro try
                if (File.Exists(filePath))
                { //if filepath exists
                    //System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
                    #region Loading Objects
                    
                    bool result = false;
                    XDocument xdoc = XDocument.Load(filePath);
                    IEnumerable<XElement> configs = xdoc.Elements();
                    if (configs.Count() > 0)
                    { // if 2
                        var list = configs.ToList();
                        var configElms = list[0].Elements();

                        foreach (var item in configElms)
                        {
                            switch (item.Name.LocalName)
                            {         
                                case "Computers":
                                    adservers = ExtractActivedirectoryServers(item);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #region Processing Objects -----------------------

                        if (adservers != null )
                            {
                                LoggingManager.Instance.GetLogger.Info("Insert Activedirectory Servers");
                                result = Utility.InsertActivedirectoryServers(adservers);
                                LoggingManager.Instance.GetLogger.Info("End Activedirectory Servers Insert");
                            }

                       #endregion end processing Objects

                    } // if 2
                    #endregion

                    if (result)
                    {
                        try
                        {
                            //File.Copy(filePath, Path.Combine(destinationPath, name), true);
                            //string destinationPathProcessed = WebConfigurationManager.AppSettings["DestinationProcessed"];
                            //File.Copy(filePath, Path.Combine(destinationPathProcessed, newFileName), true);
                            LoggingManager.Instance.GetLogger.Info(string.Format("Deleting file {0}", filePath));
                            File.Delete(filePath);
                        }
                        catch (Exception ex)
                        {
                            LoggingManager.Instance.GetLogger.Error("File copy was not successful.", ex);
                        }
                    }
                    else
                    {
                        string destinationPathFailed = WebConfigurationManager.AppSettings["DestinationFailed"];
                        File.Copy(filePath, Path.Combine(destinationPathFailed, newFileName), true);
                        LoggingManager.Instance.GetLogger.Error(String.Format("Partial data was transfered to DB."));
                        File.Delete(filePath);
                    }

                } // if filepath exists
                else
                {
                    LoggingManager.Instance.GetLogger.Error(String.Format("File <{0}> could not be found. Aborting...", filePath));
                }

            } //end proc try
            catch (Exception ex)
            {
                LoggingManager.Instance.GetLogger.Error("Fatal exception thrown. Aborting...", ex);
            }
        } 

        #endregion --------------------------------------------------------------

    }
}