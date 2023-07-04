using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApi.DataLayer.Entities.Entities.Business
{
    public class IdentitySkyflag : IdentityCommon
    {
        public string suid { get; set; }
        public string spram1 { get; set; }
        public string spram2 { get; set; }
        public int xad { get; set; }
        public DateTime approve_date { get; set; }
        public DateTime cv_date { get; set; }
        public int price { get; set; }
        public int cv_id { get; set; }
        public int wallpoint { get; set; }
        public int mcv_no { get; set; }
        public string display_name { get; set; }
        public string OS { get; set; }
    }
}
