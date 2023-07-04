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
    public class RevenueController : AuthorizeManagerController
    {
        private readonly ILogger<RevenueController> _logger;

        public RevenueController(ILogger<RevenueController> logger)
        {
            _logger = logger;
        }
        [HttpGet("gethistorybyid")]
        public ApiResponseModel GetHistoryById(string id, int pagesize = 3, int currentpage = 1)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreUser>();
                var infoUser = store.GetById(id);
                if (infoUser != null)
                {
                    var rev_store = Startup.IocContainer.Resolve<IStoreRevenue>();
                    var getResult = rev_store.GetHistoryById(id, pagesize, currentpage);
                    if (getResult != null && getResult.Count > 0)
                    {
                        var listData = new List<RevenueViewModel>();
                        foreach (var item in getResult)
                        {
                            var SourceName = (SourceCoinType)item.SourceType;
                            listData.Add(new RevenueViewModel
                            {
                                UserName = infoUser.UserName,
                                Price = item.Price,
                                Coin = item.Coin,
                                SourceType = SourceName.ToString(),
                                CreatedDate = item.CreatedDate,
                                TotalCount = item.TotalCount
                            });
                        }
                        returnModel.Data = listData;
                        returnModel.Code = EnumCommonCode.Success;
                        returnModel.Message = "Successfully";
                    }
                    else
                    {
                        returnModel.Data = null;
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Message = "No history found.";
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

        [HttpGet("gethistoryall")]
        public ApiResponseModel GetHistoryAll(RevenueDetailModel identity, int pagesize = 3, int currentpage = 1)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var dat = new IdentityRevenue();
                dat = identity;
                var rev_store = Startup.IocContainer.Resolve<IStoreRevenue>();
                var getResult = rev_store.GetHistoryAll(dat, pagesize, currentpage);
                if (getResult != null && getResult.Count > 0)
                {
                    var listData = new List<RevenueViewModel>();
                    foreach (var item in getResult)
                    {
                        var SourceName = (SourceCoinType)item.SourceType;
                        var store = Startup.IocContainer.Resolve<IStoreUser>();
                        var infoUser = store.GetById(item.UserId);
                        if (infoUser != null)
                        {
                            listData.Add(new RevenueViewModel
                            {
                                UserName = infoUser.UserName,
                                Price = item.Price,
                                Coin = item.Coin,
                                SourceType = SourceName.ToString(),
                                CreatedDate = item.CreatedDate,
                                TotalCount = item.TotalCount
                            });
                        }

                    }

                    returnModel.Data = listData;
                    returnModel.Code = EnumCommonCode.Success;
                    returnModel.Message = "Successfully";
                }
                else
                {
                    returnModel.Data = null;
                    returnModel.Code = EnumCommonCode.Error;
                    returnModel.Message = "No history found.";
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
