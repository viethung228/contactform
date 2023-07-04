using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MainApi.DataLayer.Entities
{
    public class IdentityCommon
    {
        public string? Keyword { get; set; }

        //[JsonIgnore]
        public int TotalCount { get; set; }

        [JsonIgnore]
        public DateTime? FromDate { get; set; }

        [JsonIgnore]
        public DateTime? ToDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string? SortField { get; set; }

        //ASC or DESC
        public string? SortType { get; set; }
    }

    public class IdentityMediaFile
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Type { get; set; }
        public string? Url { get; set; }
        public bool IsDefault { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
