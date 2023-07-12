using Manager.WebApp.Helpers;
using Manager.WebApp.Models;
using Manager.WebApp.Resources;
using Manager.WebApp.Services;
using Manager.WebApp.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using System;
using Manager.SharedLibs;
using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Manager.DataLayer.Stores;
using Autofac;
using System.Linq;
using Microsoft.CodeAnalysis;
using Manager.SharedLibs.Extensions;

namespace Manager.WebApp.Controllers
{
    public class CompanyController : BaseAuthedController
    {
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ILogger<CompanyController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index(ManageContactFormModel model)
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

                var getData = HelperContactForm.GetAllCompany(model.Keyword, currentPage, pageSize);
                model.SearchResults = getData;
                model.TotalCount = model.SearchResults.HasData() ? model.SearchResults[0].TotalCount : 0;
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            //LoadNeededInformation();
            return View("Index", model);
        }
        public ActionResult ViewEmployeeRelated(string companyName, int currentpage = 1, int pagesize = 10)
        {
            var model = new ManageContactFormModel();

            try
            {
                var info = HelperContactForm.GetEmployeeByCompanyName(companyName, currentpage, pagesize);
                if (info != null)
                {
                    model.SearchResults = info.MappingObject<List<IdentityContactForm>>();
                    ViewBag.CurrentPage = currentpage;
                    ViewBag.PageSize = pagesize;
                }
                else
                {
                    return RedirectToErrorPage();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            //LoadNeededInformation();
            return PartialView("Partials/_ListContactForm", model);
        }        
    }
}
