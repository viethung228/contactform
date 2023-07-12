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
    public class EmployeeController : BaseAuthedController
    {
        private readonly IStoreEmployee _mainStore;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _mainStore = Startup.IocContainer.Resolve<IStoreEmployee>();
            _logger = logger;
        }

        public ActionResult Index(EmployeeViewModel model)
        {
            model = GetDefaultFilterModel(model);
            try
            {

                var isLocked = Convert.ToBoolean(model.IsLocked);
                var filter = new IdentityEmployee
                {
                    Keyword = model.Keyword,
                    RoleId = model.RoleId,
                    LockoutEnabled = isLocked
                };


                model.SearchResult = _mainStore.GetByPage(filter, model.Page, model.PageSize);

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
            var model = new EmployeeDetailModel();
            try
            {

                var apiRes = EmployeeServices.GetDetailAsync(new EmployeeUpdateModel { Id = id }).Result;

                model = apiRes.ConvertData<EmployeeDetailModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            LoadNeededInformation();

            return PartialView("Edit", model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult Edit_Submit(EmployeeUpdateModel model)
        {
            var strError = string.Empty;

            try
            {
                if (model.image_file_upload != null)
                {
                    model.CoverImage = UploadImage(model);
                };
                var info = HelperEmployee.GetBaseInfo(model.Id);
                if (info != null)
                {
                    if (info.Avatar != null && model.CoverImage == null)
                    {
                        model.CoverImage = info.Avatar;
                    }
                    var apiRes = EmployeeServices.UpdateAsync(model).Result;
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

        /*[RequestParamsValidation]*/
        public ActionResult Detail(int id, int currentpage = 1, int pagesize = 10)
        {
            var model = new EmployeeDetailModel();

            try
            {
                var info = HelperEmployee.GetBaseInfo(id);
                if (info != null)
                {
                    model = info.MappingObject<EmployeeDetailModel>();
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
                var info = HelperEmployee.GetBaseInfo(id);
                if (info != null)
                {
                    var apiRes = ContactFormServices.GetContactFormsByEmployeeIdAsync(id).Result;
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
        public ActionResult AddContactForm(int id)
        {
            var model = new ContactFormFullDetailModel();
            try
            {
                var info = HelperEmployee.GetBaseInfo(id);
                if (info != null)
                {
                    model.ContactForm.OwnerId = id;
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
                var info = HelperEmployee.GetBaseInfo(model.ContactForm.OwnerId);
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
                var info = HelperEmployee.GetBaseInfo(model.ContactForm.OwnerId);
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
                var apiRes = EmployeeServices.DeleteAsync(new EmployeeUpdateModel { Id = id }).Result;
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
        #region Helper
        private void LoadNeededInformation()
        {
            //get categories
            var categories = HelperEmployee.GetList();
            ViewBag.Categories = categories;
        }
        private string UploadImage(EmployeeUpdateModel model)
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

            uploadModel.SubDir = "User";
            if (model.Id > 0)
            {
                uploadModel.SubDir = string.Format("User/{0}", model.Id);
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
