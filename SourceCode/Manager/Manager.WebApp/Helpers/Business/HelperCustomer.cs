using System;
using System.Collections.Generic;
using Serilog;
using MainApi.DataLayer.Entities;
using Manager.WebApp.Models;
using Manager.WebApp.Services;
using System.Reflection;

namespace Manager.WebApp.Helpers
{
    public class HelperCustomer
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperCustomer));
        public static IdentityCustomer GetBaseInfo(int id)
        {
            IdentityCustomer baseInfo = null;

            try
            {
                var apiRs = CustomerServices.GetDetailAsync(new CustomerUpdateModel { Id = id }).Result;

                baseInfo = apiRs.ConvertData<IdentityCustomer>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static IdentityCoin GetPoint(string id)
        {
            IdentityCoin baseInfo = null;

            try
            {
                var apiRs = CustomerServices.GetPoint(id).Result;

                baseInfo = apiRs.ConvertData<IdentityCoin>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static List<CoinHistoryView> GetHistoryPoint(string id, int currentpage, int pagesize)
        {
            List<CoinHistoryView> baseInfo = null;

            try
            {
                var apiRs = CustomerServices.GetHistoryPoint(id, currentpage, pagesize).Result;

                baseInfo = apiRs.ConvertData<List<CoinHistoryView>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static List<IdentityCustomer> GetList()
        {
            List<IdentityCustomer> list = null;

            try
            {
                var apiRs = CustomerServices.GetListAsync().Result;

                list = apiRs.ConvertData<List<IdentityCustomer>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return list;
        }
    }
}