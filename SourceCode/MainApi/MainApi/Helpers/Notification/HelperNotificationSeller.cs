//using Autofac;
//using MainApi.DataLayer.Entities;
//using MainApi.DataLayer.Stores;
//using System.Collections.Generic;
//using System.Reflection;
//using System;
//using MainApi.SharedLibs;
//using System.Linq;
//using Serilog;

//namespace MainApi.Helpers.Notification
//{
//    public class HelperNotificationSeller
//    {
//        private static readonly ILogger _logger = Log.ForContext(typeof(HelperNotificationManager));
//        public static void SellerCreated(int sellerId, int agencyId)
//        {
//            try
//            {
//                var _notiStore = Startup.IocContainer.Resolve<IStoreNotification>();

//                var notiInfo = new IdentityNotification();
//                notiInfo.ActionType = (int)EnumNotificationActionTypeForSeller.SellerCreated;
//                notiInfo.SenderId = 0; // 0: system
//                notiInfo.TargetType = (int)EnumNotificationTargetType.Seller;
//                notiInfo.TargetId = sellerId;
//                notiInfo.UserType = (int)EnumNotificationUserType.KyoKai;

//                var sellerStore = Startup.IocContainer.Resolve<IStoreSeller>();

//                //GetAll sellers who have permission
//                var listAccs = sellerStore.GetSellersByPermission(agencyId);
//                var listIds = new List<int>();
//                if (listAccs.HasData())
//                {
//                    listIds = listAccs.Select(x => x.Id).ToList();
//                    if (listIds.HasData())
//                    {
//                        _notiStore.MultiplePush(listIds, notiInfo);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
//            }
//        }


//        public static void SellerUpdated(int sellerId, int agencyId)
//        {
//            try
//            {
//                var _notiStore = Startup.IocContainer.Resolve<IStoreNotification>();

//                var notiInfo = new IdentityNotification();
//                notiInfo.ActionType = (int)EnumNotificationActionTypeForSeller.SellerUpdated;
//                notiInfo.SenderId = 0; // 0: system
//                notiInfo.TargetType = (int)EnumNotificationTargetType.Seller;
//                notiInfo.TargetId = sellerId;
//                notiInfo.UserType = (int)EnumNotificationUserType.KyoKai;

//                var sellerStore = Startup.IocContainer.Resolve<IStoreSeller>();

//                //GetAll sellers who have permission
//                var listAccs = sellerStore.GetSellersByPermission(agencyId);
//                var listIds = new List<int>();
//                if (listAccs.HasData())
//                {
//                    listIds = listAccs.Select(x => x.Id).ToList();
//                    if (listIds.HasData())
//                    {
//                        _notiStore.MultiplePush(listIds, notiInfo);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
//            }
//        }


//        public static void CustomerCreated(int customerId, int agencyId)
//        {
//            try
//            {
//                var _notiStore = Startup.IocContainer.Resolve<IStoreNotification>();

//                var notiInfo = new IdentityNotification();
//                notiInfo.ActionType = (int)EnumNotificationActionTypeForCustomer.CustomerCreated;
//                notiInfo.SenderId = 0;
//                notiInfo.TargetType = (int)EnumNotificationTargetType.Customer;
//                notiInfo.TargetId= customerId;
//                notiInfo.UserType = (int)EnumNotificationUserType.KyoKai;


//                var customerStore = Startup.IocContainer.Resolve<IStoreCustomer>();
//                //GetAll sellers who have permission
//                var listAccs = customerStore.GetListBySellerId(agencyId);
//                var listIds = new List<int>();
//                if (listAccs.HasData())
//                {
//                    listIds = listAccs.Select(x => x.Id).ToList();
//                    if (listIds.HasData())
//                    {
//                        _notiStore.MultiplePush(listIds, notiInfo);
//                    }
//                }

//            }
//            catch(Exception ex)
//            {
//                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
//            }
//        }
        
        
//        public static void CustomerUpdated(int customerId, int agencyId)
//        {
//            try
//            {
//                var _notiStore = Startup.IocContainer.Resolve<IStoreNotification>();

//                var notiInfo = new IdentityNotification();
//                notiInfo.ActionType = (int)EnumNotificationActionTypeForCustomer.CustomerUpdated;
//                notiInfo.SenderId = 0;
//                notiInfo.TargetType = (int)EnumNotificationTargetType.Customer;
//                notiInfo.TargetId= customerId;
//                notiInfo.UserType = (int)EnumNotificationUserType.KyoKai;


//                var customerStore = Startup.IocContainer.Resolve<IStoreCustomer>();
//                //GetAll sellers who have permission
//                var listAccs = customerStore.GetListBySellerId(agencyId);
//                var listIds = new List<int>();
//                if (listAccs.HasData())
//                {
//                    listIds = listAccs.Select(x => x.Id).ToList();
//                    if (listIds.HasData())
//                    {
//                        _notiStore.MultiplePush(listIds, notiInfo);
//                    }
//                }

//            }
//            catch(Exception ex)
//            {
//                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
//            }
//        }
//    }
//}
