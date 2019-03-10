using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilicoIVR.Models
{
    public class WatsonResult
    {
    }

    public class Alternative
    {
        public List<List<object>> timestamps { get; set; }
        public double confidence { get; set; }
        public string transcript { get; set; }
    }

    public class Result2
    {
        public List<Alternative> alternatives { get; set; }
        public bool final { get; set; }
    }

    public class Result
    {
        public List<Result2> results { get; set; }
        public int result_index { get; set; }
        public List<string> warnings { get; set; }
    }

    public class WatsonRoot
    {
        public string user_token { get; set; }
        public List<Result> results { get; set; }
        public string id { get; set; }
        public string @event { get; set; }
    }
}
