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
using System.Data.SqlTypes;

namespace Manager.WebApp.Services
{
    public class ContactFormServices : CommonServices
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ContactFormServices));
        private static readonly string apiRoute = "api/contactform";

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
                if (model.Dependents != null && model.Dependents.Count > 0)
                {
                    foreach (var item in model.Dependents)
                    {
                        item.DependentSpouseNenkinNumber = item.DependentSpouseNenkinNumber == null ? "" : item.DependentSpouseNenkinNumber;
                        item.Furigana = item.Furigana == null ? "" : item.Furigana;
                        item.FullName = item.FullName == null ? "" : item.FullName;
                        item.Relationship = item.Relationship == null ? "" : item.Relationship;
                        item.Occupation = item.Occupation == null ? "" : item.Occupation;
                        item.Address = item.Address == null ? "" : item.Address;
                        item.DateOfBirth = SqlDateTime.MinValue.Value > item.DateOfBirth ? DateTime.Now : item.DateOfBirth;
                        item.EnrollmentDate = SqlDateTime.MinValue.Value > item.EnrollmentDate ? DateTime.Now : item.EnrollmentDate;
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
        public static async Task<ApiResponseModel> GetAllCompany(string companyName,int currentpage = 1, int pagesize = 10)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/getallcompany", apiRoute), Method.GET);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddParameter("currentpage", currentpage);
                restRequest.AddParameter("pagesize", pagesize);
                restRequest.AddParameter("companyName", companyName);

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

        public static async Task<ApiResponseModel> GetByPage(string keyword, int currentpage = 1, int pagesize = 10)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}", apiRoute), Method.GET);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddParameter("currentpage", currentpage);
                restRequest.AddParameter("pagesize", pagesize);
                restRequest.AddParameter("keyword", keyword);

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

        public static async Task<ApiResponseModel> GetList()
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/getall", apiRoute), Method.GET);

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

        public static async Task<ApiResponseModel> GetContactFormByCompanyName(string keyword,string companyName, int currentpage = 1, int pagesize = 10)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/getcontactformbycompany", apiRoute), Method.GET);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddParameter("currentpage", currentpage);
                restRequest.AddParameter("pagesize", pagesize);
                restRequest.AddParameter("companyName", companyName);
                restRequest.AddParameter("keyword", keyword);

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
        public static async Task<ApiResponseModel> GetContactFormsByCompanyIdAsync(int id)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/contact_form_employee?id={1}", apiRoute, id), Method.GET);

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
        public static async Task<ApiResponseModel> GetDependentsByFormIdAsync(int id)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/dependent?id={1}", apiRoute, id), Method.GET);

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

        public static async Task<ApiResponseModel> GetAllowanceByFormIdAsync(int id)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/allowance?id={1}", apiRoute, id), Method.GET);

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

        public static async Task<ApiResponseModel> GetAllowanceDetailByAllowanceIdAsync(int id)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/allowance_type?id={1}", apiRoute, id), Method.GET);

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

        public static async Task<ApiResponseModel> GetContactFormByFormIdAsync(int id)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/contact_form?id={1}", apiRoute, id), Method.GET);

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
        public static async Task<ApiResponseModel> DeleteContactFormAsync(int formId)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest(string.Format("{0}/{1}", apiRoute, formId), Method.DELETE);

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
                _logger.Error(string.Format("Return code: {0}, message: {1}", response.StatusCode, response.ResponseUri));
            }
        }
    }
}