using Autofac;
using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Stores;
using MainApi.SharedLibs;
using MainApi.Helpers;
using MainApi.Models;
using MainApi.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Stripe;
using System.Linq;
using System.Diagnostics.Metrics;

namespace MainApi.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : AuthorizeManagerController
    {
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ApiResponseModel Index()
        {
            var returnModel = new ApiResponseModel();

            try
            {
                var filter = new IdentityNotification();

                filter.UserId = GetCurrentUserId();
                filter.UserType = (int)EnumNotificationUserType.Manager;
                filter.IsUpdateView = true;

                var pageIndex = Utils.ConvertToInt32(HttpContext.Request.Query["Page"]);
                if (pageIndex == 0)
                    pageIndex = 1;

                var pageSize = 20;

                var store = Startup.IocContainer.Resolve<IStoreNotification>();
                var listNotif = store.GetByUser(filter, pageIndex, pageSize);

                var returnList = new List<NotificationItemModel>();
                if (listNotif.HasData())
                {
                    foreach (var n in listNotif)
                    {
                        var it = new NotificationItemModel { NotifInfo = n };

                        returnList.Add(it);
                    }
                }

                returnModel.Data = returnList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = (int)EnumCommonCode.Error;
            }

            return returnModel;
        }

        [HttpGet("{id}")]
        public ApiResponseModel GetById(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreNotification>();
                var info = store.GetById(id);

                if (info != null)
                {
                    var identity = new IdentityNotification();
                    identity.Id = id;
                    identity.UserId = GetCurrentUserId();
                    identity.UserType = (int)EnumNotificationUserType.Manager;
                    identity.IsRead = true;

                    if (!info.IsRead)
                    {
                        store.MarkIsRead(identity);
                    }

                    returnModel.Data = info;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = (int)EnumCommonCode.Error;
            }

            return returnModel;
        }

        [HttpGet("unreadcount")]
        public ApiResponseModel GetUnreadCount()
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreNotification>();
                var filter = new IdentityNotification();
                filter.UserType = (int)EnumNotificationUserType.Manager;
                filter.UserId = GetCurrentUserId();

                var counter = store.CountUnread(filter);

                returnModel.Data = counter;
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = (int)EnumCommonCode.Error;
            }

            return returnModel;
        }
        [AllowAnonymous]
        [HttpPost("create")]
        public ApiResponseModel CreateNotification(int actionType, int targetType, int agencyId, int contactFormId)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                if (agencyId > 0)
                {
                    var _notifStore = Startup.IocContainer.Resolve<IStoreNotification>();

                    var notifInfo = new IdentityNotification();
                    notifInfo.ActionType = actionType;

                    notifInfo.SenderId = 0; //Sender: 0 - system
                    notifInfo.TargetType = targetType;

                    notifInfo.TargetId = contactFormId;
                    notifInfo.UserType = (int)EnumNotificationUserType.Manager;

                    var userStore = Startup.IocContainer.Resolve<IStoreUser>();

                    //Get all users who have the permission
                    var listAccs = userStore.GetUsersByPermission(agencyId, "Index", "UsersAdmin");
                    var listIds = new List<int>();
                    if (listAccs.HasData())
                    {
                        listIds = listAccs.Select(x => x.StaffId).ToList();
                        if (listIds.HasData())
                        {
                            var getResult = _notifStore.MultiplePush(listIds, notifInfo);

                            returnModel.Data = getResult;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = (int)EnumCommonCode.Error;
            }

            return returnModel;
        }

        //[HttpPost("notifstaff")]
        //public ApiResponseModel SendNotificationByStaff(HouseUpdateModel model)
        //{
        //    var returnModel = new ApiResponseModel();
        //    try
        //    {
        //        //Send notification
        //        var identity = model.MappingObject<IdentityHouse>();
        //        HelperNotificationManager.HouseAssignForStaff(identity);

        //        //Send message via LINE
        //        HelperLineUser.SendNewHouseAssignToStaff(identity);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

        //        returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
        //        returnModel.Code = (int)EnumCommonCode.Error;
        //    }

        //    return returnModel;
        //}
    }
}
