using Autofac;
using MainApi.DataLayer.Entities;
using Manager.DataLayer.Stores;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models;
using Manager.WebApp.Resources;
using Manager.WebApp.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Manager.WebApp.Controllers
{
    public class EmailServerController : BaseAuthedController
    {
        private readonly ILogger<EmailServerController> _logger;

        public EmailServerController(ILogger<EmailServerController> logger)
        {
            _logger = logger;
        }

        [AccessRoleChecker]
        public ActionResult Index(ManageEmailServerModel model)
        {
            int currentPage = 1;
            int pageSize = SystemSettings.DefaultPageSize;

            if (string.IsNullOrEmpty(model.SearchExec))
            {
                model.SearchExec = "Y";
                if (!ModelState.IsValid)
                {
                    ModelState.Clear();
                }
            }

            if (!string.IsNullOrEmpty(Request.Query["Page"]))
            {
                currentPage = Utils.ConvertToInt32(Request.Query["Page"], 1);
            }

            var filter = new IdentityEmailServer
            {
                AgencyId = GetCurrentAgencyId(),
                Keyword = !string.IsNullOrEmpty(model.Keyword) ? model.Keyword.Trim() : null,
                Status = model.Status == null ? -1 : (int)model.Status,
                PageIndex = currentPage,
                PageSize = SystemSettings.DefaultPageSize
            };

            try
            {
                var store = Startup.IocContainer.Resolve<IStoreEmailServer>();

                model.SearchResults = store.GetByPage(filter);
                if (model.SearchResults.HasData())
                {
                    model.TotalCount = model.SearchResults[0].TotalCount;
                    model.Page = currentPage;
                    model.PageSize = pageSize;

                    foreach (var item in model.SearchResults)
                    {
                        item.SendingConfig = JsonConvert.DeserializeObject<IdentityEmailServerSMTPConfig>(item.SMTPConfig);
                        item.ReceivingConfig = JsonConvert.DeserializeObject<IdentityEmailServerPOPConfig>(item.POPConfig);
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT, NotificationType.ERROR);

                _logger.LogError("Failed to get list EmailServers because: " + ex.ToString());

                return View(model);
            }

            return View(model);
        }

        [AccessRoleChecker]
        public ActionResult Create()
        {
            var createModel = new EmailServerUpdateModel();
            return View(createModel);
        }

        [HttpPost]
        //[AccessRoleChecker]
        public ActionResult Create(EmailServerUpdateModel model)
        {
            var newId = 0;
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
                var store = Startup.IocContainer.Resolve<IStoreEmailServer>();

                //Extract info
                var info = ExtractEditFormData(model);

                newId = store.Insert(info);

                var agencyId = GetCurrentAgencyId();

                //Clear cache
                CachingHelpers.ClearCacheByKey(string.Format(EnumFormatInfoCacheKeys.EmailServers, agencyId));

                if (newId > 0)
                {
                    this.AddNotification(ManagerResource.LB_INSERT_SUCCESS, NotificationType.SUCCESS);
                }
                else
                {
                    this.AddNotification(ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT, NotificationType.ERROR);
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT, NotificationType.ERROR);

                _logger.LogError("Failed for Create EmailServer request: " + ex.ToString());

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [AccessRoleChecker]
        [RequestParamsValidation]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new NotFoundResult();
            }

            try
            {
                var store = Startup.IocContainer.Resolve<IStoreEmailServer>();

                //Begin db transaction
                var info = store.GetById(id);

                if (info == null)
                    return RedirectToErrorPage();

                //Render to view model
                var editModel = RenderEditModel(info);

                return View(editModel);
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT, NotificationType.ERROR);

                _logger.LogError("Failed for Edit EmailServer request: " + ex.ToString());
            }

            return View(new EmailServerUpdateModel());
        }

        [HttpPost]
        public ActionResult Edit(EmailServerUpdateModel model)
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
                var store = Startup.IocContainer.Resolve<IStoreEmailServer>();

                //Extract data
                var info = ExtractEditFormData(model);

                var isSuccess = store.Update(info);

                var agencyId = GetCurrentAgencyId();

                //Clear cache
                CachingHelpers.ClearCacheByKey(string.Format(EnumFormatInfoCacheKeys.EmailServers, agencyId));

                this.AddNotification(ManagerResource.LB_UPDATE_SUCCESS, NotificationType.SUCCESS);
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT, NotificationType.ERROR);

                _logger.LogError("Failed for Edit EmailServer request: " + ex.ToString());

                return View(model);
            }

            return RedirectToAction("Index");
        }

        //Show popup confirm delete        
        [AccessRoleChecker]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return new NotFoundResult();
            }

            return PartialView("_PopupDelete", id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[AccessRoleChecker]
        public ActionResult Delete_Confirm(int id)
        {
            var strError = string.Empty;
            if (id <= 0)
            {
                return new NotFoundResult();
            }

            try
            {
                var store = Startup.IocContainer.Resolve<IStoreEmailServer>();

                var currentAgencyId = GetCurrentAgencyId();
                store.Delete(new IdentityEmailServer { Id = id, AgencyId = currentAgencyId });

                //Clear cache
                CachingHelpers.ClearCacheByKey(string.Format(EnumFormatInfoCacheKeys.EmailServers, currentAgencyId));
            }
            catch (Exception ex)
            {
                strError = ManagerResource.LB_SYSTEM_BUSY;

                _logger.LogError("Failed to get Delete EmailServer because: " + ex.ToString());

                return Json(new { success = false, message = strError });
            }

            return Json(new { success = true, message = ManagerResource.LB_DELETE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
        }

        #region Helpers

        private IdentityEmailServer ExtractEditFormData(EmailServerUpdateModel formData)
        {
            var myIdetity = new IdentityEmailServer();
            myIdetity.Id = formData.Id;

            myIdetity.Name = formData.Name;
            var smtpConfig = new IdentityEmailServerSMTPConfig();
            var popConfig = new IdentityEmailServerPOPConfig();

            if (formData.SendingConfig != null)
            {
                smtpConfig.SMTPServer = formData.SendingConfig.SMTPServer;
                smtpConfig.Port = formData.SendingConfig.Port;
                smtpConfig.SSLRequired = formData.SendingConfig.SSLRequired;
            }

            if (formData.ReceivingConfig != null)
            {
                popConfig.POPServer = formData.ReceivingConfig.POPServer;
                popConfig.Port = formData.ReceivingConfig.Port;
                popConfig.SSLRequired = formData.ReceivingConfig.SSLRequired;
            }

            myIdetity.SMTPConfig = JsonConvert.SerializeObject(smtpConfig);
            myIdetity.POPConfig = JsonConvert.SerializeObject(popConfig);
            myIdetity.TestingSuccessed = formData.TestingSuccessed;

            myIdetity.Status = formData.Status;
            myIdetity.AgencyId = GetCurrentAgencyId();
            myIdetity.StaffId = GetCurrentStaffId();

            return myIdetity;
        }

        private EmailServerUpdateModel RenderEditModel(IdentityEmailServer identity)
        {
            var editModel = new EmailServerUpdateModel();

            editModel.Id = identity.Id;
            editModel.Name = identity.Name;
            editModel.TestingSuccessed = identity.TestingSuccessed;
            var smtpConfig = JsonConvert.DeserializeObject<IdentityEmailServerSMTPConfig>(identity.SMTPConfig);
            var popConfig = JsonConvert.DeserializeObject<IdentityEmailServerPOPConfig>(identity.POPConfig);

            if (smtpConfig != null)
            {
                editModel.SendingConfig = new EmailServerSMTPModel();
                editModel.SendingConfig.SMTPServer = smtpConfig.SMTPServer;
                editModel.SendingConfig.Port = smtpConfig.Port;
                editModel.SendingConfig.SSLRequired = smtpConfig.SSLRequired;
            }

            if (popConfig != null)
            {
                editModel.ReceivingConfig = new EmailServerPOPModel();
                editModel.ReceivingConfig.POPServer = popConfig.POPServer;
                editModel.ReceivingConfig.Port = popConfig.Port;
                editModel.ReceivingConfig.SSLRequired = popConfig.SSLRequired;
            }

            editModel.Status = identity.Status;

            return editModel;
        }

        #endregion

    }
}
