using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models;
using Manager.WebApp.Resources;
using Manager.WebApp.Services;
using Manager.WebApp.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Manager.WebApp.Controllers
{
    public class NotificationController : BaseAuthedController
    {
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index(ManageNotificationModel model)
        {

            int currentPage = 1;
            int pageSize = SystemSettings.DefaultPageSize;
            try
            {
                if (string.IsNullOrEmpty(model.SearchExec))
                {
                    model.SearchExec = "Y";
                    if (!ModelState.IsValid)
                    {
                        ModelState.Clear();
                    }
                }

                if (model.Page > 0) currentPage = model.Page;
                model.Page = currentPage;
                model.PageSize = pageSize;

                var apiRes = NotificationServices.GetByPageAsync(model).Result;

                model.SearchResults = apiRes.ConvertData<List<NotificationItemModel>>();
                if (model.SearchResults.HasData())
                {
                    if (model.SearchResults[0].NotifInfo != null)
                    {
                        model.TotalCount = model.SearchResults[0].NotifInfo.TotalCount;
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return View(model);
        }        
    }
}
