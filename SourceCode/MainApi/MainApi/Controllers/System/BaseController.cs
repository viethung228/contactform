using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MainApi.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MainApi.DataLayer;
using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Globalization;
using System.Threading;
using MainApi.Settings;

namespace MainApi.Controllers
{
    public class BaseController : Controller
    {
        //protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        //{
        //    var lang = CommonHelpers.GetCurrentLanguageOrDefault();
        //    var cultureInfo = new CultureInfo(lang);
        //    Thread.CurrentThread.CurrentUICulture = cultureInfo;
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

        //    return base.BeginExecuteCore(callback, state);
        //}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var lang = CommonHelpers.GetCurrentLanguageOrDefault();
            var cultureInfo = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

            //Preparing before executing action
            if (!Request.IsAjaxRequest())
                ViewBag.AdminNavMenu = MenuHelper.GetAdminMenus();

            base.OnActionExecuting(filterContext);
        }

        [NonAction]
        protected string GetModelStateErrors(ModelStateDictionary stateDic)
        {
            var sb = new StringBuilder();
            foreach (var errorKey in stateDic.Keys)
            {
                foreach (var errorMsg in stateDic[errorKey].Errors)
                {
                    sb.AppendLine(errorMsg.ErrorMessage + "<br />");
                }
            }

            return sb.ToString();
        }


        [NonAction]
        public Dictionary<string, List<string>> GetModelStateErrorList(ModelStateDictionary stateDic)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var errorKey in stateDic.Keys)
            {
                var errors = new List<string>();
                foreach (var errorMsg in stateDic[errorKey].Errors)
                {
                    errors.Add(errorMsg.ErrorMessage);
                }

                result.Add(errorKey, errors);
            }

            return result;
        }

        [NonAction]
        public void AddNotificationModelStateErrors(ModelStateDictionary stateDic)
        {
            foreach (var errorKey in stateDic.Keys)
            {
                foreach (var errorMsg in stateDic[errorKey].Errors)
                {
                    this.AddNotification(errorMsg.ErrorMessage, NotificationType.ERROR);
                }
            }
        }

        [NonAction]
        protected IdentityUser GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
                return CommonHelpers.GetCurrentUser();

            return null;
        }

        //protected IdentityUser GetByStaffId(int staffId)
        //{
        //    if (staffId > 0)
        //        return AccountHelper.GetByStaffId(staffId);

        //    return null;
        //}

        [NonAction]
        protected string GetCurrentUserId()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
                return currentUser.Id;

            return string.Empty;
        }

        [NonAction]
        protected int GetCurrentAgencyId()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
            {
                if (currentUser.ParentId == 0)
                    return currentUser.StaffId;
                else
                    return currentUser.ParentId;
            }

            return 0;
        }

        [NonAction]
        protected int GetCurrentStaffId()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
            {
                return currentUser.StaffId;
            }

            return 0;
        }

        protected string GenereateHashingForNewAccount(string userName, string email)
        {
            var recoverData = string.Format("{0}|{1}", userName, email);
            var dataEncrypt = Utility.EncryptText(recoverData, SystemSettings.EncryptKey); // chuỗi bất kỳ vd: 401b09eab3c013d4ca54922bb802bec

            return string.Format("{0}.{1}.{2}", Utility.Md5HashingData(DateTime.Now.ToString(Constant.DATEFORMAT_yyyyMMddHHmmss)), dataEncrypt, Utility.Md5HashingData(userName));
        }
    }
}