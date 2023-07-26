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
using Microsoft.Extensions.DependencyModel;

namespace Manager.WebApp.Controllers
{
    public class ContactFormController : BaseAuthedController
    {
        private readonly ILogger<ContactFormController> _logger;

        public ContactFormController(ILogger<ContactFormController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index(ManageContactFormModel model)
        {
            int currentPage = 1;
            int pageSize = SystemSettings.DefaultPageSize;
            try
            {
                var companyName = string.IsNullOrEmpty(CommonHelpers.GetCookie("companyName")) ? "" : CommonHelpers.GetCookie("companyName");
                var getInfoCompany = CompanyServices.GetDetailByNameAsync(companyName).Result != null ? CompanyServices.GetDetailByNameAsync(companyName).Result.Data.MappingObject<IdentityCompany>() : new IdentityCompany();
                if (getInfoCompany != null)
                {
                    ViewBag.OwnerId = getInfoCompany.CompanyId;
                    ViewBag.CompanyName = getInfoCompany.CompanyName;
                }

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

                var getData = HelperContactForm.GetContactFormByCompanyName(model.Keyword, companyName, currentPage, pageSize);
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
        public ActionResult AddContactForm(int id, string companyName)
        {
            var model = new ContactFormFullDetailModel();
            try
            {
                var info = HelperCompany.GetBaseInfo(id);
                if (info != null)
                {
                    model.ContactForm.OwnerId = id;
                    model.ContactForm.CompanyName = companyName;
                    //model = info.MappingObject<ContactFormFullDetailModel>();
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
            return View("ContactForm", model);
        }
        public ActionResult EditContactForm(int id)
        {
            var model = new ContactFormFullDetailModel();
            try
            {

                var apiRes = ContactFormServices.GetContactFormByFormIdAsync(id).Result;
                if (apiRes != null && apiRes.Data != null)
                {
                    model.ContactForm = apiRes.ConvertData<IdentityContactForm>();
                    var allowance = ContactFormServices.GetAllowanceByFormIdAsync(model.ContactForm.FormId).Result.ConvertData<IdentityAllowance>();
                    if (allowance != null)
                    {
                        model.Allowance = allowance;
                        var allowance_type = ContactFormServices.GetAllowanceDetailByAllowanceIdAsync(allowance.AllowanceId).Result.ConvertData<IdentityAllowanceDetail>();
                        model.AllowanceDetail = allowance_type;
                    }

                    var dependents = ContactFormServices.GetDependentsByFormIdAsync(model.ContactForm.FormId).Result;
                    if (dependents != null && dependents.Data != null)
                    {
                        foreach (var item in dependents.ConvertData<List<IdentityDependent>>())
                        {
                            model.Dependents.Add(item);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            return View("EditContactForm", model);
        }

        public ActionResult Detail(int id)
        {
            var model = new ContactFormFullDetailModel();
            try
            {

                var apiRes = ContactFormServices.GetContactFormByFormIdAsync(id).Result;
                if (apiRes != null && apiRes.Data != null)
                {
                    model.ContactForm = apiRes.ConvertData<IdentityContactForm>();
                    var allowance = ContactFormServices.GetAllowanceByFormIdAsync(model.ContactForm.FormId).Result.ConvertData<IdentityAllowance>();
                    if (allowance != null)
                    {
                        model.Allowance = allowance;
                        var allowance_type = ContactFormServices.GetAllowanceDetailByAllowanceIdAsync(allowance.AllowanceId).Result.ConvertData<IdentityAllowanceDetail>();
                        model.AllowanceDetail = allowance_type;
                    }

                    var dependents = ContactFormServices.GetDependentsByFormIdAsync(model.ContactForm.FormId).Result;
                    if (dependents != null && dependents.Data != null)
                    {
                        foreach (var item in dependents.ConvertData<List<IdentityDependent>>())
                        {
                            model.Dependents.Add(item);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            return View("Detail", model);
        }

        [HttpPost, ActionName("EditSubmitContactForm")]
        public ActionResult ContactForm_EditSubmit(ContactFormFullDetailModel model)
        {
            var strError = string.Empty;

            try
            {
                var info = HelperCompany.GetBaseInfo(model.ContactForm.OwnerId);
                if (info != null)
                {
                    model.ContactForm.UpdatedDate = DateTime.Now;
                    var apiResContactForm = ContactFormServices.UpdateContactFormAsync(model).Result.Data.MappingObject<IdentityContactForm>();
                    if (apiResContactForm != null)
                    {
                        model.Allowance.FormId = apiResContactForm.FormId;
                        var apiRes2 = ContactFormServices.UpdateAllowanceAsync(model).Result.Data.MappingObject<IdentityAllowance>();
                        if (apiRes2 != null)
                        {
                            model.AllowanceDetail.AllowanceId = apiRes2.AllowanceId;
                            var apiRes3 = ContactFormServices.UpdateAllowanceDetailAsync(model).Result;
                        }
                        var tempListDependents = new List<IdentityDependent>();
                        for (int i = 0; i < model.Dependents.Count; i++)
                        {
                            if (string.IsNullOrEmpty(model.Dependents[i].FullName) && model.Dependents[i].DependentId == 0)
                            {
                                tempListDependents.Add(model.Dependents[i]);
                                continue;
                            }
                            model.Dependents[i].FormId = apiResContactForm.FormId;
                        }
                        foreach (var item in tempListDependents)
                        {
                            model.Dependents.Remove(item);
                        }
                        var apiRes4 = ContactFormServices.UpdateDependentAsync(model).Result;
                    }
                }
                return RedirectToAction("Index");
                //return Json(new { success = true, message = ManagerResource.LB_UPDATE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                this.AddNotification(ManagerResource.LB_ERROR_OCCURED, NotificationType.ERROR);
            }

            return RedirectToAction("Index");
            //return Json(new { success = false, message = strError });

        }

        [HttpPost, ActionName("SubmitContactForm")]
        public ActionResult ContactForm_Submit(ContactFormFullDetailModel model)
        {
            var strError = string.Empty;

            try
            {
                var info = HelperCompany.GetBaseInfo(model.ContactForm.OwnerId);
                if (info != null)
                {
                    var apiResContactForm = ContactFormServices.UpdateContactFormAsync(model).Result.Data.MappingObject<IdentityContactForm>();
                    if (apiResContactForm != null)
                    {
                        model.Allowance.FormId = apiResContactForm.FormId;
                        var apiRes2 = ContactFormServices.UpdateAllowanceAsync(model).Result.Data.MappingObject<IdentityAllowance>();
                        if (apiRes2 != null)
                        {
                            model.AllowanceDetail.AllowanceId = apiRes2.AllowanceId;
                            var apiRes3 = ContactFormServices.UpdateAllowanceDetailAsync(model).Result;
                        }
                        var tempListDependents = new List<IdentityDependent>();
                        for (int i = 0; i < model.Dependents.Count; i++)
                        {
                            if (string.IsNullOrEmpty(model.Dependents[i].FullName))
                            {
                                tempListDependents.Add(model.Dependents[i]);
                                continue;
                            }
                            model.Dependents[i].FormId = apiResContactForm.FormId;
                        }
                        foreach (var item in tempListDependents)
                        {
                            model.Dependents.Remove(item);
                        }
                        var apiRes4 = ContactFormServices.UpdateDependentAsync(model).Result;
                    }

                }
                this.AddNotification(ManagerResource.LB_INSERT_SUCCESS, NotificationType.SUCCESS);
                return RedirectToAction("Index");
                //return Json(new { success = true, message = ManagerResource.LB_UPDATE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                this.AddNotification(ManagerResource.LB_ERROR_OCCURED, NotificationType.ERROR);
            }

            return RedirectToAction("Index");
            //return Json(new { success = false, message = strError });

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Submit(int id)
        {
            var strError = string.Empty;
            try
            {
                var apiRes = CompanyServices.DeleteAsync(new CompanyUpdateModel { Id = id }).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                return Json(new { success = false, message = strError });
            }

            return Json(new { success = true, message = ManagerResource.LB_DELETE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
        }

        public ActionResult DeleteCF(int id)
        {
            if (id <= 0)
            {
                return new NotFoundResult();
            }

            return PartialView("Partials/_PopupDeleteCF", id);
        }
        [HttpPost, ActionName("DeleteCF")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCF_Submit(int id)
        {
            var strError = string.Empty;
            try
            {
                var apiRes = ContactFormServices.DeleteContactFormAsync(id).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                return Json(new { success = false, message = strError });
            }

            return Json(new { success = true, message = ManagerResource.LB_DELETE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
        }

        public ActionResult ExportCSVByFormId(int formId)
        {
            var model = new ContactFormFullDetailModel();
            var html = string.Empty;
            model = new ContactFormFullDetailModel();
            try
            {
                var apiResContactForm = ContactFormServices.GetContactFormByFormIdAsync(formId).Result.Data.MappingObject<IdentityContactForm>();
                if (apiResContactForm != null)
                {
                    if (apiResContactForm != null)
                    {
                        var apiRes3 = new IdentityAllowanceDetail();
                        var apiRes2 = ContactFormServices.GetAllowanceByFormIdAsync(formId).Result != null ? ContactFormServices.GetAllowanceByFormIdAsync(formId).Result.Data.MappingObject<IdentityAllowance>() : null;
                        if (apiRes2 != null)
                        {
                            apiRes3 = ContactFormServices.GetAllowanceDetailByAllowanceIdAsync(apiRes2.AllowanceId).Result.Data.MappingObject<IdentityAllowanceDetail>();
                        }
                        var apiRes4 = ContactFormServices.GetDependentsByFormIdAsync(formId).Result != null ? ContactFormServices.GetDependentsByFormIdAsync(formId).Result.Data.MappingObject<List<IdentityDependent>>() : null;
                        model = new ContactFormFullDetailModel
                        {
                            ContactForm = apiResContactForm,
                            Allowance = apiRes2,
                            AllowanceDetail = apiRes3,
                            Dependents = apiRes4
                        };
                    }
                }
                var data = new ListContactFormFullDetailModel();
                data.listContactForm = new List<ContactFormFullDetailModel>();
                data.listContactForm.Add(model);
                html = PartialViewAsString("Partials/_ExportCSV", data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
                return Json(new { success = false, html = string.Empty });
            }


            return Json(new { success = true, html = html });
        }
        public ActionResult Pagination(ManageContactFormModel model)
        {
            int currentPage = 1;
            //int pageSize = SystemSettings.DefaultPageSize;
            int pageSize = 3;
            try
            {
                var companyName = string.IsNullOrEmpty(CommonHelpers.GetCookie("companyName")) ? "" : CommonHelpers.GetCookie("companyName");
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

                var getData = HelperContactForm.GetContactFormByCompanyName(model.Keyword, companyName, currentPage, pageSize);
                model.SearchResults = getData;
                model.TotalCount = model.SearchResults.HasData() ? model.SearchResults[0].TotalCount : 0;
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            //LoadNeededInformation();
            var html = PartialViewAsString("Partials/_TableContactForm", model);
            return Json(new { html = html });
        }
        public ActionResult ExportCSVByCompanyName(string companyName)
        {
            var model = new ListContactFormFullDetailModel();
            var html = string.Empty;
            model.listContactForm = new List<ContactFormFullDetailModel>();
            try
            {
                var info = HelperContactForm.GetContactFormByCompanyName(null, companyName);
                if (info != null && info.Count > 0)
                {
                    foreach (var item in info)
                    {
                        var apiResContactForm = ContactFormServices.GetContactFormByFormIdAsync(item.FormId).Result.Data.MappingObject<IdentityContactForm>();
                        if (apiResContactForm != null)
                        {
                            var apiRes3 = new IdentityAllowanceDetail();
                            var apiRes2 = ContactFormServices.GetAllowanceByFormIdAsync(item.FormId).Result != null ? ContactFormServices.GetAllowanceByFormIdAsync(item.FormId).Result.Data.MappingObject<IdentityAllowance>() : null;
                            if (apiRes2 != null)
                            {
                                apiRes3 = ContactFormServices.GetAllowanceDetailByAllowanceIdAsync(apiRes2.AllowanceId).Result != null ? ContactFormServices.GetAllowanceDetailByAllowanceIdAsync(apiRes2.AllowanceId).Result.Data.MappingObject<IdentityAllowanceDetail>() : null;
                            }
                            var apiRes4 = ContactFormServices.GetDependentsByFormIdAsync(item.FormId).Result != null ? ContactFormServices.GetDependentsByFormIdAsync(item.FormId).Result.Data.MappingObject<List<IdentityDependent>>() : null;

                            model.listContactForm.Add(new ContactFormFullDetailModel
                            {
                                ContactForm = apiResContactForm,
                                Allowance = apiRes2,
                                AllowanceDetail = apiRes3,
                                Dependents = apiRes4
                            });
                        }
                    }
                }
                html = PartialViewAsString("Partials/_ExportCSV", model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
                return Json(new { success = false, html = string.Empty });
            }


            return Json(new { success = true, html = html });
        }

        public ActionResult ExportCSVByListFormId(int[] listFormId)
        {
            var model = new ListContactFormFullDetailModel();
            var html = string.Empty;
            model.listContactForm = new List<ContactFormFullDetailModel>();
            try
            {
                if (listFormId.Length > 0)
                {
                    for (int i = 0; i < listFormId.Length; i++)
                    {
                        var info = HelperContactForm.GetContactFormByFormId(listFormId[i]);
                        if (info != null)
                        {
                            var apiResContactForm = ContactFormServices.GetContactFormByFormIdAsync(info.FormId).Result.Data.MappingObject<IdentityContactForm>();
                            if (apiResContactForm != null)
                            {
                                var apiRes3 = new IdentityAllowanceDetail();
                                var apiRes2 = ContactFormServices.GetAllowanceByFormIdAsync(info.FormId).Result != null ? ContactFormServices.GetAllowanceByFormIdAsync(info.FormId).Result.Data.MappingObject<IdentityAllowance>() : null;
                                if (apiRes2 != null)
                                {
                                    apiRes3 = ContactFormServices.GetAllowanceDetailByAllowanceIdAsync(apiRes2.AllowanceId).Result != null ? ContactFormServices.GetAllowanceDetailByAllowanceIdAsync(apiRes2.AllowanceId).Result.Data.MappingObject<IdentityAllowanceDetail>() : null;
                                }
                                var apiRes4 = ContactFormServices.GetDependentsByFormIdAsync(info.FormId).Result != null ? ContactFormServices.GetDependentsByFormIdAsync(info.FormId).Result.Data.MappingObject<List<IdentityDependent>>() : null;

                                model.listContactForm.Add(new ContactFormFullDetailModel
                                {
                                    ContactForm = apiResContactForm,
                                    Allowance = apiRes2,
                                    AllowanceDetail = apiRes3,
                                    Dependents = apiRes4
                                });
                            }
                        }
                    }
                    if (model.listContactForm != null && model.listContactForm.Count > 0)
                    {
                        html = PartialViewAsString("Partials/_ExportCSV", model);
                    }
                    else
                    {
                        return Json(new { success = false, html = string.Empty });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
                return Json(new { success = false, html = string.Empty });
            }


            return Json(new { success = true, html = html });
        }
    }
}
