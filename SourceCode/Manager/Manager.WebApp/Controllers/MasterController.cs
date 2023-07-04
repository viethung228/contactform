using Manager.WebApp.Resources;
using System.Globalization;
using System.Threading;
using Manager.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Autofac;
using System;
using MainApi.DataLayer.Entities;
using Manager.SharedLibs;
using System.Collections.Generic;
using System.Reflection;
using Manager.WebApp.Services;

namespace Manager.WebApp.Controllers
{
    public class MasterController : BaseAuthedController
    {
        private readonly ILogger<MasterController> _logger;

        public MasterController(ILogger<MasterController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public JsonResult GetResources()
        {
            var lang = CommonHelpers.GetCurrentLanguageOrDefault();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            var resources = ResourceSerialiser.ToJson(typeof(ManagerResource), lang);

            return Json(resources);
        }

        [AllowAnonymous]
        public ActionResult ClearCache()
        {
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.ClearByPrefix("");
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return Content("Done !!!");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPrefecturesByRegion(int regionId)
        {
            List<IdentityPrefecture> returnList = null;
            try
            {
                var apiRs = MasterServices.GetListPrefectureByRegionAsync(regionId).Result;

                returnList = apiRs.ConvertData<List<IdentityPrefecture>>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return PartialView("~/Views/Widgets/Items/Master/Option/_PrefectureList.cshtml", returnList);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GetCitiesByPrefecture(int prefectureId)
        //{
        //    List<IdentityCity> returnList = null;
        //    try
        //    {
        //        returnList = HelperMaster.GetListCitiesByPrefecture(prefectureId);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
        //    }

        //    return PartialView("~/Views/Widgets/Items/Master/Option/_CityList.cshtml", returnList);
        //}
    }
}
