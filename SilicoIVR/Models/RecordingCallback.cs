using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilicoIVR.Models
{
    public class RecordingCallback
    {
        public string AccountSid { get; set; }
        public string CallSid { get; set; }
        public string RecordingSid { get; set; }
        public string RecordingUrl { get; set; }
        public string RecordingStatus { get; set; }
        public string RecordingDuration { get; set; }
        public string RecordingChannels { get; set; }
        public string RecordingSource { get; set; }

    }
}
