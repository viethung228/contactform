using Manager.SharedLibs;
using Manager.WebApp.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using Serilog;
using Manager.WebApp.Settings;
using System.Reflection;
using MainApi.DataLayer.Entities;

namespace Manager.WebApp.Services
{
    public class AuthTokenServices
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(AuthTokenServices));
        
        public static async Task<ApiResponseModel> RenewalAsync(AuthUserModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                IRestRequest restRequest = new RestRequest("api/manager/authtoken/renewal", Method.POST);
                restRequest.AddJsonBody(model);

                restRequest.AddHeader("Content-type", "application/json");

                var result = await restClient.ExecuteAsync(restRequest);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    returnModel = JsonConvert.DeserializeObject<ApiResponseModel>(result.Content);
                }
                else
                {
                    //Trace log
                    HttpStatusCodeTrace(result);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            if (returnModel == null)
            {
                returnModel = new ApiResponseModel();
            }

            return returnModel;
        }

        private static void HttpStatusCodeTrace(IRestResponse response)
        {
            var statusCode = Utils.ConvertToInt32(response.StatusCode);
            if (statusCode != (int)HttpStatusCode.OK)
            {
                _logger.Error(string.Format("Api return error code: {0}, URL: {1}", response.StatusCode.ToString(), response.ResponseUri));
            }
        }
    }
}