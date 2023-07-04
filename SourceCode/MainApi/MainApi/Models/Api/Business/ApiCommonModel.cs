using MainApi.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MainApi.Models
{
    public class ApiUploadFileModel
    {
        public string ObjectId { get; set; }
        public string SubDir { get; set; }
        public bool InCludeDatePath { get; set; }

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
        public string Token { get; set; }
        public object Data { get; set; }

        public ApiResponseModel()
        {
            Code = (int)EnumCommonCode.Success;
        }
    }

    public class ApiResponseUploadFileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
    }
}
