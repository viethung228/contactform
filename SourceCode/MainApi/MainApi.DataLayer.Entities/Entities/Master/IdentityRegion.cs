using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MainApi.DataLayer.Entities
{
    public class IdentityRegion : IdentityCommon
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Furigana { get; set; }

        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }

        [JsonIgnore]
        public DateTime? LastUpdated { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        public List<IdentityPrefecture> Prefectures { get; set; }
    }
}
