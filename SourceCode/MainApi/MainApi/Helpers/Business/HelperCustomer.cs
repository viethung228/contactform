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
    public class HelperCustomer
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperCustomer));

        public static IdentityCustomer GetBaseInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Customer, id);
            IdentityCustomer baseInfo = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                baseInfo = cacheProvider.Get<IdentityCustomer>(myKey);
                var myStore = Startup.IocContainer.Resolve<IStoreCustomer>();
                baseInfo = myStore.GetByStaffId(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        public static List<IdentityCustomer> GetList()
        {
            var myKey = EnumListCacheKeys.Customers;
            List<IdentityCustomer> list = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                list = cacheProvider.Get<List<IdentityCustomer>>(myKey);

                if (list == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreCustomer>();
                    list = myStore.GetList();

                    if (list != null)
                    {
                        //Storage to cache
                        cacheProvider.Set(myKey, list, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return list;
        }
        public static void ClearCache(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Customer, id);
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Clear(myKey);
                cacheProvider.Clear(EnumListCacheKeys.Customers);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
        }
    }
}
