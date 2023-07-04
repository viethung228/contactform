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
    public class HelperUser
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperUser));

        public static IdentityUser GetBaseInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.User, id);
            IdentityUser baseInfo = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                baseInfo = cacheProvider.Get<IdentityUser>(myKey);
                //var myStore = Startup.IocContainer.Resolve<IStoreUser>();
                //baseInfo = myStore.GetByStaffId(id);
                if (baseInfo == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreUser>();
                    baseInfo = myStore.GetByStaffId(id);

                    if (baseInfo != null)
                    {
                        //Storage to cache
                        cacheProvider.Set(myKey, baseInfo, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        public static List<IdentityUser> GetList()
        {
            var myKey = EnumListCacheKeys.Users;
            List<IdentityUser> list = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                list = cacheProvider.Get<List<IdentityUser>>(myKey);

                if (list == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreUser>();
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
            var myKey = string.Format(EnumFormatInfoCacheKeys.User, id);
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Clear(myKey);
                cacheProvider.Clear(EnumListCacheKeys.Users);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
        }
    }
}
