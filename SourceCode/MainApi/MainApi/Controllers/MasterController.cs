﻿using MainApi.Resources;
using System.Globalization;
using System.Threading;
using MainApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Autofac;
using System;

namespace MainApi.Controllers
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
                _logger.LogError("Could not ClearCache: {0}", ex.ToString());
            }

            return Content("Done !!!");
        }
    }
}
