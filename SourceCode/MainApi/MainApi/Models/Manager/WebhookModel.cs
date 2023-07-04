using System;
using System.ComponentModel.DataAnnotations;

namespace MainApi.Models
{
    public class WebhookModel
    {
        public string suid { get; set; }
        public string spram1 { get; set; }
        public string spram2 { get; set; }
        [Required]
        public int xad { get; set; }
        public DateTime approve_date { get; set; }
        public DateTime cv_date { get; set; }
        public int price { get; set; }
        public int cv_id { get; set; }
        [Required]
        public int wallpoint { get; set; }
        [Required]
        public int mcv_no { get; set; }
        public string display_name { get; set; }
        public string OS { get; set; }

    }
}
