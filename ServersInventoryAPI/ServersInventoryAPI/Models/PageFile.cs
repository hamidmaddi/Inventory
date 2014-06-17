using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class PageFile
    {
        public string Name { get; set; }
        public int FileSize { get; set; }
        public string FileType { get; set; }
        public bool Compressed { get; set; }
        public string CompressionMethod { get; set; }
        public bool Encrypted { get; set; }
        public string EncryptionMethod { get; set; }
        public bool Hidden { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime LastAccessed { get; set; }
        public DateTime LastModified { get; set; }
    }
}
