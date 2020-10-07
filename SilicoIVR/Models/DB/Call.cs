using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SilicoIVR.Models
{
    public class Call
    {
        
        public int ID { get; set; }
        public string SID { get; set; }
        public string Name { get; set; }
        public string CallerType { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int? AgentCalledID { get; set; }

        [ForeignKey("AgentCalledID")]
        public Agent AgentCalled { get; set; }
    }
}
