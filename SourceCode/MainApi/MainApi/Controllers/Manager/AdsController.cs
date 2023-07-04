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
    public class AdsController : AuthorizeManagerController
    {
        private readonly ILogger<AdsController> _logger;

        public AdsController(ILogger<AdsController> logger)
        {
            _logger = logger;
        }
        [HttpPost("removeads")]
        public ApiResponseModel RemoveAds(string id)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreCustomer>();
                var infoUser = store.GetById(id);
                if (infoUser != null)
                {
                    var ads_store = Startup.IocContainer.Resolve<IStoreAds>();
                    var getResult = ads_store.RemoveAds(id);
                    if (getResult != null && getResult.UserId != null)
                    {
                        returnModel.Data = new Dictionary<string, bool>
                        {
                            {"IsRemoveAds",getResult.RemoveAds }
                        };
                        returnModel.Code = EnumCommonCode.Success;
                        returnModel.Message = "Successfully";
                    }
                    else
                    {
                        returnModel.Data = null;
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Message = "Remove Ads fail.";
                    }
                }
                else
                {
                    returnModel.Data = null;
                    returnModel.Code = EnumCommonCode.Error;
                    returnModel.Message = "No user found.";
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

        [HttpPost("getstatus")]
        public ApiResponseModel GetStatus(string id)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreCustomer>();
                var infoUser = store.GetById(id);
                if (infoUser != null)
                {
                    var ads_store = Startup.IocContainer.Resolve<IStoreAds>();
                    var getResult = ads_store.GetStatus(id);
                    if (getResult != null)
                    {
                        returnModel.Data = new Dictionary<string, bool>
                        {
                            {"IsRemoveAds",getResult.RemoveAds }
                        };
                        returnModel.Code = EnumCommonCode.Success;
                        returnModel.Message = "Successfully";
                    }
                    else
                    {
                        returnModel.Data = null;
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Message = "Get status fail.";
                    }
                }
                else
                {
                    returnModel.Data = null;
                    returnModel.Code = EnumCommonCode.Error;
                    returnModel.Message = "No user found.";
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
