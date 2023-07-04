using System;
using System.ComponentModel.DataAnnotations;

namespace MainApi.Models
{
    public class AdsModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool RemoveAds { get; set; }
        public DateTime UpdatedDate { get; set; }


    }
}
