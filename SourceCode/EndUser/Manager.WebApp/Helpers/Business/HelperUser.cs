using System;
using System.Collections.Generic;
using Serilog;
using MainApi.DataLayer.Entities;
using Manager.WebApp.Models;
using Manager.WebApp.Services;
using System.Reflection;

namespace Manager.WebApp.Helpers
{
    public class HelperUser
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperUser));
        public static IdentityUser GetBaseInfo(int id)
        {
            IdentityUser baseInfo = null;

            try
            {
                var apiRs = UserServices.GetDetailAsync(new UserUpdateModel { Id = id }).Result;

                baseInfo = apiRs.ConvertData<IdentityUser>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        
        public static List<IdentityUser> GetList()
        {
            List<IdentityUser> list = null;

            try
            {
                var apiRs = UserServices.GetListAsync().Result;

                list = apiRs.ConvertData<List<IdentityUser>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return list;
        }
    }
}