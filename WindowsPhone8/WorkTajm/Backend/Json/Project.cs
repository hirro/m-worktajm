using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTajm.Backend.Json
{
    class Project
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Rate { get; set; }
        public string LastModified{ get; set; }
        public long? CustomerId { get; set; }
    }
}
