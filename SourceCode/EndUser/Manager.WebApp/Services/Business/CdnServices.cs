using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Serilog;
using Manager.WebApp.Models;
using Manager.SharedLibs;
using System.IO;
using Manager.WebApp.Settings;
using RestSharp;
using System.Reflection;

namespace Manager.WebApp.Services
{
    public class CdnServices : CommonServices
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(CdnServices));

        //public static async Task<ApiResponseModel> UploadImagesAsync(ApiUploadFileModel model)
        //{
        //    var strError = string.Empty;
        //    var result = new ApiResponseModel();
        //    var _baseUrl = string.Format("{0}/{1}", SystemSettings.MainApi, "api/upload/images");

        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            using (var content = new MultipartFormDataContent())
        //            {
        //                if(model.Files != null && model.Files.Count > 0)
        //                {
        //                    foreach (var item in model.Files)
        //                    {
        //                        using (var ms = new MemoryStream())
        //                        {
        //                            item.CopyTo(ms);
        //                            var fileBytes = ms.ToArray();

        //                            var fileContent = new ByteArrayContent(fileBytes);
        //                            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = item.FileName };
        //                            content.Add(fileContent);
        //                        }                                
        //                    }
        //                }                       

        //                //Extends parameters
        //                Dictionary<string, string> parameters = new Dictionary<string, string>();
        //                parameters.Add("SubDir", model.SubDir.ToString());
        //                parameters.Add("ObjectId", model.ObjectId.ToString());
        //                parameters.Add("InCludeDatePath", model.InCludeDatePath.ToString());
        //                HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
        //                content.Add(DictionaryItems, "MyFormData");
        //                var response = client.PostAsync(_baseUrl, content).Result;

        //                //Trace log
        //                HttpStatusCodeTrace(response);

        //                // Parsing the returned result                    
        //                var responseString = await response.Content.ReadAsStringAsync();

        //                result = JsonConvert.DeserializeObject<ApiResponseModel>(responseString);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

        //        throw new CustomSystemException(ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT);
        //    }

        //    return result;
        //}

        public static async Task<ApiResponseModel> UploadHouseMediaFilesAsync(ApiUploadFileModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest("api/mediafile/house/upload", Method.POST);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddQueryParameter("Id", model.ObjectId);
                //restRequest.AddQueryParameter("MediaType", model.MediaType.ToString());

                if (model.Files.HasData()) {
                    foreach (var f in model.Files)
                    {
                        using(var stream = new MemoryStream())
                        {
                            f.CopyTo(stream);

                            var fileBytes = stream.ToArray();
                            restRequest.AddFile(f.Name, fileBytes, f.FileName, "multipart/form-data");
                        }                        
                    }
                }

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

        public static async Task<ApiResponseModel> DeleteMediaFilesAsync(string code, string url)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest("api/mediafile", Method.DELETE);

                restRequest.AddQueryParameter("Code", code);
                restRequest.AddQueryParameter("Url", url);

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

        public static async Task<ApiResponseModel> UploadBoxMediaFilesAsync(ApiUploadFileModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest("api/mediafile/box/upload", Method.POST);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddQueryParameter("Id", model.ObjectId);

                if (model.Files.HasData())
                {
                    foreach (var f in model.Files)
                    {
                        using (var stream = new MemoryStream())
                        {
                            f.CopyTo(stream);

                            var fileBytes = stream.ToArray();
                            restRequest.AddFile(f.Name, fileBytes, f.FileName, "multipart/form-data");
                        }
                    }
                }

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

        public static async Task<ApiResponseModel> UploadProductMediaFilesAsync(ApiUploadFileModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest("api/mediafile/product/upload", Method.POST);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddQueryParameter("Id", model.ObjectId);

                if (model.Files.HasData())
                {
                    foreach (var f in model.Files)
                    {
                        using (var stream = new MemoryStream())
                        {
                            f.CopyTo(stream);

                            var fileBytes = stream.ToArray();
                            restRequest.AddFile(f.Name, fileBytes, f.FileName, "multipart/form-data");
                        }
                    }
                }

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

        public static async Task<ApiResponseModel> UploadLinkSettingMediaFilesAsync(ApiUploadFileModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest("api/mediafile/linksetting/upload", Method.POST);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddQueryParameter("Id", model.ObjectId);

                if (model.Files.HasData())
                {
                    foreach (var f in model.Files)
                    {
                        using (var stream = new MemoryStream())
                        {
                            f.CopyTo(stream);

                            var fileBytes = stream.ToArray();
                            restRequest.AddFile(f.Name, fileBytes, f.FileName, "multipart/form-data");
                        }
                    }
                }

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

        public static async Task<ApiResponseModel> UploadCategoryMediaFilesAsync(ApiUploadFileModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(SystemSettings.MainApi);
                MainApiAuthorization(restClient);

                IRestRequest restRequest = new RestRequest("api/mediafile/category/upload", Method.POST);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddQueryParameter("Id", model.ObjectId);

                if (model.Files.HasData())
                {
                    foreach (var f in model.Files)
                    {
                        using (var stream = new MemoryStream())
                        {
                            f.CopyTo(stream);

                            var fileBytes = stream.ToArray();
                            restRequest.AddFile(f.Name, fileBytes, f.FileName, "multipart/form-data");
                        }
                    }
                }

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