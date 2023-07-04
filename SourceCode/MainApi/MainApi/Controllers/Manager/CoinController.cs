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
    public class CoinController : AuthorizeManagerController
    {
        private readonly ILogger<CoinController> _logger;

        public CoinController(ILogger<CoinController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getpoint")]
        public ApiResponseModel GetPointById(string id)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var info = HelperCoin.GetPointById(id);
                var store = Startup.IocContainer.Resolve<IStoreCustomer>();
                var infoUser = store.GetById(id);
                if (infoUser != null)
                {
                    if (info != null && info.userId != null)
                    {
                        returnModel.Data = info.MappingObject<CoinModel>();
                        returnModel.Message = "Successfully";
                    }
                    else
                    {
                        returnModel.Data = new CoinModel
                        {
                            userId = id,
                            point = 0

                        };
                        returnModel.Message = "Successfully";
                    }
                }
                else
                {
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

        [HttpGet("gethistorypoint")]
        public ApiResponseModel GetHistoryPointById(string id, int currentpage = 1, int pagesize = 5, bool getall = false)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var info = HelperCoin.GetHistoryPointById(id, currentpage, pagesize);
                if (getall)
                {
                    info = HelperCoin.GetHistoryPointById(id, currentpage, (int)info.FirstOrDefault().TotalCount);
                }
                var store = Startup.IocContainer.Resolve<IStoreCustomer>();
                var infoUser = store.GetById(id);
                var infoView = new List<CoinHistoryView>();
                if (infoUser != null)
                {
                    if (info != null)
                    {
                        foreach (var item in info)
                        {
                            var SourceTypeName = (SourceCoinType)item.SourceType;
                            infoView.Add(new CoinHistoryView
                            {
                                ValueChange = item.ValueChange,
                                SourceType = SourceTypeName.ToString(),
                                CreatedDate = item.CreatedDate,
                                TotalCount = (int)item.TotalCount,
                            });
                        }
                        returnModel.Data = infoView;
                        returnModel.Message = "Successfully";
                    }

                }
                else
                {
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

        [HttpPost]
        [Route("purchase")]
        public ApiResponseModel Purchase(string userId, int valueChange)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreCustomer>();
                var infoUser = store.GetById(userId);
                if (infoUser != null)
                {
                    var info = HelperCoin.UpdateCoin(userId, valueChange, (int)SourceCoinType.InAppPurchase);
                    if (info != null)
                    {
                        returnModel.Data = info;
                        returnModel.Message = "Successfully";
                    }
                    else
                    {
                        returnModel.Data = null;
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Message = "Purchase fail.";
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
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = EnumCommonCode.Error;
            }

            return returnModel;
        }
    }
}
