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

        public ActionResult Index(CompanyViewModel model)
        {
            model = GetDefaultFilterModel(model);
            try
            {

                var isLocked = Convert.ToBoolean(model.IsLocked);
                var filter = new IdentityCompany
                {
                    Keyword = model.Keyword,
                    RoleId = model.RoleId,
                    LockoutEnabled = isLocked
                };


                model.SearchResult = CompanyServices.GetByPageAsync(new ManageCompanyModel
                {
                    Keyword = filter.Keyword,
                    Page = model.Page,
                    PageSize = model.PageSize,

                }).Result.Data.MappingObject<List<IdentityCompany>>();

                model.PageNo = (int)(model.Total / model.PageSize);

                if (model.SearchResult.HasData())
                {
                    model.TotalCount = model.SearchResult[0].TotalCount;
                    model.Page = model.Page;
                    model.PageSize = model.PageSize;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not display User page because: {0}", ex.ToString());
            }
            return View("Index", model);
        }

        [RequestParamsValidation]
        public ActionResult Edit(int id)
        {
            var model = new CompanyDetailModel();
            try
            {

                var apiRes = CompanyServices.GetDetailAsync(new CompanyUpdateModel { Id = id }).Result;

                model = apiRes.ConvertData<CompanyDetailModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            LoadNeededInformation();

            return PartialView("Edit", model);
        }
        public ActionResult AddCompany()
        {
            var model = new CompanyDetailModel();
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            return PartialView("Edit", model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult Edit_Submit(CompanyUpdateModel model)
        {
            var strError = string.Empty;
            var apiRes = new ApiResponseModel();
            try
            {
                if (model.image_file_upload != null)
                {
                    model.CoverImage = UploadImage(model);
                }
                var getByEmail = CompanyServices.GetDetailByEmailAsync(model.Email).Result;
                var getByCompanyName = CompanyServices.GetDetailByNameAsync(model.CompanyName).Result;
                if (getByEmail != null && getByEmail.Data != null)
                {
                    var info = getByEmail.Data.MappingObject<IdentityCompany>();
                    var infoCompany = new IdentityCompany();
                    if (getByCompanyName != null)
                    {
                        infoCompany = getByCompanyName.Data.MappingObject<IdentityCompany>();
                    }

                    if (info.Avatar != null && model.CoverImage == null)
                    {
                        model.CoverImage = info.Avatar;
                    }

                    if (infoCompany == null && infoCompany.Id == null)
                    {
                        apiRes = CompanyServices.UpdateAsync(model).Result;
                    }
                }
                else
                {
                    if (getByCompanyName.Data == null)
                    {
                        apiRes = CompanyServices.UpdateAsync(model).Result;
                    }
                }

                if (apiRes != null && apiRes.Data != null)
                {
                    this.AddNotification(ManagerResource.LB_INSERT_SUCCESS, NotificationType.SUCCESS);
                    return Json(new { success = true, message = ManagerResource.LB_UPDATE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                this.AddNotification(ManagerResource.LB_ERROR_OCCURED, NotificationType.ERROR);
            }
            return Json(new { success = false, message = strError });
        }

        /*[RequestParamsValidation]*/
        public ActionResult Detail(int id, int currentpage = 1, int pagesize = 10)
        {
            var model = new CompanyDetailModel();

            try
            {
                var info = HelperCompany.GetBaseInfo(id);
                if (info != null)
                {
                    model = info.MappingObject<CompanyDetailModel>();
                    model.CreatedDateUtc = model.CreatedDateUtc.Date;
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
            LoadNeededInformation();
            return PartialView("Detail", model);
        }

        public ActionResult GetAllCompanyNames()
        {
            var model = new ManageCompanyModel();

            try
            {
                var info = HelperCompany.GetList();
                if (info != null && info.Count > 0)
                {
                    model.SearchResults = info;
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
            LoadNeededInformation();
            return PartialView("Partials/_ListCompanyNames", model);
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return new NotFoundResult();
            }

            return PartialView("Partials/_PopupDelete", id);
        }
        public ActionResult ListContactForm(int id)
        {
            var model = new ManageContactFormModel();

            try
            {
                var info = HelperCompany.GetBaseInfo(id);
                if (info != null)
                {
                    var apiRes = ContactFormServices.GetContactFormsByCompanyIdAsync(id).Result;
                    if (apiRes != null)
                    {
                        model.SearchResults = apiRes.Data.MappingObject<List<IdentityContactForm>>();
                    }
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
            LoadNeededInformation();
            return PartialView("ListContactForm", model);
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
            LoadNeededInformation();
            return View("ContactForm", model);
        }
        public ActionResult EditContactForm(int id)
        {
            var model = new ContactFormFullDetailModel();
            try
            {

                var apiRes = ContactFormServices.GetContactFormByFormIdAsync(id).Result;
                if (apiRes != null)
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
            LoadNeededInformation();

            return View("EditContactForm", model);
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

            LoadNeededInformation();
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

            LoadNeededInformation();
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
                        var apiRes2 = ContactFormServices.GetAllowanceByFormIdAsync(formId).Result.Data.MappingObject<IdentityAllowance>();
                        var apiRes3 = ContactFormServices.GetAllowanceDetailByAllowanceIdAsync(apiRes2.AllowanceId).Result.Data.MappingObject<IdentityAllowanceDetail>();
                        var apiRes4 = ContactFormServices.GetDependentsByFormIdAsync(formId).Result.Data.MappingObject<List<IdentityDependent>>();

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
        public ActionResult ViewContactFormRelated(string companyName, int currentpage = 1, int pagesize = 10)
        {
            var model = new ManageContactFormModel();

            try
            {
                var info = HelperContactForm.GetContactFormByCompanyName(null, companyName, currentpage, pagesize);
                var getCompany = CompanyServices.GetDetailByNameAsync(companyName);
                if (getCompany != null)
                {
                    ViewBag.OwnerId = getCompany.Result.Data.MappingObject<IdentityCompany>().CompanyId;
                    ViewBag.CompanyName = getCompany.Result.Data.MappingObject<IdentityCompany>().CompanyName;
                }
                if (info != null)
                {
                    model.SearchResults = info.MappingObject<List<IdentityContactForm>>();
                    model.TotalCount = model.SearchResults.FirstOrDefault().TotalCount;
                    model.Page = model.Page == 0 ? currentpage : model.Page;
                    model.PageSize = model.PageSize == 0 ? pagesize : model.PageSize;
                    ViewBag.CurrentPage = currentpage;
                    ViewBag.PageSize = pagesize;
                }
                else
                {
                    model.SearchResults = new List<IdentityContactForm>();
                    ViewBag.CurrentPage = currentpage;
                    ViewBag.PageSize = pagesize;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            //LoadNeededInformation();
            return PartialView("Partials/_ListContactForm", model);
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
        #region Helper
        private void LoadNeededInformation()
        {
            //get categories
            var categories = HelperCompany.GetList();
            ViewBag.Categories = categories;
        }
        private string UploadImage(CompanyUpdateModel model)
        {
            var fileUrl = string.Empty;

            var uploadModel = new ApiUploadFileModel();

            if (model.image_file_upload != null)
            {
                uploadModel.Files = new List<IFormFile>
                {
                    model.image_file_upload
                };
            }

            uploadModel.SubDir = "Company";
            if (model.Id > 0)
            {
                uploadModel.SubDir = string.Format("Company/{0}", model.Id);
            }

            uploadModel.InCludeDatePath = false;

            if (uploadModel.Files != null && uploadModel.Files[0] != null)
            {
                var apiResult = CdnServices.UploadCategoryMediaFilesAsync(uploadModel).Result;
                if (apiResult != null)
                {
                    var fileRs = apiResult.ConvertData<List<ApiResponseUploadFileModel>>();

                    model.image_file_upload = null;
                    if (fileRs.HasData())
                    {
                        if (fileRs[0] != null)
                        {
                            fileUrl = fileRs[0].Path;
                        }
                    }
                }
            }

            return fileUrl;
        }
        #endregion
    }
}
