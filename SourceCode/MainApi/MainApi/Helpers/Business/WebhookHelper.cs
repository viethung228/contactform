using Autofac;
using MainApi.Settings;
using System.Reflection;
using System;
using Serilog;
using MainApi.DataLayer.Entities.Entities;
using MainApi.DataLayer.Stores.Manager;
using MainApi.DataLayer;
using MainApi.DataLayer.Stores;
using System.Collections.Generic;
using MainApi.DataLayer.Entities;
using MainApi.SharedLibs;

namespace MainApi.Helpers.Business
{
    public class WebhookHelper
    {
        private static readonly ILogger _logger = Log.ForContext<WebhookHelper>();
        public static void ClearCache(string id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Noshi, id);
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Clear(myKey);
                cacheProvider.Clear(EnumListCacheKeys.Noshies);
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
        }
    }
}
