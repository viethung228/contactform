using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MainApi.Helpers;
using MainApi.Models;
using System;
using System.Threading.Tasks;
using MainApi.DataLayer.Entities;
using Autofac;
using MainApi.DataLayer.Stores;
using MainApi.SharedLibs;
using System.Collections.Generic;
using Manager.WebApp.Services;
using MainApi.Resources;
using System.Reflection;
using MainApi.Settings;
using Stripe.Checkout;

namespace MainApi.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : BaseController
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly IMailService mailService;
        public CompanyController(ILogger<CompanyController> logger, IMailService mailService)
        {
            _logger = logger;
            this.mailService = mailService;
        }
        [HttpGet]
        public ApiResponseModel Index()
        {
            var returnModel = new ApiResponseModel();

            try
            {
                ManageCompanyModel model = new ManageCompanyModel();
                model.Page = Utils.ConvertToInt32(Request.Query["Page"]);
                model.PageSize = Utils.ConvertToInt32(Request.Query["PageSize"]);
                model.Keyword = Request.Query["Keyword"];

                if (model.PageSize <= 0 || model.PageSize > SystemSettings.DefaultPageSize) model.PageSize = SystemSettings.DefaultPageSize;
                if (model.Page <= 0) model.Page = 1;

                var filter = new IdentityCompany
                {
                    Keyword = !string.IsNullOrEmpty(model.Keyword) ? model.Keyword.ToStringNormally() : string.Empty,
                    PageSize = model.PageSize,
                    PageIndex = model.Page,
                };

                var store = Startup.IocContainer.Resolve<IStoreCompany>();

                var rs = store.GetByPage(filter, model.Page, model.PageSize);
                if (rs.HasData())
                {
                    var returnList = new List<IdentityCompany>();
                    foreach (var item in rs)
                    {
                        var info = HelperCompany.GetBaseInfo(item.CompanyId);
                        info.TotalCount = item.TotalCount;

                        if (info != null)
                        {
                            returnList.Add(info);
                        }
                    }

                    model.SearchResults = returnList.MappingObject<List<CompanyDetailModel>>();
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
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "")
        {

            LoginViewModel model = new LoginViewModel();
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost("Login")]
        public ApiResponseModel Login(LoginViewModel model)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                if (string.IsNullOrEmpty(model.ReturnUrl))
                    model.ReturnUrl = "/";

                if (ModelState.IsValid)
                {
                    model.UserName = model.UserName.ToStringNormally();

                    var pwd = model.Password.ToStringNormally();
                    pwd = Utility.Md5HashingData(pwd);
                    model.Password = pwd;
                    var storeCompany = Startup.IocContainer.Resolve<IStoreCompany>();

                    var company = storeCompany.Login(new IdentityCompany { Email = model.Email, PasswordHash = pwd });
                    var idCompany = "";

                    if (company != null)
                    {
                        idCompany = company.Id;
                        company.Id = company.CompanyId.ToString();
                        returnModel.Data = company;
                        if (company.LockoutEnabled)
                        {
                            returnModel.Code = EnumCommonCode.Error;
                            returnModel.Message = "Your account was locked !!!";
                            return returnModel;
                        }
                    }
                    else
                    {
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Message = "The companyname or password is incorrect";
                        return returnModel;
                    }
                    if (!company.EmailConfirmed)
                    {
                        var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Data = null;
                        returnModel.Token = null;
                        returnModel.Message = string.Format("Your email {0} did not verified.", company.Email);
                    }
                }
            }
            catch (Exception ex)
            {
                returnModel.Message = "Could not login: " + returnModel.Message;
                _logger.LogDebug("Could not login: " + ex.ToString());
            }

            //Send confirm email
            if (returnModel.Message == null)
                returnModel.Message = "Login successfully.";
            return returnModel;

        }

        [HttpGet("{id}")]
        public ApiResponseModel GetById(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var info = HelperCompany.GetBaseInfo(id);
                if (info != null)
                {
                    returnModel.Data = info.MappingObject<CompanyDetailModel>();
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

        [HttpGet("getbyname")]
        public ApiResponseModel GetByName(string companyFullName)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreCompany>();
                var info = store.GetByCompanyName(companyFullName);
                if (info != null)
                {
                    returnModel.Data = info.MappingObject<CompanyDetailModel>();
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

        [HttpGet("getbyemail")]
        public ApiResponseModel GetByEmail(string email)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreCompany>();
                var info = store.GetByEmail(email);
                if (info != null)
                {
                    returnModel.Data = info.MappingObject<CompanyDetailModel>();
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
                returnModel.Data = HelperCompany.GetList();
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
        public ApiResponseModel Update(CompanyUpdateModel model)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var store = Startup.IocContainer.Resolve<IStoreCompany>();
                var identity = store.GetByCompanyId(model.Id).MappingObject<IdentityCompany>();
                if (identity == null)
                {
                    var hashData = GenereateHashingForNewAccount(model.Email, model.CompanyName);

                    var newCompany = store.Insert(new IdentityCompany
                    {
                        CompanyName = model.CompanyName,
                        Email = model.Email,
                        PasswordHash = "e10adc3949ba59abbe56e057f20f883e",
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber,
                        SecurityStamp = hashData,
                        Avatar = model.CoverImage,
                        CreatedDateUtc = DateTime.UtcNow
                    });
                    if (!string.IsNullOrEmpty(newCompany))
                    {
                        returnModel.Data = true;
                    }
                }
                else
                {
                    identity.PhoneNumber = model.PhoneNumber;
                    identity.FullName = model.FullName;
                    identity.Avatar = model.CoverImage;
                    identity.Address = model.Address;
                    //Update DB
                    var returnId = store.Update(identity);
                    returnModel.Data = returnId;
                    //Clear cache
                    HelperCompany.ClearCache(model.Id);
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

        [HttpDelete("{id}")]
        public ApiResponseModel Delete(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var info = HelperCompany.GetBaseInfo(id);
                if (info != null)
                {
                    var store = Startup.IocContainer.Resolve<IStoreCompany>();

                    store.Delete(info.CompanyId);

                    //Clear cache
                    HelperCompany.ClearCache(info.CompanyId);
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
