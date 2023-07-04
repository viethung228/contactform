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
    public class RevenueServices : CommonServices
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(RevenueServices));
        private static readonly string apiRoute = "api/revenue";

        public static async Task<ApiResponseModel> GetHistoryById(string id, int currentpage, int pagesize)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(apiRoute, Method.GET);

                restRequest.AddHeader("Accept", "application/json");

                restRequest.AddQueryParameter("id", id);
                restRequest.AddQueryParameter("pagesize", pagesize.ToString());
                restRequest.AddQueryParameter("currentpage", currentpage.ToString());

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
        public static async Task<ApiResponseModel> GetHistoryAll(RevenueDetailModel identity, int pagesize, int currentpage)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);

                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(apiRoute + "/gethistoryall", Method.GET);

                restRequest.AddHeader("Accept", "application/json");

                restRequest.AddQueryParameter("pagesize", pagesize.ToString());
                restRequest.AddQueryParameter("currentpage", currentpage.ToString());

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
                _logger.Error(string.Format("Return code: {0}, message: {1}", response.StatusCode, response.ResponseUri));
            }
        }
    }
}