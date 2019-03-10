using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SilicoIVR.Models.DB
{
    public class Recording
    {
        public int ID { get; set; }
        public string SID { get; set; }
        public double duration { get; set; }
        public int CallID { get; set; }

        [ForeignKey("CallID")]
        public Call Call { get; set; }
    }
}
