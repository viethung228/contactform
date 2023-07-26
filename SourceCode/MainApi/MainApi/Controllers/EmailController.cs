using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MainApi.Helpers;
using MainApi.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MainApi.DataLayer.Entities;
using Autofac;
using MainApi.DataLayer.Stores;
using MainApi.SharedLibs;
using MainApi.Resources;
using StackExchange.Redis;
using System.Collections.Generic;
using Square.Models;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using Manager.WebApp.Services;
using Polly;
using MimeKit;
using MailKit.Net.Smtp;
using RestSharp;
using System.Text.Json;
using MainApi.Settings;
using MainApi.DataLayer;
using MainApi.SharedLibs.Extensions;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using System.Net;
using System.Web.Mvc;

namespace MainApi.Controllers
{
    public class EmailController : BaseController
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IMailService mailService;
        public EmailController(ILogger<EmailController> logger, IMailService mailService)
        {
            _logger = logger;
            this.mailService = mailService;
        }

        public Microsoft.AspNetCore.Mvc.ActionResult ConfirmEmail(string token)
        {
            try
            {
                if (ModelState.IsValid && token != null)
                {
                    token = Utility.ReplaceWhitespace(token, "+");
                    var array = token.Split('.');
                    var result = "";
                    var description = "";
                    var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                    if (array.Length != 3)
                    {
                        result = "エラーが発生しました メールアドレスが認証されていません";
                        description = "同じエラーが繰り返し発生する場合は、管理者に問い合わせてください";
                    }
                    var check = Utility.DecryptText(array[1], SystemSettings.EncryptKey);
                    var getCompanyName = Utility.DecryptText(array[1], SystemSettings.EncryptKey).Substring(0, Utility.DecryptText(array[1], SystemSettings.EncryptKey).IndexOf('|'));
                    var storeCompany = Startup.IocContainer.Resolve<IStoreCompany>();
                    var getDetail = storeCompany.GetByEmail(getCompanyName);

                    if (getDetail == null || getDetail.SecurityStamp != token || getDetail.EmailConfirmed)
                    {
                        result = "エラーが発生しました メールアドレスが認証されていません";
                        description = "同じエラーが繰り返し発生する場合は、管理者に問い合わせてください";
                    }
                    if (result == "")
                    {
                        var confirm = storeCompany.ConfirmEmail(getDetail.CompanyId);
                        if (confirm)
                        {
                            result = "あなたのメールアドレスは正常に認証されました";
                        }
                        else
                        {
                            result = "エラーが発生しました メールアドレスが認証されていません";
                            description = "同じエラーが繰り返し発生する場合は、管理者に問い合わせてください";
                        }
                    }
                    ViewBag.email = getCompanyName;
                    ViewBag.description = description;
                    ViewBag.baseURL = baseUrl;
                    ViewBag.result = result;
                }
            }
            catch (Exception ex)
            {
                ViewBag.result = "エラーが発生しました メールアドレスが認証されていません";
                ViewBag.description = "同じエラーが繰り返し発生する場合は、管理者に問い合わせてください";
                ViewBag.email = "";
                ViewBag.baseURL = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                _logger.LogDebug("Could not login: " + ex.ToString());
                //throw new Exception(ex.ToString());
            }

            return View();
        }
        public Microsoft.AspNetCore.Mvc.ActionResult Confirmation(string token)
        {
            try
            {
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                var restClient = new RestClient(baseUrl);
                IRestRequest restRequest = new RestRequest("api/account/ConfirmEmail", Method.POST);
                var param = new RestSharp.Parameter("token", token, ParameterType.QueryString);
                restRequest.AddParameter(param);

                restRequest.AddHeader("Content-type", "application/json");

                var result = restClient.ExecuteAsync(restRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                //_logger.LogDebug("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return RedirectToAction("ConfirmEmail", "Email", new { token = token });
        }
    }
}
