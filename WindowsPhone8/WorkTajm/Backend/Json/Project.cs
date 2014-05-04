using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTajm.Backend.Json
{
    class Project
    {
        public long? id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal? rate { get; set; }
        public string lastModified{ get; set; }
        public long? customerId { get; set; }
    }
}
