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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PrefectureController : AuthorizeBaseController
    {
        private readonly ILogger<PrefectureController> _logger;

        public PrefectureController(ILogger<PrefectureController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public ApiResponseModel GetById(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var info = CommonHelpers.GetBaseInfoPrefecture(id);
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

        [HttpGet("listbyregion/{id}")]
        public ApiResponseModel GetListByRegion(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                returnModel.Data = CommonHelpers.GetListPrefecturesByRegion(id);
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
