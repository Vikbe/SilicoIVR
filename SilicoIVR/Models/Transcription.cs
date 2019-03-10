using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilicoIVR.Models
{
    public class Transcription
    {
        public string TranscriptionSid { get; set; }
        public string TranscriptionText { get; set; }
        public string TranscriptionStatus { get; set; }
        public string TranscriptionUrl { get; set; }
        public string RecordingSid { get; set; }
        public string RecordingUrl { get; set; }
        public string CallSid { get; set; }
        public string AccountSid { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CallStatus { get; set; }
        public string ApiVersion { get; set; }
        public string Direction { get; set; }
        public string ForwardedFrom { get; set; }

    }
}
