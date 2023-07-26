using Autofac;
using MainApi.DataLayer.Entities;
using Manager.DataLayer.Stores;
using Manager.SharedLibs;
using Manager.WebApp.Services;
using Manager.WebApp.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using MsSql.AspNet.Identity.Entities;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Claims;

namespace Manager.WebApp.Helpers
{
    public static class CommonHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(CommonHelpers));

        public static string GetBaseUrl()
        {
            var host = System.Web.HttpContext.Current.Request.Host.Value;
            var scheme = System.Web.HttpContext.Current.Request.Scheme;

            var baseHostUrl = string.Format("{0}://{1}", scheme, host);

            return baseHostUrl;
        }

        public static string GetVersionToken()
        {
            return "JZ-VSTK";
        }
        public static bool IsValidUrl(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
        public static string GetCurrentLanguageOrDefault()
        {
            try
            {
                var myObjStr = GetCookie(SystemSettings.CultureKey);
                if (myObjStr != null && !string.IsNullOrEmpty(myObjStr))
                {
                    if (LanguageMessageHandler.IsSupported(myObjStr))
                    {
                        return myObjStr;
                    }
                    else
                    {
                        return LanguageMessageHandler.GetDefaultLanguage();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetCurrentLanguageOrDefault error because: {0}", ex.ToString()));
            }

            return LanguageMessageHandler.GetDefaultLanguage();
        }
        public static int ImperialYearToNormalYear(string nengou, int imperialYear)
        {
            int westYear = 0;

            DateTime reiwa = new DateTime(2019, 10, 26);
            DateTime heisei = new DateTime(1989, 1, 8);
            DateTime shouwa = new DateTime(1926, 1, 1);
            DateTime taishou = new DateTime(1912, 1, 1);

            switch (nengou)
            {
                case "reiwa":
                    westYear = reiwa.Year + imperialYear - 1;
                    break;
                case "heisei":
                    westYear = heisei.Year + imperialYear - 1;
                    break;
                case "shouwa":
                    westYear = shouwa.Year + imperialYear - 1;
                    break;
                case "taishou":
                    westYear = taishou.Year + imperialYear - 1;
                    break;
            }

            return westYear;
        }
        public static string GetCookie(string key)
        {
            return System.Web.HttpContext.Current.Request.Cookies[key];
        }

        public static void ClearCookie(string cookieName)
        {
            System.Web.HttpContext.Current.Response.Cookies.Delete(cookieName);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void SetCookie(string key, string str, int expireInMinutes = 30)
        {
            //string myObjectJson = JsonConvert.SerializeObject(data);
            //var encryptedStr = Base64Encode(myObjectJson);

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(expireInMinutes);

            System.Web.HttpContext.Current.Response.Cookies.Delete(key);

            System.Web.HttpContext.Current.Response.Cookies.Append(key, str, option);
        }

        public static ClaimsPrincipal GetLoggedInUserClaimPricipal()
        {
            return System.Web.HttpContext.Current.User;
        }

        public static IdentityCompany GetCurrentUser()
        {
            IdentityCompany user = null;

            var principal = GetLoggedInUserClaimPricipal();
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                var userId = principal.GetLoggedInUserId<string>();
                //user = new IdentityUser();

                //user.Id = principal.GetLoggedInUserId<string>();
                //user.UserName = principal.GetLoggedInUserName();
                //user.FullName = principal.GetLoggedInUserDisplayName();

                user = GetCompanyById(userId);
            }

            return user;
        }



        public static T GetLoggedInUserId<T>()
        {
            var principal = GetLoggedInUserClaimPricipal();
            if (principal == null)
                return default(T);

            var loggedInUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(loggedInUserId, typeof(T));
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                return loggedInUserId != null ? (T)Convert.ChangeType(loggedInUserId, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));
            }
            else
            {
                return default(T);
            }
        }

        public static string GetLoggedInUserName()
        {
            var principal = GetLoggedInUserClaimPricipal();
            if (principal == null)
                return string.Empty;

            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static string GetLoggedInUserDisplayName()
        {
            var principal = GetLoggedInUserClaimPricipal();
            if (principal == null)
                return string.Empty;

            return principal.FindFirstValue(ClaimTypes.GivenName);
        }

        public static string GetLoggedInUserEmail()
        {
            var principal = GetLoggedInUserClaimPricipal();
            if (principal == null)
                return string.Empty;

            return principal.FindFirstValue(ClaimTypes.Email);
        }

        public static string MapPath(string path)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), path);
        }

        public static bool CurrentUserIsAdmin()
        {
            try
            {
                var user = GetCurrentUser();
                if (user != null)
                {
                    var currentUserName = user.CompanyName.ToLower();
                    if (user.CompanyName == "admin" || user.CompanyName == "bangvl")
                        return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Check CurrentUserIsAdmin error because: {0}", ex.ToString()));
            }

            return false;
        }

        public static bool CurrentUserIsAgency()
        {
            try
            {
                var user = GetCurrentUser();
                if (user != null)
                {
                    if (user.ParentId == 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Check CurrentUserIsAgency error because: {0}", ex.ToString()));
            }

            return false;
        }

        public static IdentityUser GetUserById(string userId)
        {
            var myKey = string.Format("{0}_{1}", "USER", userId);
            IdentityUser info = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityUser>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreUser>();
                    info = myStore.GetById(userId);

                    //Storage to cache
                    if (info != null)
                        cacheProvider.Set(myKey, info);
                }

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetUserById error because: {0}", ex.ToString()));
            }

            return info;
        }
        public static IdentityCompany GetCompanyById(string userId)
        {
            var myKey = string.Format("{0}_{1}", "COMPANY", userId);
            IdentityCompany info = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityCompany>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreCompany>();
                    info = myStore.GetById(userId);

                    //Storage to cache
                    if (info != null)
                        cacheProvider.Set(myKey, info);
                }

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetUserById error because: {0}", ex.ToString()));
            }

            return info;
        }


        public static IdentityUser GetUserByStaffId(int staffId)
        {
            var myKey = string.Format("{0}_{1}", "USER", staffId);
            IdentityUser info = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityUser>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreUser>();
                    info = myStore.GetByStaffId(staffId);

                    //Storage to cache
                    if (info != null)
                        cacheProvider.Set(myKey, info);
                }

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetUserByStaffId error because: {0}", ex.ToString()));
            }

            return info;
        }

        public static int GetCurrentAgencyId()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
            {
                if (currentUser.ParentId == 0)
                    return currentUser.CompanyId;
                else
                    return currentUser.ParentId;
            }

            return 0;
        }

        public static List<IdentityUser> GetActiveUsers()
        {
            var returnList = new List<IdentityUser>();
            try
            {
                var agencyId = GetCurrentAgencyId();

                var myStore = Startup.IocContainer.Resolve<IStoreUser>();
                var users = myStore.GetActiveUsersByParent(agencyId);
                if (users.HasData())
                {
                    foreach (var u in users)
                    {
                        var userInfo = GetUserById(u.Id);
                        if (userInfo != null)
                        {
                            returnList.Add(userInfo);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetActiveUsers error because: {0}", ex.ToString()));
            }

            return returnList;
        }

        public static int GetCurrentStaffId()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
            {
                return currentUser.CompanyId;
            }

            return 0;
        }

        public static string GetStaffName(IdentityUser staff)
        {
            var staffName = string.Empty;
            if (staff != null)
            {
                staffName = !string.IsNullOrEmpty(staff.FullName) ? staff.FullName : staff.UserName;

                if (string.IsNullOrEmpty(staffName))
                {
                    staffName = staff.Email;
                }
            }

            return staffName;
        }

        public static void ClearUserCache(IdentityUser currentUser)
        {
            try
            {
                var staffKey = string.Format("{0}_{1}", "USER", currentUser.StaffId);
                var idKey = string.Format("{0}_{1}", "USER", currentUser.Id);

                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                cacheProvider.Clear(staffKey);
                cacheProvider.Clear(idKey);
            }
            catch (Exception ex)
            {
                _logger.Error("Could not ClearUserCache: " + ex.ToString());
            }
        }
        public static void ClearCompanyCache(IdentityCompany currentUser)
        {
            try
            {
                var staffKey = string.Format("{0}_{1}", "COMPANY", currentUser.CompanyId);
                var idKey = string.Format("{0}_{1}", "COMPANY", currentUser.Id);

                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                cacheProvider.Clear(staffKey);
                cacheProvider.Clear(idKey);
            }
            catch (Exception ex)
            {
                _logger.Error("Could not ClearUserCache: " + ex.ToString());
            }
        }
        //public static List<string> GetEmailMessageIdSynchronized(int agencyId, int staffId)
        //{
        //    var currentAction = "GetEmailMessageIdSynchronized";
        //    List<string> returnList = null;
        //    var myKey = string.Format(EnumFormatInfoCacheKeys.EmailIdsSynchronized, agencyId, staffId);

        //    try
        //    {
        //        var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
        //        returnList = cacheProvider.Get<List<string>>(myKey);

        //        if (!returnList.HasData())
        //        {
        //            var myStore = Startup.IocContainer.Resolve<IStoreEmailMessage>();
        //            returnList = myStore.GetSynchronizedIds(agencyId, staffId);

        //            if (returnList.HasData())
        //            {
        //                cacheProvider.Set(myKey, returnList);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(string.Format("Failed when [{0}] due to {1}", currentAction, ex.ToString()));
        //    }

        //    return returnList;
        //}

        //public static List<IdentityEmailServer> GetListEmailServers(int agencyId)
        //{
        //    var currentAction = "GetListEmailServers";
        //    List<IdentityEmailServer> returnList = null;
        //    var myKey = string.Format(EnumFormatInfoCacheKeys.EmailServers, agencyId);

        //    try
        //    {
        //        var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
        //        returnList = cacheProvider.Get<List<IdentityEmailServer>>(myKey);
        //        if (!returnList.HasData())
        //        {
        //            var myStore = Startup.IocContainer.Resolve<IStoreEmailServer>();
        //            returnList = myStore.GetListByAgency(agencyId);

        //            if (returnList.HasData())
        //            {
        //                foreach (var item in returnList)
        //                {
        //                    item.ReceivingConfig = JsonConvert.DeserializeObject<IdentityEmailServerPOPConfig>(item.POPConfig);
        //                    item.SendingConfig = JsonConvert.DeserializeObject<IdentityEmailServerSMTPConfig>(item.SMTPConfig);
        //                }
        //                cacheProvider.Set(myKey, returnList, SystemSettings.DefaultCachingTimeInMinutes);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(string.Format("Failed when [{0}] due to {1}", currentAction, ex.ToString()));
        //    }

        //    return returnList;
        //}

        //public static List<IdentityEmailSetting> GetListEmailSettings(int agencyId, int staffId)
        //{
        //    var currentAction = "GetListEmailSettings";
        //    List<IdentityEmailSetting> returnList = null;
        //    var myKey = string.Format(EnumFormatInfoCacheKeys.EmailSettings, agencyId, staffId);

        //    try
        //    {
        //        var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
        //        cacheProvider.Get<List<IdentityEmailSetting>>(myKey);

        //        if (!returnList.HasData())
        //        {
        //            var myStore = Startup.IocContainer.Resolve<IStoreEmailServer>();
        //            returnList = myStore.GetEmailSettingsByStaff(agencyId, staffId);

        //            if (returnList.HasData())
        //                cacheProvider.Set(myKey, returnList, SystemSettings.DefaultCachingTimeInMinutes);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(string.Format("Failed when [{0}] due to {1}", currentAction, ex.ToString()));
        //    }

        //    return returnList;
        //}

        public static void ClearEmailSettings(int agencyId, int staffId)
        {
            var currentAction = "ClearEmailSettings";
            var myKey = string.Format(EnumFormatInfoCacheKeys.EmailSettings, agencyId, staffId);

            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Clear(myKey);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Failed when [{0}] due to {1}", currentAction, ex.ToString()));
            }
        }

        #region Region, Prefecture, City

        public static List<IdentityRegion> GetListRegions(int countryId = 81)
        {
            var list = new List<IdentityRegion>();
            try
            {
                var apiRs = MasterServices.GetListRegionsAsync(countryId).Result;
                list = apiRs.ConvertData<List<IdentityRegion>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListRegions: " + ex.ToString());
            }

            return list;
        }

        public static List<IdentityCity> GetListCitiesByPrefecture(int prefectureId)
        {
            var list = new List<IdentityCity>();
            try
            {
                var apiRs = MasterServices.GetListCitiesByPrefectureAsync(prefectureId).Result;
                list = apiRs.ConvertData<List<IdentityCity>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListCitiesByPrefecture: " + ex.ToString());
            }

            return list;
        }

        #endregion

        public static string GetBackLink(string defaultUrl = "")
        {
            var request = System.Web.HttpContext.Current.Request;
            var backLink = request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(backLink) || backLink == request.GetDisplayUrl())
            {
                backLink = defaultUrl;
            }

            return backLink;
        }

        public static List<IdentityEmailServer> GetListEmailServers(int agencyId)
        {
            var currentAction = "GetListEmailServers";
            List<IdentityEmailServer> returnList = null;
            var myKey = string.Format(EnumFormatInfoCacheKeys.EmailServers, agencyId);

            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                returnList = cacheProvider.Get<List<IdentityEmailServer>>(myKey);
                if (!returnList.HasData())
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreEmailServer>();
                    returnList = myStore.GetListByAgency(agencyId);

                    if (returnList.HasData())
                    {
                        foreach (var item in returnList)
                        {
                            item.ReceivingConfig = JsonConvert.DeserializeObject<IdentityEmailServerPOPConfig>(item.POPConfig);
                            item.SendingConfig = JsonConvert.DeserializeObject<IdentityEmailServerSMTPConfig>(item.SMTPConfig);
                        }
                        cacheProvider.Set(myKey, returnList, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Failed when [{0}] due to {1}", currentAction, ex.ToString()));
            }

            return returnList;
        }

        public static List<IdentityEmailSetting> GetListEmailSettings(int agencyId, int staffId)
        {
            var currentAction = "GetListEmailSettings";
            List<IdentityEmailSetting> returnList = null;
            var myKey = string.Format(EnumFormatInfoCacheKeys.EmailSettings, agencyId, staffId);

            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Get<List<IdentityEmailSetting>>(myKey);

                if (!returnList.HasData())
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreEmailServer>();
                    returnList = myStore.GetEmailSettingsByStaff(agencyId, staffId);

                    if (returnList.HasData())
                        cacheProvider.Set(myKey, returnList, SystemSettings.DefaultCachingTimeInMinutes);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Failed when [{0}] due to {1}", currentAction, ex.ToString()));
            }

            return returnList;
        }
    }
}
