using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manager.WebApp.Models
{
    public class ApiUploadFileModel
    {
        public string ObjectId { get; set; }
        public string SubDir { get; set; }
        public bool InCludeDatePath { get; set; }
        //public int MediaType { get; set; }
        public List<IFormFile> Files { get; set; }

        public ApiUploadFileModel()
        {
            Files = new List<IFormFile>();
        }
    }

    public class ApiResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string TokenKey { get; set; }
        public object Data { get; set; }

        public T ConvertData<T>()
        {
            try
            {
                if (this.Data != null)
                {
                    var targetObj = JsonConvert.DeserializeObject<T>(this.Data.ToString());

                    return targetObj;
                }
            }
            catch
            {
                return default(T);
            }

            return default(T);
        }
    }

    public class ApiResponseUploadFileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Type { get; set; }
    }
}
