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

namespace Manager.WebApp.Services
{
    public class NotificationServices : CommonServices
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(NotificationServices));

        public static async Task<ApiResponseModel> GetByPageAsync(ManageNotificationModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest("api/notification", Method.GET);

                restRequest.AddHeader("Accept", "application/json");

                restRequest.AddQueryParameter("Keyword", model.Keyword);
                restRequest.AddQueryParameter("Page", model.Page.ToString());
                restRequest.AddQueryParameter("PageSize", model.PageSize.ToString());

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

        public static async Task<ApiResponseModel> GetNotificationByIdAsync(int id)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("api/notification/{0}", id), Method.GET);

                restRequest.AddHeader("Accept", "application/json");

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

        public static async Task<ApiResponseModel> CreateNotificationAsync(int actionType, int targetType, int agencyId, int contactFormId)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("api/notification/create"), Method.POST);

                restRequest.AddHeader("Accept", "application/json");

                restRequest.AddQueryParameter("actionType", actionType.ToString());
                restRequest.AddQueryParameter("targetType", targetType.ToString());
                restRequest.AddQueryParameter("agencyId", agencyId.ToString());
                restRequest.AddQueryParameter("contactFormId", contactFormId.ToString());

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
        public static async Task<ApiResponseModel> GetUnreadCountAsync()
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest("api/notification/unreadcount", Method.GET);

                restRequest.AddHeader("Accept", "application/json");

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