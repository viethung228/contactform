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
    public class HelperEmployee
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperEmployee));

        public static IdentityEmployee GetBaseInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Employee, id);
            IdentityEmployee baseInfo = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                baseInfo = cacheProvider.Get<IdentityEmployee>(myKey);
                var myStore = Startup.IocContainer.Resolve<IStoreEmployee>();
                baseInfo = myStore.GetByStaffId(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        public static List<IdentityEmployee> GetList()
        {
            var myKey = EnumListCacheKeys.Employees;
            List<IdentityEmployee> list = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                list = cacheProvider.Get<List<IdentityEmployee>>(myKey);

                if (list == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreEmployee>();
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
            var myKey = string.Format(EnumFormatInfoCacheKeys.Employee, id);
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Clear(myKey);
                cacheProvider.Clear(EnumListCacheKeys.Employees);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
        }
    }
}
