using Autofac;
using Manager.DataLayer.Stores;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models;
using Manager.WebApp.Resources;
using Manager.WebApp.Services;
using Manager.WebApp.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MsSql.AspNet.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Manager.WebApp.Controllers
{
    public class SettingsController : BaseAuthedController
    {
        //ISettingsService _settingService;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
           // _settingService = settingService;
            _logger = logger;
        }

        //[AccessRoleChecker]
        public ActionResult Index()
        {
            var model = new SettingsViewModel();
            try
            {
                if (!CommonHelpers.CurrentUserIsAdmin())
                {
                    return Redirect("/Settings/Email");
                }

                ModelState.Clear();
                var _settingService = Startup.IocContainer.Resolve<ISettingsService>();

                var settings = new SiteSettings();
                settings.Load(_settingService, false);

                model.CurrentSettingsType = settings.Mail.GetType().Name;
                model.SystemSestings = settings;
                model.EmailServers = CommonHelpers.GetListEmailServers(GetCurrentAgencyId());
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(SettingsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _settingService = Startup.IocContainer.Resolve<ISettingsService>();

                    //if (model.EmailPasswordChanged)
                    //{
                    //    var myEncryptedPwd = Utility.TripleDESEncrypt(model.SystemSestings.Mail.Account, model.SystemSestings.Mail.Password);
                    //    model.SystemSestings.Mail.Password = myEncryptedPwd;
                    //}

                    model.SystemSestings.Save(_settingService);

                    this.AddNotification(ManagerResource.LB_OPERATION_SUCCESS, NotificationType.SUCCESS);
                }
                else
                {
                    this.AddNotificationModelStateErrors(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save Settings because: " + ex.ToString());
            }

            return RedirectToAction("Index");
        }

        public ActionResult Email(ManageEmailSettingsModel model)
        {
            try
            {
                var agencyId = GetCurrentAgencyId();
                var staffId = GetCurrentStaffId();

                model.EmailServers = CommonHelpers.GetListEmailServers(agencyId);
                model.InComing = new ManageEmailSettingItemModel();
                model.OutGoing = new ManageEmailSettingItemModel();

                var listEmails = CommonHelpers.GetListEmailSettings(agencyId, staffId);
                if (listEmails.HasData())
                {
                    foreach (var item in listEmails)
                    {
                        if (item.EmailType == (int)EnumEmailSettingTypes.InComing)
                        {
                            model.InComing.Email = item.Email;
                            model.InComing.EmailPassword = "xxxxxx";
                            model.InComing.EmailServerId = item.EmailServerId;
                            model.InComing.EmailType = (int)EnumEmailSettingTypes.InComing;

                            continue;
                        }

                        if (item.EmailType == (int)EnumEmailSettingTypes.OutGoing)
                        {
                            model.OutGoing.Email = item.Email;
                            model.OutGoing.EmailPassword = "xxxxxx";
                            model.OutGoing.EmailServerId = item.EmailServerId;
                            model.OutGoing.EmailType = (int)EnumEmailSettingTypes.OutGoing;

                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT, NotificationType.ERROR);

                _logger.LogError("Failed to display Email Settings because: " + ex.ToString());

                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("Email")]
        public ActionResult Email_Post(ManageEmailSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage + x.Exception));
                this.AddNotification(messages, NotificationType.ERROR);
                return View(model);
            }

            try
            {
                //Extract data
                var settings = new List<IdentityEmailSetting>();
                var agencyId = GetCurrentAgencyId();
                var staffId = GetCurrentStaffId();

                var outGoing = new IdentityEmailSetting();
                if (model.OutGoing != null)
                {
                    outGoing.Email = model.OutGoing.Email;

                    if (model.OutGoing.PasswordChanged)
                    {
                        //var myEncryptedPwd = Utility.EncryptText(model.OutGoing.Email, model.OutGoing.EmailPassword);
                        var myEncryptedPwd = Utility.EncryptText(model.OutGoing.EmailPassword, SystemSettings.EncryptTokenKey);
                        outGoing.EmailPasswordHash = myEncryptedPwd;
                    }

                    outGoing.PasswordChanged = model.OutGoing.PasswordChanged;

                    outGoing.EmailServerId = model.OutGoing.EmailServerId;
                    outGoing.EmailType = (int)EnumEmailSettingTypes.OutGoing;

                    outGoing.AgencyId = agencyId;
                    outGoing.StaffId = staffId;
                    outGoing.TestingSuccessed = model.OutGoing.TestingSuccessed;

                    settings.Add(outGoing);
                }


                //if (model.InComing != null)
                //{
                //    var inComming = new IdentityEmailSetting();
                //    inComming.Email = model.InComing.Email;

                //    if (model.InComing.PasswordChanged)
                //    {
                //        var myEncryptedPwd = Utility.TripleDESEncrypt(model.InComing.Email, model.InComing.EmailPassword);
                //        inComming.EmailPasswordHash = myEncryptedPwd;
                //    }

                //    inComming.PasswordChanged = model.InComing.PasswordChanged;

                //    inComming.EmailServerId = model.InComing.EmailServerId;
                //    inComming.EmailType = (int)EnumEmailSettingTypes.InComing;
                //    inComming.AgencyId = agencyId;
                //    inComming.StaffId = staffId;
                //    inComming.TestingSuccessed = model.InComing.TestingSuccessed;

                //    settings.Add(inComming);
                //}

                if (model.OutGoing != null)
                {
                    var inComming = new IdentityEmailSetting();
                    inComming.Email = model.OutGoing.Email;

                    if (model.OutGoing.PasswordChanged)
                    {
                        //var myEncryptedPwd = Utility.EncryptText(model.OutGoing.Email, model.OutGoing.EmailPassword);
                        var myEncryptedPwd = Utility.EncryptText(model.OutGoing.EmailPassword, SystemSettings.EncryptTokenKey);
                        inComming.EmailPasswordHash = myEncryptedPwd;
                    }

                    inComming.PasswordChanged = model.OutGoing.PasswordChanged;

                    inComming.EmailServerId = model.OutGoing.EmailServerId;
                    inComming.EmailType = (int)EnumEmailSettingTypes.InComing;
                    inComming.AgencyId = agencyId;
                    inComming.StaffId = staffId;
                    inComming.TestingSuccessed = model.OutGoing.TestingSuccessed;

                    settings.Add(inComming);
                }

                var store = Startup.IocContainer.Resolve<IStoreEmailServer>();

                var isSuccess = store.UpdateEmailSettings(settings);

                //Clear cache
                CachingHelpers.ClearCacheByKey(string.Format(EnumFormatInfoCacheKeys.EmailSettings, agencyId, staffId));

                this.AddNotification(ManagerResource.LB_UPDATE_SUCCESS, NotificationType.SUCCESS);
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT, NotificationType.ERROR);

                _logger.LogError("Failed for update Email Settings: " + ex.ToString());

                //return View(model);
            }

            return RedirectToAction("Email");
        }
    }
}
