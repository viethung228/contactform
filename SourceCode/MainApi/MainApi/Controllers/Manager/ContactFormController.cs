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
using Stripe;

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

        [HttpPost("contact_form")]
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
        [HttpPost("dependent")]
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
        [HttpPost("allowance")]
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
        [HttpPost("allowance_type")]
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


        [HttpGet("contact_form")]
        public ApiResponseModel GetById(int id)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.GetContactFormByFormId(id);
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

        [HttpGet]
        public ApiResponseModel GetByPage(string keyword, int currentpage = 1, int pagesize = 10)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.GetByPage(keyword, currentpage, pagesize);
                if (returnId != null && returnId.Count > 0)
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

        [HttpGet("getall")]
        public ApiResponseModel GetList()
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.GetList();
                if (returnId != null && returnId.Count > 0)
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

        [HttpGet("contact_form_employee")]
        public ApiResponseModel GetByEmployeeId(int id)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.GetContactFormByEmployeeId(id);
                if (returnId != null && returnId.Count > 0)
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
        [HttpGet("dependent")]
        public ApiResponseModel GetDependentsByFormId(int id)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.GetDependentsByFormId(id);
                if (returnId != null && returnId.Count > 0)
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
        [HttpGet("allowance")]
        public ApiResponseModel GetAllowanceByFormId(int id)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.GetAllowanceByFormId(id);
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
        [HttpGet("allowance_type")]
        public ApiResponseModel GetAllowanceDetailByAllowanceId(int id)
        {

            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                //Update DB
                var returnId = store.GetAllowanceDetailByAllowanceId(id);
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
        [HttpGet("getallcompany")]
        public ApiResponseModel GetAllCompany(string companyName, int currentpage = 1, int pagesize = 10)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                ManageContactFormModel model = new ManageContactFormModel();
                model.Page = currentpage;
                model.PageSize = pagesize;
                model.Keyword = companyName;

                if (model.PageSize <= 0 || model.PageSize > SystemSettings.DefaultPageSize) model.PageSize = SystemSettings.DefaultPageSize;
                if (model.Page <= 0) model.Page = 1;

                var filter = new IdentityContactForm
                {
                    Keyword = !string.IsNullOrEmpty(model.Keyword) ? model.Keyword.ToStringNormally() : string.Empty,
                    PageSize = model.PageSize,
                    PageIndex = model.Page,
                };

                var store = Startup.IocContainer.Resolve<IStoreContactForm>();

                var rs = store.GetAllCompany(filter, model.Page, model.PageSize);
                if (rs.HasData())
                {
                    var returnList = new List<IdentityContactForm>();
                    foreach (var item in rs)
                    {
                        var info = item;
                        info.TotalCount = rs.FirstOrDefault().TotalCount;
                        if (info != null)
                        {
                            returnList.Add(info);
                        }
                    }

                    model.SearchResults = returnList.MappingObject<List<ContactFormDetailModel>>();
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
        [HttpGet("getcontactformbycompany")]
        public ApiResponseModel GetAllContactFormByCompany(string keyword, string companyName, int currentpage = 1, int pagesize = 10)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                ManageContactFormModel model = new ManageContactFormModel();
                model.Page = currentpage;
                model.PageSize = pagesize;
                model.Keyword = keyword;

                if (model.PageSize <= 0 || model.PageSize > SystemSettings.DefaultPageSize) model.PageSize = SystemSettings.DefaultPageSize;
                if (model.Page <= 0) model.Page = 1;

                var filter = new IdentityContactForm
                {
                    Keyword = !string.IsNullOrEmpty(model.Keyword) ? model.Keyword.ToStringNormally() : string.Empty,
                    PageSize = model.PageSize,
                    PageIndex = model.Page,
                };

                var store = Startup.IocContainer.Resolve<IStoreContactForm>();

                var rs = store.GetContactFormByCompanyName(filter.Keyword, companyName, model.Page, model.PageSize);
                if (rs.HasData())
                {
                    var returnList = new List<IdentityContactForm>();
                    foreach (var item in rs)
                    {
                        var info = item;
                        info.TotalCount = rs.FirstOrDefault().TotalCount;
                        if (info != null)
                        {
                            returnList.Add(info);
                        }
                    }

                    model.SearchResults = returnList.MappingObject<List<ContactFormDetailModel>>();
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
        [HttpDelete("{id}")]
        public ApiResponseModel DeleteContactForm(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreContactForm>();
                var returnId = store.GetContactFormByFormId(id);
                if (returnId != null)
                {
                    var getResult = store.DeleteContactForm(id);
                    if (getResult != null && getResult.FormId != 0)
                    {
                        returnModel.Data = getResult;
                        returnModel.Message = ManagerResource.LB_DELETE_SUCCESS;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = EnumCommonCode.Error;
            }
            if (returnModel.Data == null)
            {
                returnModel.Code = EnumCommonCode.Error;
            }
            return returnModel;
        }
    }
}
