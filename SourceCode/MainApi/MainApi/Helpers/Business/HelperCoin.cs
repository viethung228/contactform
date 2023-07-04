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
    public class HelperCoin
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperCoin));

        public static IdentityCoin GetPointById(string id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Coin, id);
            IdentityCoin baseInfo = null;

            try
            {

                //Check from cache first
                var myStore = Startup.IocContainer.Resolve<IStoreCoin>();
                baseInfo = myStore.GetPointById(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static List<CoinHistory> GetHistoryPointById(string id, int currentpage, int pagesize)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Coin, id);
            List<CoinHistory> baseInfo = null;

            try
            {

                //Check from cache first
                var myStore = Startup.IocContainer.Resolve<IStoreCoin>();
                baseInfo = myStore.GetHistoryPointById(id, currentpage, pagesize);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static CoinHistory UpdateCoin(string userId, int valueChange, int sourceType)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Coin, userId);
            CoinHistory baseInfo = null;

            try
            {
                //Check from cache first
                var myStore = Startup.IocContainer.Resolve<IStoreCoin>();
                baseInfo = myStore.UpdateCoin(userId, valueChange, sourceType);
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
