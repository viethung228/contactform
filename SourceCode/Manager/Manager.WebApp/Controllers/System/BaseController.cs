using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Manager.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Threading;
using System.Web;
using Microsoft.AspNetCore.Routing;
using Manager.WebApp.Settings;
using System;

namespace Manager.WebApp.Controllers
{
    public class BaseController : Controller
    {        
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

        protected string GetCurrentUserId()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
                return currentUser.Id;

            return string.Empty;
        }



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

        protected int GetCurrentStaffId()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
            {
                return currentUser.StaffId;
            }

            return 0;
        }

        protected string GetPreviousPageQueryParams()
        {
            var queryParams = string.Empty;
            try
            {
                queryParams = Request.Headers["Referer"].ToString();
            }
            catch
            {
            }

            return queryParams;
        }
        protected string GenereateHashingForNewAccount(string userName, string email)
        {
            var recoverData = string.Format("{0}|{1}", userName, email);
            var dataEncrypt = Utility.EncryptText(recoverData, SystemSettings.EncryptKey); // chuỗi bất kỳ vd: 401b09eab3c013d4ca54922bb802bec

            var rawData = string.Format("{0}.{1}.{2}", Utility.Md5HashingData(DateTime.Now.ToString(Constant.DATEFORMAT_yyyyMMddHHmmss)), dataEncrypt, Utility.Md5HashingData(userName));

            return rawData;
        }
    }
}