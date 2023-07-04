using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Serilog;
using System.IO;
using RestSharp;
using System.Reflection;
using MainApi.Models;
using MainApi.Settings;
using MainApi.SharedLibs;
using System.Collections.Generic;
using MainApi.DataLayer.Entities;

namespace Manager.WebApp.Services
{
    public class CdnService
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(CdnService));


        public static async Task<ApiResponseModel> UploadHouseMediaFilesAsync(ApiUploadFileModel model)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(CDNSettings.FileServerUrl);
                IRestRequest restRequest = new RestRequest("api/mediafile/house/upload", Method.POST);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddQueryParameter("Id", model.ObjectId);

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

        public static async Task<ApiResponseModel> DeleteMediaFilesAsync(List<IdentityMediaFile> files)
        {
            ApiResponseModel returnModel = new ApiResponseModel();
            try
            {
                var restClient = new RestClient(CDNSettings.FileServerUrl);
                IRestRequest restRequest = new RestRequest("api/mediafile", Method.DELETE);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddJsonBody(files);

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
                _logger.Debug(string.Format("Return code: {0}, message: {1}", response.StatusCode, response.ResponseUri));
            }
        }
    }
}