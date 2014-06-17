using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServersInventoryAPI
{
    public class NICbinding
    {
        public int BindingOrder { get; set; }
        public string Name { get; set; }
        public bool NICenabled { get; set; }
        public string GUID { get; set; }
    }
}
