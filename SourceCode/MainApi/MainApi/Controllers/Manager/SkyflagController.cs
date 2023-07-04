using Autofac;
using MainApi.DataLayer.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System;
using MainApi.DataLayer.Stores.Manager;
using MainApi.DataLayer.Entities.Entities.Business;
using MainApi.Helpers.Business;
using System.Globalization;

namespace MainApi.Controllers
{
    [ApiController]
    [Route("ad/cv")]
    public class SkyflagController : AuthorizeManagerController
    {
        private readonly ILogger<SkyflagController> _logger;
        public SkyflagController(ILogger<SkyflagController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetData(string uid, int adid, int point, string cv_date)
        {

            var data = new IdentitySkyflag();
            try
            {
                var storeSkyflag = Startup.IocContainer.Resolve<IStoreSkyflag>();
                var storeUser = Startup.IocContainer.Resolve<IStoreUser>();
                var getData = storeUser.GetById(uid);

                if (getData == null || uid == null)
                {
                    return BadRequest();
                }

                var webhookData = new IdentitySkyflag
                {
                    suid = uid,
                    xad = adid,
                    wallpoint = point,
                    cv_date = DateTime.ParseExact(cv_date, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)

            };

                var update = storeSkyflag.Insert(webhookData);
                if (update == null)
                {
                    return BadRequest();
                }
                update.suid = getData.StaffId.ToString();
                data = update;

                //Clear cache
                WebhookHelper.ClearCache(uid);
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                return BadRequest();
            }

            return Ok(data);
        }

        [HttpGet("sur")]
        public IActionResult GetDataWithSUR(string uid, int adid, int point, string cv_date, int sur_no)
        {
            var data = new IdentitySkyflag();
            try
            {
                var storeSkyflag = Startup.IocContainer.Resolve<IStoreSkyflag>();
                var storeUser = Startup.IocContainer.Resolve<IStoreUser>();
                var getData = storeUser.GetById(uid);

                if (getData == null || uid == null)
                {
                    return BadRequest();
                }

                var webhookData = new IdentitySkyflag
                {
                    suid = uid,
                    xad = adid,
                    wallpoint = point,
                    cv_date = DateTime.ParseExact(cv_date, "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                    mcv_no = sur_no

                };

                var update = storeSkyflag.Insert(webhookData);
                if (update == null)
                {
                    return BadRequest();

                }
                update.suid = getData.StaffId.ToString();
                data = update;
                //Clear cache
                WebhookHelper.ClearCache(uid);
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                return BadRequest();
            }
            //returnModel.Data = model;

            return Ok(data);
        }
    }
}
