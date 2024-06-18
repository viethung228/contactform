using System;
using System.Collections.Generic;
using Serilog;
using MainApi.DataLayer.Entities;
using Manager.WebApp.Models;
using Manager.WebApp.Services;
using System.Reflection;

namespace Manager.WebApp.Helpers
{
    public class HelperCompany
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperCompany));
        public static IdentityCompany GetBaseInfo(int id)
        {
            IdentityCompany baseInfo = null;

            try
            {
                var apiRs = CompanyServices.GetDetailAsync(new CompanyUpdateModel { Id = id }).Result;

                baseInfo = apiRs.ConvertData<IdentityCompany>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static IdentityCompany GetBaseByCompanyNameInfo(string companyName)
        {
            IdentityCompany baseInfo = null;

            try
            {
                var apiRs = CompanyServices.GetDetailByNameAsync(companyName).Result;

                baseInfo = apiRs.ConvertData<IdentityCompany>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        public static List<IdentityCompany> GetList()
        {
            List<IdentityCompany> list = null;

            try
            {
                var apiRs = CompanyServices.GetListAsync().Result;

                list = apiRs.ConvertData<List<IdentityCompany>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return list;
        }
    }
}