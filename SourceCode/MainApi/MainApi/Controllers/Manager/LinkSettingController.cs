using Autofac;
using MainApi.DataLayer.Stores;
using MainApi.Helpers;
using MainApi.Models;
using MainApi.Resources;
using MainApi.Settings;
using MainApi.SharedLibs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using System;
using MainApi.DataLayer.Entities;
using MainApi.DataLayer;
using MainApi.SharedLibs.Extensions;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Linq;

namespace MainApi.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkSettingController : AuthorizeManagerController
    {
        private readonly ILogger<LinkSettingController> _logger;

        public LinkSettingController(ILogger<LinkSettingController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public ApiResponseModel GetById(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var info = HelperLinkSetting.GetById(id);
                if (info != null)
                {
                    returnModel.Data = info.MappingObject<LinkSettingDetailModel>();
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
        public ApiResponseModel GetByPage(int currentpage = 1, int pagesize = 10)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                ManageLinkSettingModel model = new ManageLinkSettingModel();
                model.Page = currentpage;
                model.PageSize = pagesize;
                model.Keyword = Request.Query["Keyword"];

                if (model.PageSize <= 0 || model.PageSize > SystemSettings.DefaultPageSize) model.PageSize = SystemSettings.DefaultPageSize;
                if (model.Page <= 0) model.Page = 1;

                var filter = new IdentityLinkSetting
                {
                    Keyword = !string.IsNullOrEmpty(model.Keyword) ? model.Keyword.ToStringNormally() : string.Empty,
                    PageSize = model.PageSize,
                    PageIndex = model.Page,
                };

                var store = Startup.IocContainer.Resolve<IStoreLinkSetting>();

                var rs = store.GetByPage(filter, model.Page, model.PageSize);
                if (rs.HasData())
                {
                    var returnList = new List<IdentityLinkSetting>();
                    foreach (var item in rs)
                    {
                        var info = item;
                        info.TotalCount = rs.FirstOrDefault().TotalCount;
                        if (info != null)
                        {
                            returnList.Add(info);
                        }
                    }

                    model.SearchResults = returnList.MappingObject<List<LinkSettingDetailModel>>();
                }

                returnModel.Data = model.SearchResults;
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0}r error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = EnumCommonCode.Error;
            }

            return returnModel;
        }

        [HttpPost("update")]
        public ApiResponseModel Update(LinkSettingUpdateModel identity)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreLinkSetting>();
                var data = store.GetById(identity.Id).MappingObject<IdentityLinkSetting>();
                if (data != null)
                {
                    identity.Link = data.Type ? identity.CoverImage : identity.Link;
                }

                //Update DB
                var returnId = store.Update(identity.Id, identity.Link, identity.SettingName, identity.Type);
                if (returnId != null)
                {
                    returnModel.Data = returnId;
                    returnModel.Message = ManagerResource.LB_UPDATE_SUCCESS;
                }
                else
                {
                    returnModel.Code = EnumCommonCode.Error;
                    returnModel.Message = ManagerResource.LB_ERROR_OCCURED;
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

        [HttpPost("insert")]
        public ApiResponseModel Insert(string settingName, bool? type, string link)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreLinkSetting>();

                //Update DB
                var returnId = store.Insert(settingName, type, link);
                if (returnId != null && returnId.Id > 0)
                {
                    returnModel.Data = returnId;
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

        [HttpGet("getlink")]
        public ApiResponseModel GetLink(string settingName)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var info = HelperLinkSetting.GetLinkByName(settingName);
                if (info != null)
                {
                    returnModel.Message = ManagerResource.LB_OPERATION_SUCCESS;
                    returnModel.Data = info.MappingObject<LinkSettingDetailModel>().Link;
                }
                else
                {
                    returnModel.Code = EnumCommonCode.Error;
                    returnModel.Message = ManagerResource.LB_NO_RESULTS_FOUND;
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
    }
}
