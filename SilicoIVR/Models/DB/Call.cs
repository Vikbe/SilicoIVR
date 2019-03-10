using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilicoIVR.Models
{
    public class Call
    {
        
        public int ID { get; set; }
        public string SID { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
