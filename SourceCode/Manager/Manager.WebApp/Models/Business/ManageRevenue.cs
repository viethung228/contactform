using MainApi.DataLayer.Entities;
using System.Collections.Generic;
using System;
using MainApi.DataLayer.Entities.Entities;
using Manager.WebApp.Resources;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Manager.WebApp.Helpers;

namespace Manager.WebApp.Models
{
    public class ManageRevenueModel : CommonPagingModel
    {
        public List<RevenueViewModel> SearchResults { get; set; }
        public int CoinSourceType { get; set; }
        public string UserName { get; set; }
        public List<string> ListUserName { get; set; }
        public ManageRevenueModel()
        {
            ListUserName = new List<string>();
            CoinSourceType = (int)EnumSourceCoinType.All;
            ToDate = DateTime.Now;
        }
    }
    public class RevenueDetailModel : IdentityRevenue
    {
        public string CurrentTab { get; set; }
        public List<RevenueViewModel> SearchResult { get; set; }

    }
    public class ChartRevenueModel
    {

    }

}
