using MainApi.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MainApi.Models
{
    public class ApiOrderModel
    {
        public string CreatedAfter { get; set; }
        public string CreatedBefore { get; set; }
        public string MarketplaceIds { get; set; }
        public int MaxResultsPerPage { get; set; }
        public string details { get; set; }
        public string NextToken { get; set; }
    }

    public class ApiOrderDetailModel
    {
        public string ShopId { get; set; }
        public string Token { get; set; }
        public string OrderCode { get; set; }
    }

}
