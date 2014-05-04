using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTajm.Backend.Json
{
    class Customer
    {
        public Int64? id { get; set; }
        public String created { get; set; }
        public String name { get; set; }
        public String line1 { get; set; }
        public String line2 { get; set; }
        public String state { get; set; }
        public String zip { get; set; }
        public String city { get; set; }
        public String country { get; set; }
        public String referencePerson { get; set; }
        public String personId { get; set; }
        public String vatRegistrationNumber { get; set; }
        public String bank { get; set; }
        public String invoiceNumber { get; set; }
    }
}
