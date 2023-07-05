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
    public class ContactFormServices : CommonServices
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ContactFormServices));
        private static readonly string apiRoute = "api/update";

        public static async Task<ApiResponseModel> UpdateContactFormAsync(ContactFormFullDetailModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/contact_form", apiRoute), Method.POST);
                restRequest.AddJsonBody(model.ContactForm);

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
        public static async Task<ApiResponseModel> UpdateAllowanceAsync(ContactFormFullDetailModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/allowance", apiRoute), Method.POST);
                restRequest.AddJsonBody(model.Allowance);

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
        public static async Task<ApiResponseModel> UpdateAllowanceDetailAsync(ContactFormFullDetailModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/allowance_type", apiRoute), Method.POST);
                restRequest.AddJsonBody(model.AllowanceDetail);

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
        public static async Task<ApiResponseModel> UpdateDependentAsync(ContactFormFullDetailModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                if(model.Dependents!=null &&  model.Dependents.Count>0)
                {
                    foreach(var item in model.Dependents)
                    {
                        var restClient = new RestClient(SystemSettings.MainApi);
                        MainApiAuthorization(restClient);

                        IRestRequest restRequest = new RestRequest(string.Format("{0}/dependent", apiRoute), Method.POST);
                        restRequest.AddJsonBody(item);

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