using System;
using System.Collections.Generic;
using Serilog;
using MainApi.DataLayer.Entities;
using Manager.WebApp.Models;
using Manager.WebApp.Services;
using System.Reflection;

namespace Manager.WebApp.Helpers
{
    public class HelperRevenue
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperRevenue));

        public static List<RevenueViewModel> GetHistoryAll(RevenueDetailModel identity, int currentpage, int pagesize)
        {
            List<RevenueViewModel> baseInfo = null;

            try
            {
                var apiRs = RevenueServices.GetHistoryAll(identity, currentpage, pagesize).Result;

                baseInfo = apiRs.ConvertData<List<RevenueViewModel>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static List<RevenueViewModel> GetHistoryById(string id, int currentpage, int pagesize)
        {
            List<RevenueViewModel> baseInfo = null;

            try
            {
                var apiRs = RevenueServices.GetHistoryById(id, currentpage, pagesize).Result;

                baseInfo = apiRs.ConvertData<List<RevenueViewModel>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
    }
}