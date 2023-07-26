using System;
using System.Linq;
using Manager.WebApp.Helpers;
using Manager.WebApp.Resources;
using Manager.SharedLibs;
using Microsoft.Extensions.Logging;
using Manager.WebApp.Models;
using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Manager.DataLayer.Stores;
using Manager.DataLayer;
using Autofac;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MainApi.DataLayer;
using QRCoder;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Reflection;

namespace Manager.WebApp.Controllers
{
    public class UsersAdminController : BaseAuthedController
    {
        private readonly IStoreUser _mainStore;
        private readonly IStoreRole _roleStore;
        private readonly IStoreActivity _activityStore;
        private readonly ILogger<UsersAdminController> _logger;

        public UsersAdminController(ILogger<UsersAdminController> logger)
        {
            _mainStore = Startup.IocContainer.Resolve<IStoreUser>();
            _roleStore = Startup.IocContainer.Resolve<IStoreRole>();
            _activityStore = Startup.IocContainer.Resolve<IStoreActivity>();
            _logger = logger;

            //Clear cache
            CachingHelpers.ClearUserCache();
            CommonHelpers.ClearCookie(CommonHelpers.GetVersionToken());
        }

        // GET: /UsersAdmin
        [AccessRoleChecker]
        public ActionResult Index(UserViewModel model)
        {
            model = GetDefaultFilterModel(model);
            try
            {
                var agencyId = GetCurrentAgencyId();                

                var roles = _roleStore.GetListByAgencyId(agencyId);

                model.AllRoles = roles;
                if (roles.HasData())
                {
                    model.RoleList = roles.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id
                    });
                }

                var isLocked = Convert.ToBoolean(model.IsLocked);
                var filter = new IdentityUser { 
                    Keyword = model.Keyword,
                    ParentId = agencyId,
                    RoleId = model.RoleId,
                    LockoutEnabled = isLocked
                };


                model.SearchResult = _mainStore.GetByPage(filter, model.Page, model.PageSize);

                model.PageNo = (int)(model.Total / model.PageSize);

                if (model.SearchResult.HasData())
                {
                    foreach (var record in model.SearchResult)
                    {
                        var _userRoles = _roleStore.GetRolesByUserId(record.Id);
                        if (_userRoles.HasData())
                        {
                            record.Roles = _userRoles.Select(x => x.Name).ToList();
                        }
                    }

                    model.TotalCount = model.SearchResult[0].TotalCount;
                    model.Page = model.Page;
                    model.PageSize = model.PageSize;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not display UserAdmin page because: {0}", ex.ToString());
            }

            return View(model);
        }

        // GET: /Users/Details/5
        [AccessRoleChecker]
        public async Task<ActionResult> Details(UserDetailsViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Id))
            {
                return new NotFoundResult();
            }
            var detailsModel = new UserDetailsViewModel();
            try
            {
                var user = _mainStore.GetById(model.Id);

                await Task.FromResult(user);
                if (user == null)
                {
                    return RedirectToErrorPage();
                }

                
                var userRoles = _roleStore.GetRolesByUserId(user.Id);
                ViewBag.RoleNames = userRoles.HasData() ? userRoles.Select(x => x.Name).ToList() : null;

                detailsModel.User = user;
                detailsModel.Lockout = new LockoutViewModel();
                var isLocked = user.LockoutEnabled;

                detailsModel.Lockout.Status = isLocked ? LockoutStatus.Locked : LockoutStatus.Unlocked;
                if (detailsModel.Lockout.Status == LockoutStatus.Locked)
                {
                    detailsModel.Lockout.LockoutEndDate = user.LockoutEndDateUtc;
                }

                detailsModel.FullName = user.FullName;
            }
            catch
            {
                return RedirectToErrorPage();
            }

            return View(detailsModel);
        }

        //
        // GET: /Users/Create
        [AccessRoleChecker(AgencyRequired = true)]
        public ActionResult Create()
        {
            var model = new RegisterViewModel();            
            try
            {
                model.StaffId = GetCurrentStaffId();                

                var agencyId = GetCurrentAgencyId();
                var roles = _roleStore.GetListByAgencyId(agencyId);

                model.RolesList = roles.Select(x => new System.Web.Mvc.SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id
                });

                model.IsActived = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not display Create page because: {0}", ex.ToString());
            }

            return View(model);
        }


        //
        // POST: /Users/Create
        [HttpPost]
        [AccessRoleChecker]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRole)
        {
            try
            {
                userViewModel.Password = Utility.Md5HashingData(userViewModel.Password);
                int parentId = GetCurrentAgencyId();
                if (parentId == 0)
                {
                    GetCurrentStaffId();
                }

                var user = new IdentityUser
                {
                    UserName = userViewModel.UserName,
                    FullName = userViewModel.FullName,
                    EmailConfirmed = true,
                    ParentId = parentId,
                    PasswordHash = userViewModel.Password,
                    ReceiveAllUpdate = userViewModel.ReceiveAllUpdate
                };                

                var agencyId = GetCurrentAgencyId();
                var roles = _roleStore.GetListByAgencyId(agencyId);
                userViewModel.RolesList = roles.Select(x => new System.Web.Mvc.SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Name
                });

                if (!string.IsNullOrEmpty(user.UserName))
                {
                    var existedUser = _mainStore.GetByUserName(user.UserName);
                    if (existedUser != null)
                    {
                        if (existedUser != null)
                        {
                            this.AddNotification(ManagerResource.ERROR_ACCOUNT_DUPLICATED, NotificationType.ERROR);

                            return View(userViewModel);
                        }
                    }
                }
                user.Email = user.UserName;
                user.SecurityStamp = GenereateHashingForNewAccount(user.UserName,user.Email);
                var newId = _mainStore.Insert(user);
                await Task.FromResult(newId);

                var newUserInfo = _mainStore.GetById(newId);
                if(newUserInfo != null)
                {
                    if (selectedRole.HasData())
                    {
                        foreach (var item in selectedRole)
                        {
                            //Add to roles
                            _roleStore.AddUserToRole(newUserInfo.Id, item);
                        }
                    }

                    var imageName = EpochTime.GetIntDate(DateTime.Now) + ".png";
                    var dirSeparator = System.IO.Path.DirectorySeparatorChar;

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + dirSeparator + "Media" + dirSeparator + "QRcode");
                    // Check if the directory we are saving to exists
                    if (!Directory.Exists(folderPath))
                    {
                        // If it doesn't exist, create the directory
                        Directory.CreateDirectory(folderPath);
                    }

                    //var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + dirSeparator + "Media" + dirSeparator + "QRcode" + dirSeparator + imageName);
                    //var qrURL = string.Format("{0}?staffid={1}", AppConfiguration.GetAppsetting("Line:LiffUrl"), newUserInfo.StaffId);

                    ////Create LINE QR Code
                    //QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    //QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrURL, QRCodeGenerator.ECCLevel.Q);
                    //QRCode qrCode = new QRCode(qrCodeData);
                    //Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    //qrCodeImage.Save(imagePath);

                    //var absolutePath = "/Media/QRcode/" + imageName;

                    //_mainStore.UpdateQrcode(newUserInfo.StaffId, absolutePath);
                    CommonHelpers.ClearUserCache(newUserInfo);
                }

                //Clear cache
                CachingHelpers.ClearUserCache();

                CommonHelpers.ClearCookie(CommonHelpers.GetVersionToken());

                this.AddNotification(ManagerResource.LB_INSERT_SUCCESS, NotificationType.SUCCESS);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not Create because: {0}", ex.ToString());
            }

            return View(userViewModel);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public ActionResult GenerateQRCode(int staffId)
        {
            try
            {
                var qrURL = string.Format("{0}?staffid={1}", AppConfiguration.GetAppsetting("Line:LiffUrl"), staffId);

                //Create LINE QR Code
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrURL, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                //qrCodeImage.Save(imagePath);

                using (var stream = new MemoryStream())
                {
                    qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    var imgByteArr = stream.ToArray();

                    return File(imgByteArr, "image/jpeg");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return null;
        }

        public async Task<ActionResult> Lock(string id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }

            try
            {
                var user = _mainStore.GetById(id);
                if (user == null)
                {
                    return new NotFoundResult();
                }

                var result = _mainStore.LockAccount(new IdentityUser { Id = id });

                await Task.FromResult(result);

                //Clear cache
                CachingHelpers.ClearUserCache();
                CachingHelpers.ClearUserCache(id);

                CommonHelpers.ClearCookie(CommonHelpers.GetVersionToken());

                return RedirectToAction("Details", "UsersAdmin", new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not LockAccount because: {0}", ex.ToString());
            }

            return RedirectToAction("Details", "UsersAdmin", new { Id = id });
        }

        public async Task<ActionResult> Unlock(string id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }

            try
            {
                var user = _mainStore.GetById(id);
                if (user == null)
                {
                    return new NotFoundResult();
                }

                var result = _mainStore.UnLockAccount(new IdentityUser { Id = id });

                await Task.FromResult(result);

                //Clear cache
                CachingHelpers.ClearUserCache();
                CachingHelpers.ClearUserCache(id);

                CommonHelpers.ClearCookie(CommonHelpers.GetVersionToken());
                return RedirectToAction("Details", "UsersAdmin", new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not UnLockAccount because: {0}", ex.ToString());
            }

            return RedirectToAction("Details", "UsersAdmin", new { Id = id });
        }

        //
        // GET: /Users/Edit/1
        [AccessRoleChecker]
        public async Task<ActionResult> Edit(string id)
        {
            var model = new EditUserViewModel();

            try
            {
                var agencyId = GetCurrentAgencyId();
                if (string.IsNullOrEmpty(id))
                {
                    return new NotFoundResult();
                }

                var allRoles = _roleStore.GetListByAgencyId(agencyId);

                var user = _mainStore.GetById(id);

                await Task.FromResult(user);
                if (user == null)
                {
                    return RedirectToErrorPage();
                }

                model.Lockout = new LockoutViewModel();
                var isLocked = user.LockoutEnabled;
                model.Lockout.Status = isLocked ? LockoutStatus.Locked : LockoutStatus.Unlocked;
                model.IsActived = !isLocked;
                if (model.Lockout.Status == LockoutStatus.Locked)
                {
                    model.Lockout.LockoutEndDate = user.LockoutEndDateUtc;
                }

                model.Id = user.Id;
                model.UserName = user.UserName;
                model.FullName = user.FullName;
                model.ReceiveAllUpdate = user.ReceiveAllUpdate;

                var userRoles = _roleStore.GetRolesByUserId(user.Id);
                var hasRoles = userRoles.HasData();

                model.RolesList = new List<SelectListItem>();
                if (allRoles.HasData())
                {
                    foreach (var r in allRoles)
                    {
                        var item = new SelectListItem();
                        item.Text = r.Name;
                        item.Value = r.Id;
                        if (hasRoles)
                        {
                            item.Selected = userRoles.Exists(x => x.Id == r.Id);
                            if (item.Selected)
                                model.Role = r.Id;
                        }

                        model.RolesList.Add(item);
                    }
                }

                model.SEmail = HttpContext.Request.Query["Email"];
                model.SRoleId = HttpContext.Request.Query["RoleId"];
                model.SearchExec = HttpContext.Request.Query["SearchExec"];
                model.Page = HttpContext.Request.Query["Page"];
                model.SIsLocked = Convert.ToInt32(HttpContext.Request.Query["IsLocked"]);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not display EditUser page because: {0}", ex.ToString());

                return RedirectToErrorPage();
            }
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessRoleChecker]
        public async Task<ActionResult> Edit(EditUserViewModel editUser, params string[] selectedRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _mainStore.GetById(editUser.Id);
                    if (user == null)
                    {
                        return new NotFoundResult();
                    }

                    user.UserName = editUser.UserName;
                    user.FullName = editUser.FullName;
                    user.ReceiveAllUpdate = editUser.ReceiveAllUpdate;

                    user.StaffId = GetCurrentStaffId();
                    user.ParentId = GetCurrentAgencyId();
                    if (user.ParentId == 0) user.ParentId = GetCurrentStaffId();

                    if (!string.IsNullOrEmpty(user.UserName))
                    {
                        var existedUser = _mainStore.GetByUserName(user.UserName);
                        if (existedUser != null)
                        {
                            if (editUser.Id != existedUser.Id)
                            {
                                this.AddNotification(ManagerResource.ERROR_ACCOUNT_DUPLICATED, NotificationType.ERROR);

                                return RedirectToAction("Edit", "UsersAdmin", new { Id = editUser.Id });
                            }
                        }
                    }
                    if (!editUser.IsActived && !user.LockoutEnabled)
                    {
                        user.LockoutEndDateUtc = DateTime.UtcNow;
                        user.LockoutEnabled = true;
                    }
                    if (editUser.IsActived && user.LockoutEnabled)
                    {
                        user.LockoutEndDateUtc = DateTime.UtcNow;
                        user.LockoutEnabled = false;
                    }

                    user.Status = editUser.IsActived ? 1 : 0;
                    user.Roles = selectedRole.HasData() ? selectedRole.ToList() : null;

                    //Update info
                    var result = _mainStore.Update(user);

                    await Task.FromResult(result);

                    //Clear cache
                    //CachingHelpers.ClearUserCache();
                    MenuHelper.ClearUserMenuCache(editUser.Id);
                    CachingHelpers.ClearUserCache(editUser.Id);
                    PermissionHelper.ClearPermissionsCache(editUser.Id);
                    CommonHelpers.ClearCookie(CommonHelpers.GetVersionToken());

                    this.AddNotification(ManagerResource.LB_UPDATE_SUCCESS, NotificationType.SUCCESS);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Could not Update User info because: {0}", ex.ToString()));

                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                return RedirectToAction("Edit", "UsersAdmin", new { Id = editUser.Id });
            }

            return RedirectToAction("Edit", "UsersAdmin", new { Id = editUser.Id });
        }

        //Show popup confirm delete        
        public ActionResult DeleteUser(string id)
        {
            if (id == null)
            {

                return new NotFoundResult();
            }

            UserViewModel record = new UserViewModel();
            record.UserInfoViewModel = new IdentityUser();
            record.UserInfoViewModel.Id = id;

            return PartialView("_DeleteUserInfo", record);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        [AccessRoleChecker]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    return Json(new { success = false, message = ManagerResource.LB_ERROR_OCCURED });
                }

                var user = _mainStore.GetById(id);

                await Task.FromResult(user);
                if (user == null)
                {
                    return Json(new { success = false, message = ManagerResource.LB_ERROR_OCCURED });
                }

                var result = _mainStore.Delete(user.StaffId);

                //Clear cache
                CachingHelpers.ClearUserCache();
                CachingHelpers.ClearUserCache(id);

                CommonHelpers.ClearCookie(CommonHelpers.GetVersionToken());

                //return Json(new { success = true });
                return Json(new { success = true, message = ManagerResource.LB_DELETE_SUCCESS, clientcallback = "location.reload();" });
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed when delete user because: {0}", ex.ToString()));
            }

            return Json(new { success = false, message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT });
        }

        //Show popup confirm reset password        
        public ActionResult ResetPassword(string id)
        {
            if (id == null)
            {

                return new NotFoundResult();
            }

            UserViewModel record = new UserViewModel();
            record.UserInfoViewModel = new IdentityUser();
            record.UserInfoViewModel.Id = id;

            return PartialView("_ConfirmResetPwd", record);
        }

        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        [AccessRoleChecker]
        public async Task<ActionResult> ResetPwd(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = ManagerResource.LB_ERROR_OCCURED });
            }

            try
            {
                var user = _mainStore.GetById(id);

                await Task.FromResult(user);
                if (user == null)
                {
                    return Json(new { success = false, message = ManagerResource.LB_ERROR_OCCURED });
                }

                var defaultPassword = AppConfiguration.GetAppsetting("SystemSetting:UserDefaultPassword");

                user.PasswordHash = Utility.Md5HashingData(defaultPassword);

                _mainStore.ChangePassword(user);
                return Json(new { success = true, message = ManagerResource.LB_PASSWORD_RESET_SUCCESS + ": " + defaultPassword });
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Reset password failed, reason: {0}", ex.Message));
            }

            return Json(new { success = false, message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT });
        }
    }
}