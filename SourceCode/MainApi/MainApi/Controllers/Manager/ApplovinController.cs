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
using MainApi.Helpers;

namespace MainApi.Controllers
{
    [ApiController]
    [Route("rewards")]
    public class ApplovinController : AuthorizeManagerController
    {
        private readonly ILogger<ApplovinController> _logger;
        public ApplovinController(ILogger<ApplovinController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetData(string idfa, string user_id, string event_id, string token, long ts, int amount)
        {
            var data = new IdentityApplovin();
            try
            {
                var storeApplovin = Startup.IocContainer.Resolve<IStoreApplovin>();
                var storeUser = Startup.IocContainer.Resolve<IStoreCustomer>();
                var getData = storeUser.GetById(user_id);

                if (getData == null || user_id == null)
                {
                    return BadRequest();
                }
                ts = ts == 0 ? DateTimeOffset.UtcNow.ToUnixTimeSeconds() : ts;
                var appLovinData = new IdentityApplovin
                {
                    idfa = idfa,
                    user_id = user_id,
                    event_id = event_id,
                    event_token = token,
                    amount = amount,
                    ts = EpochTime.DateTime(ts).ToLocalTime(),

                };

                var update = storeApplovin.Insert(appLovinData);
                if (update == null)
                {
                    return BadRequest();
                }
                update.user_id = getData.StaffId.ToString();
                data = update;

                //Clear cache
                WebhookHelper.ClearCache(user_id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                return BadRequest();
            }

            return Ok(data);
        }
    }
}
