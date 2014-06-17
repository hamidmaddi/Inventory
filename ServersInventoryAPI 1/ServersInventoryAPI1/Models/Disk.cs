using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI1
{
    public class Disk
    {
        public string Drive { get; set; }
        public string PhysicalDisk { get; set; }
        public string Model { get; set; }
        public string Partition { get; set; }
        public string Description { get; set; }
        public bool PrimaryPartition { get; set; }
        public string VolumeName { get; set; }
        public int DiskSize { get; set; }
        public int FreeSpace { get; set; }
        public float PercentageFree { get; set; }
        public string DiskType { get; set; }
        public string SerialNumber { get; set; }
    }
}
