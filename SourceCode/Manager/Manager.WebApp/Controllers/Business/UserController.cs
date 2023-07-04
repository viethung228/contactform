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

namespace Manager.WebApp.Controllers
{
    public class UserController : BaseAuthedController
    {
        private readonly IStoreCustomer _mainStore;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _mainStore = Startup.IocContainer.Resolve<IStoreCustomer>();
            _logger = logger;
        }

        public ActionResult Index(CustomerViewModel model)
        {
            model = GetDefaultFilterModel(model);
            try
            {

                var isLocked = Convert.ToBoolean(model.IsLocked);
                var filter = new IdentityCustomer
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
                    foreach (var item in model.SearchResult)
                    {
                        item.Point = HelperCustomer.GetPoint(item.Id) == null ? 0 : HelperCustomer.GetPoint(item.Id).point;
                    }
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
            var model = new CustomerDetailModel();
            try
            {

                var apiRes = CustomerServices.GetDetailAsync(new CustomerUpdateModel { Id = id }).Result;

                model = apiRes.ConvertData<CustomerDetailModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }
            LoadNeededInformation();

            return PartialView("Edit", model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult Edit_Submit(CustomerUpdateModel model)
        {
            var strError = string.Empty;

            try
            {
                if (model.image_file_upload != null)
                {
                    model.CoverImage = UploadImage(model);
                };
                var info = HelperCustomer.GetBaseInfo(model.Id);
                if (info != null)
                {
                    if (info.Avatar != null && model.CoverImage == null)
                    {
                        model.CoverImage = info.Avatar;
                    }
                    var apiRes = CustomerServices.UpdateAsync(model).Result;
                }


                this.AddNotification(ManagerResource.LB_INSERT_SUCCESS, NotificationType.SUCCESS);

                return Json(new { success = true, message = ManagerResource.LB_UPDATE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                this.AddNotification(ManagerResource.LB_ERROR_OCCURED, NotificationType.ERROR);
            }
            LoadNeededInformation();

            return Json(new { success = false, message = strError });
        }

        /*[RequestParamsValidation]*/
        public ActionResult Detail(int id, int currentpage = 1, int pagesize = 10)
        {
            var model = new CustomerDetailModel();

            try
            {
                var info = HelperCustomer.GetBaseInfo(id);
                if (info != null)
                {
                    model = info.MappingObject<CustomerDetailModel>();
                    model.CreatedDateUtc = model.CreatedDateUtc.Date;
                    var _gethistory = HelperCustomer.GetHistoryPoint(info.Id, currentpage, pagesize);
                    if (_gethistory != null && _gethistory.Count > 0)
                    {
                        int totalCount = _gethistory.FirstOrDefault().TotalCount;
                        int numberOfPage = totalCount % pagesize == 0 ? totalCount / pagesize : totalCount / pagesize + 1;
                        ViewBag.Point = HelperCustomer.GetPoint(info.Id).point;
                        ViewBag.TotalCount = totalCount;
                        ViewBag.NumberOfPage = numberOfPage;
                        ViewBag.CoinHistoryList = _gethistory;
                        ViewBag.CurrentPage = currentpage;
                        ViewBag.PageSize = pagesize;
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Submit(int id)
        {
            var strError = string.Empty;
            try
            {
                var apiRes = CustomerServices.DeleteAsync(new CustomerUpdateModel { Id = id }).Result;
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
            var categories = HelperCustomer.GetList();
            ViewBag.Categories = categories;
        }

        private string UploadImage(CustomerUpdateModel model)
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
