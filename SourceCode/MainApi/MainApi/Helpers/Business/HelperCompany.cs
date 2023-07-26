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
    public class HelperCompany
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperCompany));

        public static IdentityCompany GetBaseInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Company, id);
            IdentityCompany baseInfo = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                baseInfo = cacheProvider.Get<IdentityCompany>(myKey);
                var myStore = Startup.IocContainer.Resolve<IStoreCompany>();
                baseInfo = myStore.GetByCompanyId(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        public static List<IdentityCompany> GetList()
        {
            var myKey = EnumListCacheKeys.Companies;
            List<IdentityCompany> list = null;

            try
            {
                var myStore = Startup.IocContainer.Resolve<IStoreCompany>();
                list = myStore.GetList();
               
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return list;
        }
        public static void ClearCache(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Company, id);
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Clear(myKey);
                cacheProvider.Clear(EnumListCacheKeys.Companies);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
        }
    }
}
