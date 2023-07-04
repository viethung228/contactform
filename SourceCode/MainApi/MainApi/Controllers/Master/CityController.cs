using MainApi.Helpers;
using MainApi.Models;
using MainApi.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using NetCoreMVC.Controllers;
using Microsoft.AspNetCore.Authorization;
using Stripe;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http.Headers;

namespace MainApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : AuthorizeBaseController
    {
        private readonly ILogger<CityController> _logger;
        private const string Webhook = "whsec_9c9a21e970788a070a36796a20e86f882b9d4e3102ac78c0241a29737de27294";
        public CityController(ILogger<CityController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public ApiResponseModel GetById(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                var info = CommonHelpers.GetBaseInfoCity(id);
                returnModel.Data = info;
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = (int)EnumCommonCode.Error;
            }

            return returnModel;
        }
        [HttpPost("/webhook")]
        public async Task<ActionResult> WebhookHandler()
        {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], Webhook);

                //Handle the event
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {

                }
                else
                {
                    Console.WriteLine("Unhandled event type :{0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e) { return BadRequest(); }
        }
        [HttpGet("listbyprefecture/{id}")]
        public ApiResponseModel GetListByPrefecture(int id)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                returnModel.Data = CommonHelpers.GetListCitiesByPrefecture(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                returnModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
                returnModel.Code = (int)EnumCommonCode.Error;
            }

            return returnModel;
        }
    }
}
