using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilicoIVR.Models
{

    public class CallerInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
    }



    public class BizInfoResult
    {
        public string status { get; set; }
        public string e164 { get; set; }
        public string error { get; set; }
        public BizInfoCaller result { get; set; }

    }

    public class BizInfoCaller
    {
        public string city { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string state { get; set; }
        public string suite { get; set; }
        public string street { get; set; }
        public string website { get; set; }
        public string postal_code { get; set; }
        public string facebook_url { get; set; }
    }


    public class WhitepagesLatLong
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string accuracy { get; set; }
    }

    public class WhitepagesCurrentAddress
    {
        public string city { get; set; }
        public WhitepagesLatLong lat_long { get; set; }
        public string is_active { get; set; }
        public string location_type { get; set; }
        public string street_line_2 { get; set; }
        public string link_to_person_start_date { get; set; }
        public string street_line_1 { get; set; }
        public string postal_code { get; set; }
        public string delivery_point { get; set; }
        public string country_code { get; set; }
        public string state_code { get; set; }
        public string id { get; set; }
        public string zip4 { get; set; }
    }

    public class WhitepagesResult
    {
        public string phone_number { get; set; }
        public List<string> warnings { get; set; }
        public List<string> historical_addresses { get; set; }
        public List<string> alternate_phones { get; set; }
        public string error { get; set; }
        public string is_commercial { get; set; }
        public List<string> associated_people { get; set; }
        public string country_calling_code { get; set; }
        public List<string> belongs_to { get; set; }
        public bool is_valid { get; set; }
        public string line_type { get; set; }
        public string carrier { get; set; }
        public List<WhitepagesCurrentAddress> current_addresses { get; set; }
        public string id { get; set; }
        public string is_prepaid { get; set; }
    }

    public class WhitepagesRoot
    {
        public string status { get; set; }
        public string request_sid { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public WhitepagesResult result { get; set; }
    }
}
