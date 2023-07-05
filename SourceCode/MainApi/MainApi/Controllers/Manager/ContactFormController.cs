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
    public class ContactFormController : AuthorizeManagerController
    {
        private readonly ILogger<ContactFormController> _logger;

        public ContactFormController(ILogger<ContactFormController> logger)
        {
            _logger = logger;
        }
        
        [HttpPost("update/contact_form")]
        public ApiResponseModel Insert(ContactFormDetailModel identity)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.InsertContactForm(identity);
                if (returnId != null && returnId.FormId > 0)
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
        [HttpPost("update/dependent")]
        public ApiResponseModel InsertDependent(DependentDetailModel identity)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.InsertDependent(identity);
                if (returnId != null && returnId.DependentId > 0)
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
        [HttpPost("update/allowance")]
        public ApiResponseModel InsertAllowance(AllowanceDetailModel identity)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.InsertAllowance(identity);
                if (returnId != null && returnId.AllowanceId > 0)
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
        [HttpPost("update/allowance_type")]
        public ApiResponseModel InsertAllowanceType(AllowanceTypeDetailModel identity)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.InsertAllowanceDetail(identity);
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
    }
}
