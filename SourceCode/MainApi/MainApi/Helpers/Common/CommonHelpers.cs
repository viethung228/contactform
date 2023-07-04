using Autofac;
using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Stores;
using MainApi.Settings;
using MainApi.SharedLibs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Claims;

namespace MainApi.Helpers
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
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
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
            catch(Exception ex)
            {
                _logger.Error(string.Format("GetCurrentLanguageOrDefault error because: {0}", ex.ToString()));
            }

            return LanguageMessageHandler.GetDefaultLanguage();
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

        public static void SetCookie(string key, object data, int expireInMinutes = 30)
        {
            string myObjectJson = JsonConvert.SerializeObject(data);
            var encryptedStr = Base64Encode(myObjectJson);

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(expireInMinutes);

            System.Web.HttpContext.Current.Response.Cookies.Delete(key);

            System.Web.HttpContext.Current.Response.Cookies.Append(key, encryptedStr, option);
        }

        public static ClaimsPrincipal GetLoggedInUserClaimPricipal()
        {
            return System.Web.HttpContext.Current.User;
        }

        public static IdentityUser GetCurrentUser()
        {
            IdentityUser user = null;

            var principal = GetLoggedInUserClaimPricipal();
            if(principal != null && principal.Identity.IsAuthenticated)
            {
                var userId = principal.GetLoggedInUserId<string>();
                //user = new IdentityUser();

                //user.Id = principal.GetLoggedInUserId<string>();
                //user.UserName = principal.GetLoggedInUserName();
                //user.FullName = principal.GetLoggedInUserDisplayName();

                user = GetUserById(userId);
            }

            return user;
        }

        public static string GenerateUniqueCode(string prefix = "", long uniqueNumber = 0, int lengthOfCode = 10)
        {
            if (uniqueNumber == 0)
            {
                uniqueNumber = EpochTime.GetIntDate(DateTime.UtcNow);
                if (lengthOfCode == 0)
                    lengthOfCode = 10;
            }
            else
            {
                if (lengthOfCode == 0)
                    lengthOfCode = uniqueNumber.ToString().Length;
            }

            var codeFormat = prefix + "{0:D" + lengthOfCode + "}";

            return string.Format(codeFormat, uniqueNumber);
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
                    var currentUserName = user.UserName.ToLower();
                    if (user.UserName == "admin" || user.UserName == "bangvl")
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

        #region Region,Prefecture,City

        public static IdentityPrefecture GetBaseInfoPrefecture(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Prefecture, id);
            IdentityPrefecture baseInfo = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                baseInfo = cacheProvider.Get<IdentityPrefecture>(myKey);

                if (baseInfo == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStorePrefecture>();
                    baseInfo = myStore.GetById(id);

                    if (baseInfo != null)
                    {
                        //Storage to cache
                        cacheProvider.Set(myKey, baseInfo, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetBaseInfoPrefecture: " + ex.ToString());
            }

            return baseInfo;
        }

        public static IdentityCity GetBaseInfoCity(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.City, id);
            IdentityCity baseInfo = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                baseInfo = cacheProvider.Get<IdentityCity>(myKey);

                if (baseInfo == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreCity>();
                    baseInfo = myStore.GetById(id);

                    if (baseInfo != null)
                    {
                        //Storage to cache
                        cacheProvider.Set(myKey, baseInfo, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetBaseInfoCity: " + ex.ToString());
            }

            return baseInfo;
        }

        //public static List<IdentityCountry> GetCountries()
        //{
        //    var myKey = EnumListCacheKeys.Countries;
        //    List<IdentityCountry> myList = null;
        //    try
        //    {
        //        //Check from cache first
        //        var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
        //        myList = cacheProvider.Get<List<IdentityCountry>>(myKey);

        //        if (!myList.HasData())
        //        {
        //            var myStore = Startup.IocContainer.Resolve<IStoreCountry>();
        //            myList = myStore.GetList();

        //            //Storage to cache
        //            cacheProvider.Set(myKey, myList, SystemSettings.DefaultCachingTimeInMinutes);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("Could not GetListCountries: " + ex.ToString());
        //    }

        //    return myList;
        //}

        public static List<IdentityRegion> GetListRegions(int countryId)
        {
            var myKey = string.Format(EnumListCacheKeys.RegionByCountry, countryId);
            List<IdentityRegion> myList = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                myList = cacheProvider.Get<List<IdentityRegion>>(myKey);

                if (!myList.HasData())
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreRegion>();
                    myList = myStore.GetList(countryId);

                    //Storage to cache
                    cacheProvider.Set(myKey, myList, SystemSettings.DefaultCachingTimeInMinutes);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListRegions: " + ex.ToString());
            }

            return myList;
        }

        public static IdentityRegion GetBaseInfoRegion(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Region, id);
            IdentityRegion baseInfo = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                baseInfo = cacheProvider.Get<IdentityRegion>(myKey);

                if (baseInfo == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreRegion>();
                    baseInfo = myStore.GetById(id);

                    if (baseInfo != null)
                    {
                        //Storage to cache
                        cacheProvider.Set(myKey, baseInfo, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetBaseInfoRegion: " + ex.ToString());
            }

            return baseInfo;
        }

        public static List<IdentityRegion> GetListRegionsByIds(List<int> listIds)
        {
            List<IdentityRegion> myList = new List<IdentityRegion>();
            try
            {
                if (listIds.HasData())
                {
                    foreach (var id in listIds)
                    {
                        if (id > 0)
                        {
                            var info = CommonHelpers.GetBaseInfoRegion(id);
                            if (info != null)
                                myList.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListRegionsByIds: " + ex.ToString());
            }

            return myList;
        }

        public static List<IdentityPrefecture> GetListPrefectures()
        {
            var myKey = EnumListCacheKeys.PrefecturesByRegion;
            List<IdentityPrefecture> myList = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                myList = cacheProvider.Get<List<IdentityPrefecture>>(myKey);

                if (!myList.HasData())
                {
                    var myStore = Startup.IocContainer.Resolve<IStorePrefecture>();
                    myList = myStore.GetList();

                    //Storage to cache
                    cacheProvider.Set(myKey, myList, SystemSettings.DefaultCachingTimeInMinutes);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListRegions: " + ex.ToString());
            }

            return myList;
        }

        public static List<IdentityPrefecture> GetListPrefecturesByIds(List<int> listIds)
        {
            List<IdentityPrefecture> myList = new List<IdentityPrefecture>();
            try
            {
                if (listIds.HasData())
                {
                    foreach (var id in listIds)
                    {
                        if (id > 0)
                        {
                            var info = CommonHelpers.GetBaseInfoPrefecture(id);
                            if (info != null)
                                myList.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListPrefecturesByIds: " + ex.ToString());
            }

            return myList;
        }

        public static List<IdentityCity> GetListCities()
        {
            var myKey = EnumListCacheKeys.CitiesByPrefecture;
            List<IdentityCity> myList = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                myList = cacheProvider.Get<List<IdentityCity>>(myKey);

                if (myList == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreCity>();
                    myList = myStore.GetList();

                    if (myList.HasData())
                    {
                        //Storage to cache
                        cacheProvider.Set(myKey, myList, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListCities: " + ex.ToString());
            }

            return myList;
        }

        public static List<IdentityCity> GetListCitiesByIds(List<int> listIds)
        {
            List<IdentityCity> myList = new List<IdentityCity>();
            try
            {
                if (listIds.HasData())
                {
                    foreach (var id in listIds)
                    {
                        if (id > 0)
                        {
                            var info = CommonHelpers.GetBaseInfoCity(id);
                            if (info != null)
                                myList.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListCitysByIds: " + ex.ToString());
            }

            return myList;
        }
       
        public static List<IdentityPrefecture> GetListPrefecturesByCountry(int countryId)
        {
            var myKey = string.Format(EnumListCacheKeys.PrefecturesByCountry, countryId);
            List<IdentityPrefecture> myList = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                myList = cacheProvider.Get<List<IdentityPrefecture>>(myKey);

                if (!myList.HasData())
                {
                    var myStore = Startup.IocContainer.Resolve<IStorePrefecture>();
                    myList = myStore.GetListByCountry(countryId);

                    //Storage to cache
                    cacheProvider.Set(myKey, myList, SystemSettings.DefaultCachingTimeInMinutes);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListPrefecturesByCountry: " + ex.ToString());
            }

            return myList;
        }

        public static List<IdentityPrefecture> GetListPrefecturesByRegion(int region_id)
        {
            var myKey = string.Format(EnumListCacheKeys.PrefecturesByRegion, region_id);
            List<IdentityPrefecture> myList = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                myList = cacheProvider.Get<List<IdentityPrefecture>>(myKey);

                if (!myList.HasData())
                {
                    var myStore = Startup.IocContainer.Resolve<IStorePrefecture>();
                    myList = myStore.GetListByRegion(region_id);

                    //Storage to cache
                    cacheProvider.Set(myKey, myList, SystemSettings.DefaultCachingTimeInMinutes);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListPrefecturesByRegion: " + ex.ToString());
            }

            return myList;
        }

        public static List<IdentityCity> GetListCitiesByPrefecture(int prefecture_id)
        {
            var myKey = string.Format(EnumListCacheKeys.CitiesByPrefecture, prefecture_id);
            List<IdentityCity> myList = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                myList = cacheProvider.Get<List<IdentityCity>>(myKey);

                if (!myList.HasData())
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreCity>();
                    myList = myStore.GetListByPrefecture(prefecture_id);

                    //Storage to cache
                    cacheProvider.Set(myKey, myList, SystemSettings.DefaultCachingTimeInMinutes);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetListCitiesByPrefecture: " + ex.ToString());
            }

            return myList;
        }


        #endregion
    }
}
