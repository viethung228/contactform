using Autofac;
using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Stores;
using MainApi.SharedLibs;
using MainApi.Helpers;
using MainApi.Models;
using MainApi.Resources;
using MainApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using NetCoreMVC.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;
using System.Net.Http;

namespace MainApi.Controllers.Manager
{
    //[Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : AuthorizeManagerController
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ApiResponseModel Index()
        {
            var returnModel = new ApiResponseModel();

            try
            {
                ManageUserModel model = new ManageUserModel();
                model.Page = Utils.ConvertToInt32(Request.Query["Page"]);
                model.PageSize = Utils.ConvertToInt32(Request.Query["PageSize"]);
                model.Keyword = Request.Query["Keyword"];

                if (model.PageSize <= 0 || model.PageSize > SystemSettings.DefaultPageSize) model.PageSize = SystemSettings.DefaultPageSize;
                if (model.Page <= 0) model.Page = 1;

                var filter = new IdentityUser
                {
                    Keyword = !string.IsNullOrEmpty(model.Keyword) ? model.Keyword.ToStringNormally() : string.Empty,
                    PageSize = model.PageSize,
                    PageIndex = model.Page,
                };

                var store = Startup.IocContainer.Resolve<IStoreUser>();

                var rs = store.GetByPage(filter, model.Page, model.PageSize);
                if (rs.HasData())
                {
                    var returnList = new List<IdentityUser>();
                    foreach (var item in rs)
                    {
                        var info = HelperUser.GetBaseInfo(item.StaffId);
                        info.TotalCount = item.TotalCount;

                        if (info != null)
                        {
                            returnList.Add(info);
                        }
                    }

                    model.SearchResults = returnList.MappingObject<List<UserDetailModel>>();
                }

                returnModel.Data = model.SearchResults;
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = EnumCommonCode.Error;
            }

            return returnModel;
        }

        [HttpGet("{id}")]
        public ApiResponseModel GetById(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var info = HelperUser.GetBaseInfo(id);
                if (info != null)
                {
                    returnModel.Data = info.MappingObject<UserDetailModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0}r error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = EnumCommonCode.Error;
            }

            return returnModel;
        }

        [HttpGet]
        [Route("all")]
        public ApiResponseModel GetList()
        {
            var returnModel = new ApiResponseModel();

            try
            {
                returnModel.Data = HelperUser.GetList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = (int)EnumCommonCode.Error;
            }

            return returnModel;
        }

        [HttpPost]
        [Route("update")]
        public ApiResponseModel Update(UserUpdateModel model)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                if (model.ReceiveAllUpdate == null) model.ReceiveAllUpdate = false;
               
                var store = Startup.IocContainer.Resolve<IStoreUser>();
                var identity = store.GetByStaffId(model.Id).MappingObject<IdentityUser>();
                identity.PhoneNumber = model.PhoneNumber;
                identity.FullName = model.FullName;
                identity.ReceiveAllUpdate = (bool)model.ReceiveAllUpdate;
                identity.RemoveAds = (bool)model.RemoveAds;
                identity.Avatar = model.CoverImage;
                //Update DB
                var returnId = store.Update(identity);

                returnModel.Data = returnId;

                //Clear cache
                HelperUser.ClearCache(model.Id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = EnumCommonCode.Error;
            }

            return returnModel;
        }

        [HttpDelete("{id}")]
        public ApiResponseModel Delete(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var info = HelperUser.GetBaseInfo(id);
                if (info != null)
                {
                    var store = Startup.IocContainer.Resolve<IStoreUser>();

                    store.Delete(info.StaffId);

                    //Clear cache
                    HelperUser.ClearCache(info.StaffId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = EnumCommonCode.Error;
            }

            return returnModel;
        }

    }
}
