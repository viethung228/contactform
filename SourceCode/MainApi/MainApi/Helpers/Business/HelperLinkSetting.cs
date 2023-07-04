using Autofac;
using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Stores;
using MainApi.Settings;
using System.Reflection;
using System;
using Serilog;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MainApi.Models;

namespace MainApi.Helpers
{
    public class HelperLinkSetting
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperLinkSetting));

        public static IdentityLinkSetting GetById(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.LinkSetting, id);
            IdentityLinkSetting baseInfo = null;

            try
            {

                //Check from cache first
                var myStore = Startup.IocContainer.Resolve<IStoreLinkSetting>();
                baseInfo = myStore.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        public static IdentityLinkSetting GetLinkByName(string settingName)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.LinkSetting, settingName);
            IdentityLinkSetting baseInfo = null;

            try
            {

                //Check from cache first
                var myStore = Startup.IocContainer.Resolve<IStoreLinkSetting>();
                baseInfo = myStore.GetLinkByName(settingName);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        public static List<IdentityLinkSetting> GetByPage(IdentityLinkSetting identity, int currentpage, int pagesize)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.LinkSetting, identity.Id);
            List<IdentityLinkSetting> baseInfo = null;

            try
            {

                //Check from cache first
                var myStore = Startup.IocContainer.Resolve<IStoreLinkSetting>();
                baseInfo = myStore.GetByPage(identity, currentpage, pagesize);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        //public static IdentityLinkSetting Update(LinkSettingUpdateModel identity)
        //{
        //    var myKey = string.Format(EnumFormatInfoCacheKeys.LinkSetting, identity.Id);
        //    IdentityLinkSetting baseInfo = null;

        //    try
        //    {
        //        //Check from cache first
        //        var myStore = Startup.IocContainer.Resolve<IStoreLinkSetting>();
        //        baseInfo = myStore.Update(identity.Id, identity.Link);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
        //    }

        //    return baseInfo;
        //}
        public static void ClearCache(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.LinkSetting, id);
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Clear(myKey);
                cacheProvider.Clear(EnumListCacheKeys.LinkSettings);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
        }
    }
}
