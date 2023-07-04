//using System;
//using System.Linq;
//using Manager.WebApp.Helpers;
//using Manager.WebApp.Resources;
//using Manager.SharedLibs;
//using Microsoft.Extensions.Logging;
//using Manager.WebApp.Models;
//using MainApi.DataLayer.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Manager.DataLayer.Stores;
//using Manager.DataLayer;
//using Autofac;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using Manager.WebApp.Services;
//using System.Reflection;
//using System.Diagnostics;

//namespace Manager.WebApp.Controllers
//{
//    public class AuthedGlobalController : BaseAuthedController
//    {
//        private readonly ILogger<AuthedGlobalController> _logger;

//        public AuthedGlobalController(ILogger<AuthedGlobalController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpPost]
//        public async Task<ActionResult> GetNotifCount()
//        {
//            var isSuccess = false;
//            var msg = string.Empty;
//            var counter = 0;
//            try
//            {
//                var apiRs = NotificationServices.GetUnreadCountAsync().Result;
                
//                counter = apiRs.ConvertData<int>();

//                await Task.FromResult(counter);

//                isSuccess = true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
//            }

//            return Json(new { success = isSuccess, message = msg, data = counter });
//        }

//        [HttpPost]
//        public async Task<ActionResult> GetNotification()
//        {
//            var isSuccess = false;
//            var htmlReturn = string.Empty;
//            List<NotificationItemModel> returnList = new List<NotificationItemModel>();

//            var msg = string.Empty;
//            try
//            {
//                var pageIndex = Utils.ConvertToInt32(HttpContext.Request.Query["Page"]);
//                if (pageIndex == 0)
//                    pageIndex = 1;

//                var filter = new ManageNotificationModel();
//                filter.Page = pageIndex;
//                filter.PageSize = 20;

//                var apiRs = NotificationServices.GetByPageAsync(filter).Result;
//                var listNotif = apiRs.ConvertData<List<NotificationItemModel>>();

//                await Task.FromResult(listNotif);
                
//                isSuccess = true;
//                htmlReturn = PartialViewAsString("../Widgets/Items/Notification/_NotificationItems", listNotif);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
//            }

//            return Json(new { success = isSuccess, message = msg, html = htmlReturn });
//        }

//        public async Task<ActionResult> ReadNotif(int? id = 0, int? targetId = 0)
//        {
//            var model = new NotificationItemModel();
//            try
//            {
//                var notifId = Utils.ConvertToIntFromNullable(id);
//                if (notifId == 0)
//                    return RedirectToErrorPage();

//                var apiRs = NotificationServices.GetNotificationByIdAsync(notifId).Result;
//                var info = apiRs.ConvertData<IdentityNotification>();

//                await Task.FromResult(info);
//                if (info != null)
//                {
//                    if (info.TargetType == (int)EnumNotificationTargetType.Product)
//                    {
//                        if (info.ActionType == (int)EnumNotificationActionTypeForManager.ProductCreated)
//                        {
//                            if (targetId != null && targetId > 0)
//                            {

//                                var rs = ProductServices.GetDetailAsync(new ProductDetailModel { Id = (int)targetId }).Result;
//                                var course = rs.ConvertData<IdentityProduct>();
//                                if (course != null)
//                                {
//                                    var link = SecurityHelper.GenerateSecureLink("Product", "Index", new { id = targetId});
//                                    return Redirect(link);
//                                }
//                            }
//                        }

//                        if (info.ActionType == (int)EnumNotificationActionTypeForManager.ProductUpdated)
//                        {
//                            if (targetId != null && targetId > 0)
//                            {
//                                var rs = ProductServices.GetDetailAsync(new ProductDetailModel { Id = (int)targetId }).Result;
//                                var course = rs.ConvertData<IdentityProduct>();
//                                if (course != null)
//                                {
//                                    var link = SecurityHelper.GenerateSecureLink("Product", "Index", new { id = targetId});
//                                    return Redirect(link);
//                                }
//                            }
//                        }
//                    }
//                    if (info.TargetType == (int)EnumNotificationTargetType.Order)
//                    {
//                        if (info.ActionType == (int)EnumNotificationActionTypeForManager.OrderCreated)
//                        {
//                            if (targetId > 0)
//                            {
//                                var link = SecurityHelper.GenerateSecureLink("Order", "Index", new { id = targetId});
//                                return Redirect(link);
//                            }
//                        }
//                    }
//                    if (info.TargetType == (int)EnumNotificationTargetType.Customer)
//                    {
//                        if (info.ActionType == (int)EnumNotificationActionTypeForManager.CustomerCreated)
//                        {
//                            if (targetId != null && targetId > 0)
//                            {
//                                var rs = CustomerServices.GetDetailAsync(new CustomerDetailModel { Id = (int)targetId }).Result;
//                                var course = rs.ConvertData<IdentityCustomer>();
//                                if (course != null)
//                                {
//                                    var link = SecurityHelper.GenerateSecureLink("Customer", "Index", new { id = targetId});
//                                    return Redirect(link);
//                                }
//                            }
//                        }

//                        if (info.ActionType == (int)EnumNotificationActionTypeForManager.CustomerUpdated)
//                        {
//                            if (targetId != null && targetId > 0)
//                            {
//                                var rs = CustomerServices.GetDetailAsync(new CustomerDetailModel { Id = (int)targetId }).Result;
//                                var course = rs.ConvertData<IdentityCustomer>();
//                                if (course != null)
//                                {
//                                    var link = SecurityHelper.GenerateSecureLink("Customer", "Index", new { id = targetId});
//                                    return Redirect(link);
//                                }
//                            }
//                        }
                        
//                        if (info.ActionType == (int)EnumNotificationActionTypeForManager.CustomerRegister)
//                        {
//                            if (targetId != null && targetId > 0)
//                            {
//                                var rs = CustomerServices.GetDetailAsync(new CustomerDetailModel { Id = (int)targetId }).Result;
//                                var course = rs.ConvertData<IdentityCustomer>();
//                                if (course != null)
//                                {
//                                    var link = SecurityHelper.GenerateSecureLink("Customer", "Index", new { id = targetId}); 
//                                    return Redirect(link);
//                                }
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    return RedirectToErrorPage();
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
//            }

//            return RedirectToErrorPage();
//        }
//    }
//}
