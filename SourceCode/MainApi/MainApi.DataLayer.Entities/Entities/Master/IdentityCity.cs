using Newtonsoft.Json;
using System;

namespace MainApi.DataLayer.Entities
{
    public class IdentityCity : IdentityCommon
    {
        public int Id { get; set; }

        public int PrefectureId { get; set; }

        public string Name { get; set; }

        public string Furigana { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }

        [JsonIgnore]
        public DateTime? LastUpdated { get; set; }
    }
}
