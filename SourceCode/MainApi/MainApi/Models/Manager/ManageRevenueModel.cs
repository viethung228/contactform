using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace MainApi.Models
{
    public class ManageRevenueModel : CommonPagingModel
    {
        public List<RevenueDetailModel> SearchResults { get; set; }
    }

    public class RevenueDetailModel : IdentityRevenue
    {

    }
   
}
