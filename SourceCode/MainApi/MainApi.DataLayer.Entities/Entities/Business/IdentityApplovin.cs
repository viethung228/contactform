using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApi.DataLayer.Entities.Entities.Business
{
    public class IdentityApplovin : IdentityCommon
    {
        public string ad_unit_id { get; set; }
        public string ad_unit_name { get; set; }
        public int amount { get; set; }
        public string cc { get; set; }
        public string currency { get; set; }
        public string custom_data { get; set; }
        public string event_id { get; set; }
        public string event_token { get; set; }
        public string event_token_all { get; set; }
        public string idfa { get; set; }
        public string idfv { get; set; }
        public string ip { get; set; }
        public string network_name { get; set; }
        public string package_name { get; set; }
        public string placement { get; set; }
        public string platform { get; set; }
        public string user_id { get; set; }
        public DateTime ts { get; set; }
        
    }
}
