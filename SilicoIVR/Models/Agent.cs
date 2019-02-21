using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SilicoIVR.Models
{
    public class Agent
    {
        [Key]
        public int ID { get; set; }
        public int Extension { get; set; }
        public string PhoneNumber { get; set; }

        public string Name { get; set; }

    }
}
