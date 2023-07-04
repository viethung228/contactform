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

namespace MainApi.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IMailService mailService;
        public CustomerController(ILogger<CustomerController> logger, IMailService mailService)
        {
            _logger = logger;
            this.mailService = mailService;
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
                    var storeCustomer = Startup.IocContainer.Resolve<IStoreCustomer>();

                    var customer = storeCustomer.Login(new IdentityCustomer { UserName = model.UserName, PasswordHash = pwd });
                    var idCustomer = "";

                    if (customer != null)
                    {
                        idCustomer = customer.Id;
                        customer.Id = customer.StaffId.ToString();
                        returnModel.Data = customer;
                        if (customer.LockoutEnabled)
                        {
                            returnModel.Code = EnumCommonCode.Error;
                            returnModel.Message = "Your account was locked !!!";
                            return returnModel;
                        }
                        returnModel.Token = Utility.GenerateAuthCustomerJwtToken(customer, "ogatore");
                    }
                    else
                    {
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Message = "The customername or password is incorrect";
                        return returnModel;
                    }
                    if (!customer.EmailConfirmed)
                    {
                        var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Data = null;
                        returnModel.Token = null;
                        returnModel.Message = string.Format("Your email {0} did not verified.", customer.Email);
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

        //public ApiResponseModel ResendEmail(int staffId)
        //{
        //    var returnModel = new ApiResponseModel();
        //    var storeCustomer = Startup.IocContainer.Resolve<IStoreCustomer>();
        //    var getInfo=storeCustomer.Ge
        //    return returnModel;
        //}

        public void SendHTMLMail([FromForm] MailRequestWithHTML request)
        {
            try
            {
                mailService.SendHTMLEmailAsync(request);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("Register")]
        public ApiResponseModel Register(RegisterViewModel model)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var pwd = model.Password.ToStringNormally();
                    model.Role = "Customer";
                    var storeCustomer = Startup.IocContainer.Resolve<IStoreCustomer>();
                    var isEmailExist = storeCustomer.GetByEmail(model.Email);

                    if (isEmailExist != null)
                    {
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Message = "Email already exists.";
                        return returnModel;
                    }

                    pwd = Utility.Md5HashingData(pwd);
                    model.Password = pwd;

                    var hashData = GenereateHashingForNewAccount(model.Email, model.Email);

                    var customer = storeCustomer.Insert(new IdentityCustomer
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PasswordHash = pwd,
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber,
                        Roles = new List<string> { model.Role },
                        SecurityStamp = hashData
                    });

                    if (customer != null)
                    {
                        var getInfo = storeCustomer.GetByEmail(model.Email);
                        returnModel.Data = new CustomerDetailModel
                        {
                            UserName = model.Email,
                            Id = getInfo.StaffId.ToString(),
                            Email = getInfo.Email,
                            PhoneNumber = getInfo.PhoneNumber,
                            FullName = model.FullName,
                            CreatedDateUtc = DateTime.Now,
                        };

                        returnModel.Token = Utility.GenerateAuthCustomerJwtToken(getInfo, "ogatore");
                        var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                        var mailHtml = new MailRequestWithHTML()
                        {
                            ToEmail = getInfo.Email,
                            UserName = getInfo.FullName,
                            Link = baseUrl + "/Email/Confirmation?token=",
                            IdUser = getInfo.SecurityStamp,
                        };

                        SendHTMLMail(mailHtml);
                        returnModel.Message = string.Format("Register sucessfully. An email just sent to your address.Please check and verify your email.");
                    }
                    else
                    {
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Message = "Sign in does not successfully.";
                        return returnModel;
                    }
                }
            }
            catch (Exception ex)
            {
                returnModel.Message = "Could not login: " + returnModel.Message;
                _logger.LogDebug("Could not login: " + ex.ToString());
            }

            return returnModel;

        }

        [HttpPost("ConfirmEmail")]
        public ActionResult ConfirmEmail(string token)
        {
            if (token != null)
            {
                return RedirectToAction("ConfirmEmail", "Email", new { token = token });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet("ResendEmail")]
        public ApiResponseModel ResendEmail(string email)
        {
            var returnModel = new ApiResponseModel();
            var storeCustomer = Startup.IocContainer.Resolve<IStoreCustomer>();
            var info = storeCustomer.GetByEmail(email);
            if (info != null)
            {
                if (info.SecurityStamp != null)
                {
                    var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                    returnModel.Code = EnumCommonCode.Success;
                    returnModel.Data = baseUrl + "/Email/Confirmation?token=" + info.SecurityStamp;
                }
            }
            return returnModel;
        }
        [HttpGet("{id}")]
        public ApiResponseModel GetById(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                _logger.LogError("Test1");
                var info = HelperCustomer.GetBaseInfo(id);
                if (info != null)
                {
                    _logger.LogError("Test2");
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
                returnModel.Data = HelperCustomer.GetList();
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
        public ApiResponseModel Update(CustomerUpdateModel model)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                if (model.ReceiveAllUpdate == null) model.ReceiveAllUpdate = false;

                var store = Startup.IocContainer.Resolve<IStoreCustomer>();
                var identity = store.GetByStaffId(model.Id).MappingObject<IdentityCustomer>();
                identity.PhoneNumber = model.PhoneNumber;
                identity.FullName = model.FullName;
                identity.ReceiveAllUpdate = (bool)model.ReceiveAllUpdate;
                identity.RemoveAds = (bool)model.RemoveAds;
                identity.Avatar = model.CoverImage;
                //Update DB
                var returnId = store.Update(identity);

                returnModel.Data = returnId;

                //Clear cache
                HelperCustomer.ClearCache(model.Id);

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
                var info = HelperCustomer.GetBaseInfo(id);
                if (info != null)
                {
                    var store = Startup.IocContainer.Resolve<IStoreCustomer>();

                    store.Delete(info.StaffId);

                    //Clear cache
                    HelperCustomer.ClearCache(info.StaffId);
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
