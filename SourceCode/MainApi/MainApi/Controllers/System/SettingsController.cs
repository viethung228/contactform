//using Autofac;
//using MainApi.DataLayer.Stores;
//using MainApi.SharedLibs;
//using MainApi.Helpers;
//using MainApi.Models;
//using MainApi.Resources;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using MsSql.AspNet.Identity.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace MainApi.Controllers
//{
//    public class SettingsController : BaseAuthedController
//    {
//        private readonly ILogger<SettingsController> _logger;

//        public SettingsController(ILogger<SettingsController> logger)
//        {
//            _logger = logger;
//        }

//        public ActionResult Email(ManageEmailSettingsModel model)
//        {
//            try
//            {
//                var agencyId = GetCurrentAgencyId();
//                var staffId = GetCurrentStaffId();

//                model.EmailServers = CommonHelpers.GetListEmailServers(agencyId);
//                model.InComing = new ManageEmailSettingItemModel();
//                model.OutGoing = new ManageEmailSettingItemModel();

//                var listEmails = CommonHelpers.GetListEmailSettings(agencyId, staffId);
//                if (listEmails.HasData())
//                {
//                    foreach (var item in listEmails)
//                    {
//                        if (item.EmailType == (int)EnumEmailSettingTypes.InComing)
//                        {
//                            model.InComing.Email = item.Email;
//                            model.InComing.EmailPassword = "xxxxxx";
//                            model.InComing.EmailServerId = item.EmailServerId;
//                            model.InComing.EmailType = (int)EnumEmailSettingTypes.InComing;

//                            continue;
//                        }

//                        if (item.EmailType == (int)EnumEmailSettingTypes.OutGoing)
//                        {
//                            model.OutGoing.Email = item.Email;
//                            model.OutGoing.EmailPassword = "xxxxxx";
//                            model.OutGoing.EmailServerId = item.EmailServerId;
//                            model.OutGoing.EmailType = (int)EnumEmailSettingTypes.OutGoing;

//                            continue;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                this.AddNotification(ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT, NotificationType.ERROR);

//                _logger.LogError("Failed to display Email Settings because: " + ex.ToString());

//                return View(model);
//            }

//            return View(model);
//        }

//        [HttpPost]
//        [ActionName("Email")]
//        public ActionResult Email_Post(ManageEmailSettingsModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                string messages = string.Join("; ", ModelState.Values
//                                       .SelectMany(x => x.Errors)
//                                       .Select(x => x.ErrorMessage + x.Exception));
//                this.AddNotification(messages, NotificationType.ERROR);
//                return View(model);
//            }

//            try
//            {
//                //Extract data
//                var settings = new List<IdentityEmailSetting>();
//                var agencyId = GetCurrentAgencyId();
//                var staffId = GetCurrentStaffId();

//                var outGoing = new IdentityEmailSetting();
//                if (model.OutGoing != null)
//                {
//                    outGoing.Email = model.OutGoing.Email;

//                    if (model.OutGoing.PasswordChanged)
//                    {
//                        var myEncryptedPwd = Utility.TripleDESEncrypt(model.OutGoing.Email, model.OutGoing.EmailPassword);
//                        outGoing.EmailPasswordHash = myEncryptedPwd;
//                    }

//                    outGoing.PasswordChanged = model.OutGoing.PasswordChanged;

//                    outGoing.EmailServerId = model.OutGoing.EmailServerId;
//                    outGoing.EmailType = (int)EnumEmailSettingTypes.OutGoing;

//                    outGoing.AgencyId = agencyId;
//                    outGoing.StaffId = staffId;
//                    outGoing.TestingSuccessed = model.OutGoing.TestingSuccessed;

//                    settings.Add(outGoing);
//                }


//                //if (model.InComing != null)
//                //{
//                //    var inComming = new IdentityEmailSetting();
//                //    inComming.Email = model.InComing.Email;

//                //    if (model.InComing.PasswordChanged)
//                //    {
//                //        var myEncryptedPwd = Utility.TripleDESEncrypt(model.InComing.Email, model.InComing.EmailPassword);
//                //        inComming.EmailPasswordHash = myEncryptedPwd;
//                //    }

//                //    inComming.PasswordChanged = model.InComing.PasswordChanged;

//                //    inComming.EmailServerId = model.InComing.EmailServerId;
//                //    inComming.EmailType = (int)EnumEmailSettingTypes.InComing;
//                //    inComming.AgencyId = agencyId;
//                //    inComming.StaffId = staffId;
//                //    inComming.TestingSuccessed = model.InComing.TestingSuccessed;

//                //    settings.Add(inComming);
//                //}

//                if (model.OutGoing != null)
//                {
//                    var inComming = new IdentityEmailSetting();
//                    inComming.Email = model.OutGoing.Email;

//                    if (model.OutGoing.PasswordChanged)
//                    {
//                        var myEncryptedPwd = Utility.TripleDESEncrypt(model.OutGoing.Email, model.OutGoing.EmailPassword);
//                        inComming.EmailPasswordHash = myEncryptedPwd;
//                    }

//                    inComming.PasswordChanged = model.OutGoing.PasswordChanged;

//                    inComming.EmailServerId = model.OutGoing.EmailServerId;
//                    inComming.EmailType = (int)EnumEmailSettingTypes.InComing;
//                    inComming.AgencyId = agencyId;
//                    inComming.StaffId = staffId;
//                    inComming.TestingSuccessed = model.OutGoing.TestingSuccessed;

//                    settings.Add(inComming);
//                }

//                var store = Startup.IocContainer.Resolve<IStoreEmailServer>();

//                var isSuccess = store.UpdateEmailSettings(settings);

//                //Clear cache
//                CachingHelpers.ClearCacheByKey(string.Format(EnumFormatInfoCacheKeys.EmailSettings, agencyId, staffId));

//                this.AddNotification(ManagerResource.LB_UPDATE_SUCCESS, NotificationType.SUCCESS);
//            }
//            catch (Exception ex)
//            {
//                this.AddNotification(ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT, NotificationType.ERROR);

//                _logger.LogError("Failed for update Email Settings: " + ex.ToString());

//                //return View(model);
//            }

//            return RedirectToAction("Email");
//        }
//    }
//}
