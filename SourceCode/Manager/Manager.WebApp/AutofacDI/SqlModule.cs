using Autofac;
using MainApi.DataLayer;
using Manager.DataLayer.Stores;
using Manager.WebApp.Helpers;
using Manager.WebApp.Services;

namespace Manager.WebApp.AutofacDI
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
            builder.RegisterType<StoreRevenue>().As<IStoreRevenue>();

            builder.RegisterType<StoreActivity>().As<IStoreActivity>();
            builder.RegisterType<StoreRole>().As<IStoreRole>();
            builder.RegisterType<StoreAccessRoles>().As<IStoreAccessRoles>();
            builder.RegisterType<StoreNotification>().As<IStoreNotification>();
           // builder.RegisterType<StoreLineUser>().As<IStoreLineUser>();

            builder.RegisterType<StoreEmailServer>().As<IStoreEmailServer>();

            //builder.RegisterType<StoreSales>().As<IStoreSales>();
            //builder.RegisterType<StoreAmazonInventory>().As<IStoreAmazonInventory>();
            //builder.RegisterType<StoreAmazonOrder>().As<IStoreAmazonOrder>();
            //builder.RegisterType<StoreProduct>().As<IStoreProduct>();
            //builder.RegisterType<StoreProductLine>().As<IStoreProductLine>();
            //builder.RegisterType<StoreImport>().As<IStoreImport>();
            //builder.RegisterType<StoreSalesAccount>().As<IStoreSalesAccount>();
        }
    }
}