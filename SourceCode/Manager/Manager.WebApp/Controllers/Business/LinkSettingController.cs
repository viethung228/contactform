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

namespace Manager.WebApp.Controllers
{
    public class LinkSettingController : BaseAuthedController
    {
        private readonly ILogger<LinkSettingController> _logger;

        public LinkSettingController(ILogger<LinkSettingController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index(ManageLinkSettingModel model)
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

                var apiRes = LinkSettingServices.GetByPageAsync(model).Result;

                model.SearchResults = apiRes.ConvertData<List<IdentityLinkSetting>>();
                foreach (var item in model.SearchResults)
                {
                    if (item.Type)
                        item.CoverImage = item.Link;
                }
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
        public ActionResult Create()
        {
            var model = new LinkSettingUpdateModel();

            return PartialView("Create", model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult Create_Submit(LinkSettingUpdateModel model)
        {
            var strError = string.Empty;

            try
            {
                if (model.image_file_upload != null)
                {
                    model.CoverImage = UploadImage(model);
                };
                if (model.Type)
                {
                    model.Link = model.CoverImage;
                }
                var apiRes = LinkSettingServices.InsertAsync(model).Result;

                if (apiRes.Code == 1 && apiRes.Data != null)
                {
                    this.AddNotification(ManagerResource.LB_INSERT_SUCCESS, NotificationType.SUCCESS);

                    return Json(new { success = true, message = ManagerResource.LB_UPDATE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                return Json(new { success = false, message = strError });
            }

            this.AddNotification(ManagerResource.LB_ERROR_OCCURED, NotificationType.ERROR);
            return Json(new { success = false });
        }
        [RequestParamsValidation]
        public ActionResult Edit(int id)
        {
            var model = new LinkSettingUpdateModel();
            try
            {
                model.Id = id;

                var apiRes = LinkSettingServices.GetDetailAsync(id).Result;

                model = apiRes.ConvertData<LinkSettingUpdateModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            //LoadNeededInformation();

            return PartialView("Edit", model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult Edit_Submit(LinkSettingUpdateModel model)
        {
            var strError = string.Empty;

            try
            {
                var apiRes = LinkSettingServices.GetDetailAsync(model.Id).Result;
                var model_old = apiRes.ConvertData<LinkSettingUpdateModel>();
                if (model.image_file_upload != null)
                {
                    model.CoverImage = UploadImage(model);
                    model.Link = model.CoverImage;
                };
                if (!model_old.Type && !CommonHelpers.IsValidUrl(model.Link))
                {
                    return Json(new { success = false, message = ManagerResource.LB_NOT_UPDATED, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
                }
                apiRes = LinkSettingServices.UpdateAsync(model).Result;

                this.AddNotification(ManagerResource.LB_INSERT_SUCCESS, NotificationType.SUCCESS);

                return Json(new { success = true, message = ManagerResource.LB_UPDATE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                this.AddNotification(ManagerResource.LB_ERROR_OCCURED, NotificationType.ERROR);
            }
            //LoadNeededInformation();

            return Json(new { success = false, message = strError });
        }

        [RequestParamsValidation]
        public ActionResult Detail(int id)
        {
            var model = new LinkSettingDetailModel();

            try
            {
                var info = HelperLinkSetting.GetBaseInfo(id);
                if (info != null)
                {
                    model = info.MappingObject<LinkSettingDetailModel>();
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
            return PartialView("Detail", model);
        }

        //public ActionResult Delete(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return new NotFoundResult();
        //    }

        //    return PartialView("Partials/_PopupDelete", id);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete_Submit(int id)
        //{
        //    var strError = string.Empty;
        //    try
        //    {
        //        var apiRes = LinkSettingServices.DeleteAsync(new LinkSettingUpdateModel { Id = id }).Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

        //        return Json(new { success = false, message = strError });
        //    }

        //    return Json(new { success = true, message = ManagerResource.LB_DELETE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
        //}

        #region Helper
        //private void LoadNeededInformation()
        //{
        //    //get helper
        //    var linksettings = HelperLinkSetting.GetList();
        //    ViewBag.LinkSettings = linksettings;
        //}

        private string UploadImage(LinkSettingUpdateModel model)
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

            uploadModel.SubDir = "LinkSetting";
            if (model.Id > 0)
            {
                uploadModel.SubDir = string.Format("LinkSetting/{0}", model.Id);
            }

            uploadModel.InCludeDatePath = false;

            if (uploadModel.Files != null && uploadModel.Files[0] != null)
            {
                var apiResult = CdnServices.UploadLinkSettingMediaFilesAsync(uploadModel).Result;
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
