using System;
using System.Collections.Generic;
using Serilog;
using MainApi.DataLayer.Entities;
using Manager.WebApp.Models;
using Manager.WebApp.Services;
using System.Reflection;

namespace Manager.WebApp.Helpers
{
    public class HelperEmployee
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperEmployee));
        public static IdentityEmployee GetBaseInfo(int id)
        {
            IdentityEmployee baseInfo = null;

            try
            {
                var apiRs = EmployeeServices.GetDetailAsync(new EmployeeUpdateModel { Id = id }).Result;

                baseInfo = apiRs.ConvertData<IdentityEmployee>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
       
        public static List<IdentityEmployee> GetList()
        {
            List<IdentityEmployee> list = null;

            try
            {
                var apiRs = EmployeeServices.GetListAsync().Result;

                list = apiRs.ConvertData<List<IdentityEmployee>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return list;
        }
    }
}