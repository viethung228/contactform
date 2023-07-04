using Autofac;
using MainApi.DataLayer.Stores;

using MainApi.DataLayer.Stores;
using MainApi.DataLayer.Stores.Manager;
using MainApi.Helpers;
using MainApi.Services;
using Manager.WebApp.Services;

namespace MainApi.AutofacDI
{
    public class SqlModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CacheProvider>().As<ICacheProvider>();

            builder.RegisterType<StoreSetting>().As<IStoreSetting>();
            builder.RegisterType<SettingsService>().As<ISettingsService>();
            
            builder.RegisterType<StoreUser>().As<IStoreUser>();
            builder.RegisterType<StoreCustomer>().As<IStoreCustomer>();
            builder.RegisterType<StoreActivity>().As<IStoreActivity>();
            builder.RegisterType<StoreRole>().As<IStoreRole>();
            builder.RegisterType<StoreAccessRoles>().As<IStoreAccessRoles>();
           

            builder.RegisterType<StoreRegion>().As<IStoreRegion>();
            builder.RegisterType<StorePrefecture>().As<IStorePrefecture>();
            builder.RegisterType<StoreCity>().As<IStoreCity>();
            builder.RegisterType<StoreNotification>().As<IStoreNotification>();
            #region Business
            builder.RegisterType<StoreLinkSetting>().As<IStoreLinkSetting>();
            builder.RegisterType<StoreSkyflag>().As<IStoreSkyflag>();
            builder.RegisterType<StoreApplovin>().As<IStoreApplovin>();
            builder.RegisterType<StoreCoin>().As<IStoreCoin>();
            builder.RegisterType<StoreAds>().As<IStoreAds>();
            builder.RegisterType<StoreRevenue>().As<IStoreRevenue>();

            //builder.RegisterType<MailService>().As<IMailService>();
            #endregion

            #region Public


            #endregion

        }
    }
}