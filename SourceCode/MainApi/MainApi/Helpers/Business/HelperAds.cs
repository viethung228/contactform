using Autofac;
using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Stores;
using MainApi.Settings;
using System.Reflection;
using System;
using Serilog;
using System.Collections.Generic;

namespace MainApi.Helpers
{
    public class HelperAds
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperAds));

        public static IdentityAds GetStatus(string id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Ads, id);
            IdentityAds baseInfo = null;

            try
            {
                //Check from cache first
                var myStore = Startup.IocContainer.Resolve<IStoreAds>();
                baseInfo = myStore.GetStatus(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static IdentityAds RemoveAds(string id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Ads, id);
            IdentityAds baseInfo = null;

            try
            {
                //Check from cache first
                var myStore = Startup.IocContainer.Resolve<IStoreAds>();
                baseInfo = myStore.RemoveAds(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static void ClearCache(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Coin, id);
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Clear(myKey);
                cacheProvider.Clear(EnumListCacheKeys.Coins);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
        }
    }
}
