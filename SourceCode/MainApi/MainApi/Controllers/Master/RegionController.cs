using MainApi.Helpers;
using MainApi.Models;
using MainApi.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using NetCoreMVC.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace MainApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RegionController : AuthorizeBaseController
    {
        private readonly ILogger<RegionController> _logger;

        public RegionController(ILogger<RegionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ApiResponseModel GetList()
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var info = CommonHelpers.GetListRegions(81);
                returnModel.Data = info;
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
                var info = CommonHelpers.GetBaseInfoRegion(id);
                returnModel.Data = info;
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = (int)EnumCommonCode.Error;
            }

            return returnModel;
        }

        [HttpGet("listbyprefecture/{id}")]
        public ApiResponseModel GetListByPrefecture(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                returnModel.Data = CommonHelpers.GetListCitiesByPrefecture(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = (int)EnumCommonCode.Error;
            }

            return returnModel;
        }
    }
}
