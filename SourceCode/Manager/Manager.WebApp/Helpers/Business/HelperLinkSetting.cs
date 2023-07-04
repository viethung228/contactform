using System;
using System.Collections.Generic;
using Serilog;
using MainApi.DataLayer.Entities;
using Manager.WebApp.Models;
using Manager.WebApp.Services;
using System.Reflection;

namespace Manager.WebApp.Helpers
{
    public class HelperLinkSetting
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperLinkSetting));
        public static IdentityLinkSetting GetBaseInfo(int id)
        {
            IdentityLinkSetting baseInfo = null;

            try
            {
                var apiRs = LinkSettingServices.GetDetailAsync(id).Result;

                baseInfo = apiRs.ConvertData<IdentityLinkSetting>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        //public static List<IdentityLinkSetting> GetList()
        //{
        //    List<IdentityLinkSetting> list = null;

        //    try
        //    {
        //        var apiRs = LinkSettingServices.GetListAsync().Result;

        //        list = apiRs.ConvertData<List<IdentityLinkSetting>>();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
        //    }

        //    return list;
        //}
    }
}