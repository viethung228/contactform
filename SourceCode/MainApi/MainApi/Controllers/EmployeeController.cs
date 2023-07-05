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
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMailService mailService;
        public EmployeeController(ILogger<EmployeeController> logger, IMailService mailService)
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
                    var storeEmployee = Startup.IocContainer.Resolve<IStoreEmployee>();

                    var employee = storeEmployee.Login(new IdentityEmployee { UserName = model.UserName, PasswordHash = pwd });
                    var idEmployee = "";

                    if (employee != null)
                    {
                        idEmployee = employee.Id;
                        employee.Id = employee.StaffId.ToString();
                        returnModel.Data = employee;
                        if (employee.LockoutEnabled)
                        {
                            returnModel.Code = EnumCommonCode.Error;
                            returnModel.Message = "Your account was locked !!!";
                            return returnModel;
                        }
                        returnModel.Token = Utility.GenerateAuthEmployeeJwtToken(employee, "ogatore");
                    }
                    else
                    {
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Message = "The employeename or password is incorrect";
                        return returnModel;
                    }
                    if (!employee.EmailConfirmed)
                    {
                        var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Data = null;
                        returnModel.Token = null;
                        returnModel.Message = string.Format("Your email {0} did not verified.", employee.Email);
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
        //    var storeEmployee = Startup.IocContainer.Resolve<IStoreEmployee>();
        //    var getInfo=storeEmployee.Ge
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
                    model.Role = "Employee";
                    var storeEmployee = Startup.IocContainer.Resolve<IStoreEmployee>();
                    var isEmailExist = storeEmployee.GetByEmail(model.Email);

                    if (isEmailExist != null)
                    {
                        returnModel.Code = EnumCommonCode.Error;
                        returnModel.Message = "Email already exists.";
                        return returnModel;
                    }

                    pwd = Utility.Md5HashingData(pwd);
                    model.Password = pwd;

                    var hashData = GenereateHashingForNewAccount(model.Email, model.Email);

                    var employee = storeEmployee.Insert(new IdentityEmployee
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PasswordHash = pwd,
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber,
                        Roles = new List<string> { model.Role },
                        SecurityStamp = hashData
                    });

                    if (employee != null)
                    {
                        var getInfo = storeEmployee.GetByEmail(model.Email);
                        returnModel.Data = new EmployeeDetailModel
                        {
                            UserName = model.Email,
                            Id = getInfo.StaffId.ToString(),
                            Email = getInfo.Email,
                            PhoneNumber = getInfo.PhoneNumber,
                            FullName = model.FullName,
                            CreatedDateUtc = DateTime.Now,
                        };

                        returnModel.Token = Utility.GenerateAuthEmployeeJwtToken(getInfo, "ogatore");
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
                        returnModel.Message = "Sign up does not successfully.";
                        return returnModel;
                    }
                }
            }
            catch (Exception ex)
            {
                returnModel.Message = "Could not signup: " + returnModel.Message;
                _logger.LogDebug("Could not signup: " + ex.ToString());
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
            var storeEmployee = Startup.IocContainer.Resolve<IStoreEmployee>();
            var info = storeEmployee.GetByEmail(email);
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
                var info = HelperEmployee.GetBaseInfo(id);
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
                returnModel.Data = HelperEmployee.GetList();
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
        public ApiResponseModel Update(EmployeeUpdateModel model)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                if (model.ReceiveAllUpdate == null) model.ReceiveAllUpdate = false;

                var store = Startup.IocContainer.Resolve<IStoreEmployee>();
                var identity = store.GetByStaffId(model.Id).MappingObject<IdentityEmployee>();
                identity.PhoneNumber = model.PhoneNumber;
                identity.FullName = model.FullName;
                identity.ReceiveAllUpdate = (bool)model.ReceiveAllUpdate;
                identity.Avatar = model.CoverImage;
                //Update DB
                var returnId = store.Update(identity);

                returnModel.Data = returnId;

                //Clear cache
                HelperEmployee.ClearCache(model.Id);

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
                var info = HelperEmployee.GetBaseInfo(id);
                if (info != null)
                {
                    var store = Startup.IocContainer.Resolve<IStoreEmployee>();

                    store.Delete(info.StaffId);

                    //Clear cache
                    HelperEmployee.ClearCache(info.StaffId);
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
