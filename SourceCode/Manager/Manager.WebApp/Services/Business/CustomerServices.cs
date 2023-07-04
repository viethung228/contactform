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
    public class CustomerServices : CommonServices
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(CustomerServices));
        private static readonly string apiRoute = "api/customer";

        public static async Task<ApiResponseModel> GetByPageAsync(ManageCustomerModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(apiRoute, Method.GET);

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

        public static async Task<ApiResponseModel> GetDetailAsync(CustomerUpdateModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/{1}", apiRoute, model.Id), Method.GET);

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

        public static async Task<ApiResponseModel> UpdateAsync(CustomerUpdateModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/update", apiRoute), Method.POST);
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

        public static async Task<ApiResponseModel> DeleteAsync(CustomerUpdateModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/{1}", apiRoute, model.Id), Method.DELETE);

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
        public static async Task<ApiResponseModel> GetPoint(string id)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("api/coin/getpoint?id=" + id), Method.GET);

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
        public static async Task<ApiResponseModel> GetHistoryPoint(string id, int currentpage, int pagesize)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("api/coin/gethistorypoint?id={0}&currentpage={1}&pagesize={2}",id,currentpage,pagesize), Method.GET);

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
        public static async Task<ApiResponseModel> GetListAsync()
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/all", apiRoute), Method.GET);

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
                _logger.Error(string.Format("Return code: {0}, message: {1}", response.StatusCode, response.ResponseUri));
            }
        }
    }
}