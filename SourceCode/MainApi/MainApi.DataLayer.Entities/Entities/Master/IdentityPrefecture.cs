using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MainApi.DataLayer.Entities
{
    public class IdentityPrefecture : IdentityCommon
    {
        public int Id { get; set; }

        public int RegionId { get; set; }

        public string Name { get; set; }

        public string Furigana { get; set; }

        public List<IdentityCity> Cities { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }

        [JsonIgnore]
        public DateTime? LastUpdated { get; set; }

    }
}
