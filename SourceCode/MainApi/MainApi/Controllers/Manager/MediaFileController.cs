using MainApi.SharedLibs;
using MainApi.Helpers;
using MainApi.Models;
using MainApi.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;

namespace MainApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaFileController : ControllerBase
    {
        private readonly ILogger<MediaFileController> _logger;

        public MediaFileController(ILogger<MediaFileController> logger)
        {
            _logger = logger;
        }

        [RequestSizeLimit(3000000000)]
        [HttpPost]
        [Route("label/upload")]
        public async Task<ApiResponseModel> UploadLabelMedia()
        {
            var resModel = new ApiResponseModel();
            try
            {
                List<IFormFile> files = Request.Form.Files.ToList();
                if (!files.HasData())
                    files = Request.Form.Files != null ? Request.Form.Files.ToList() : null;

                if (files.HasData())
                {
                    var returnList = new List<ApiResponseUploadFileModel>();
                    foreach (var item in files)
                    {
                        var imageTypes = new[] { "png", "jpg", "jpeg", "tiff", "gif" };
                        var videoTypes = new[] { "mp4", "mov", "wmv", "avi", "flv", "mkv" };

                        var fileExt = System.IO.Path.GetExtension(item.FileName).Substring(1);
                        if (imageTypes.Contains(fileExt))
                        {
                            //Upload image
                            var result = FileUploadHelper.UploadImageAsync(item, "Label", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                result.Type = (int)EnumMediaFileType.Image;
                                returnList.Add(result);
                            }
                        }
                        else if (videoTypes.Contains(fileExt))
                        {
                            //Upload video
                            var result = FileUploadHelper.UploadFileAsync(item, "Label", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                result.Type = (int)EnumMediaFileType.Video;
                                returnList.Add(result);
                            }

                            returnList.Add(result);
                        }
                        else
                        {
                            //Upload attachment
                            var result = FileUploadHelper.UploadFileAsync(item, "Label", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                result.Type = (int)EnumMediaFileType.Other;
                                returnList.Add(result);
                            }

                            returnList.Add(result);
                        }
                    }

                    resModel.Data = returnList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                resModel.Code = (int)EnumCommonCode.Error;
                resModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
            }

            return resModel;
        }

        [RequestSizeLimit(3000000000)]
        [HttpPost]
        [Route("label/uploadsingle")]
        public async Task<ApiResponseModel> UploadLabelMediaSingle()
        {
            var resModel = new ApiResponseModel();
            try
            {
                List<IFormFile> files = Request.Form.Files.ToList();
                if (!files.HasData())
                    files = Request.Form.Files != null ? Request.Form.Files.ToList() : null;

                if (files.HasData())
                {
                    var fileName = Request.Query["FileName"].ToString();
                    var returnList = new List<ApiResponseUploadFileModel>();
                    foreach (var item in files)
                    {
                        var imageTypes = new[] { "png", "jpg", "jpeg", "tiff", "gif" };
                        var videoTypes = new[] { "mp4", "mov", "wmv", "avi", "flv", "mkv" };

                        var fileExt = System.IO.Path.GetExtension(fileName.Substring(1));
                        if (imageTypes.Contains(fileExt))
                        {
                            //Upload image
                            var result = FileUploadHelper.UploadFileCustomAsync(item, fileName, "Label", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                result.Type = (int)EnumMediaFileType.Image;
                                returnList.Add(result);
                            }
                        }
                        else if (videoTypes.Contains(fileExt))
                        {
                            //Upload video
                            var result = FileUploadHelper.UploadFileCustomAsync(item, fileName, "Label", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                result.Type = (int)EnumMediaFileType.Video;
                                returnList.Add(result);
                            }

                            returnList.Add(result);
                        }
                        else
                        {
                            //Upload attachment
                            var result = FileUploadHelper.UploadFileCustomAsync(item, fileName, "Label", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                result.Type = (int)EnumMediaFileType.Other;
                                returnList.Add(result);
                            }

                            returnList.Add(result);
                        }
                    }

                    resModel.Data = returnList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                resModel.Code = (int)EnumCommonCode.Error;
                resModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
            }

            return resModel;
        }


        [HttpPost]
        [Route("product/upload")]
        [RequestSizeLimit(3000000000)]
        public async Task<ApiResponseModel> UploadProductMedia(List<IFormFile> files)
        {
            var resModel = new ApiResponseModel();
            try
            {
                if (!files.HasData())
                    files = Request.Form.Files != null ? Request.Form.Files.ToList() : null;

                if (files.HasData())
                {
                    var returnList = new List<ApiResponseUploadFileModel>();
                    foreach (var item in files)
                    {
                        var imageTypes = new[] { "png", "jpg", "jpeg", "tiff", "gif", "webp" };

                        var fileExt = System.IO.Path.GetExtension(item.FileName).Substring(1).ToLower();
                        if (imageTypes.Contains(fileExt))
                        {
                            if (!fileExt.Contains("webp"))
                            {
                                //Upload image
                                var result = FileUploadHelper.UploadImageAsync(item, "Product", true).Result;

                                await Task.FromResult(result);

                                if (result != null)
                                {
                                    result.Type = (int)EnumMediaFileType.Image;
                                    returnList.Add(result);
                                }
                            }
                            else
                            {
                                //Upload image
                                var result = FileUploadHelper.UploadFileAsync(item, "Product", true).Result;

                                await Task.FromResult(result);

                                if (result != null)
                                {
                                    result.Type = (int)EnumMediaFileType.Image;
                                    returnList.Add(result);
                                }
                            }
                        }
                        else
                        {
                            //Upload file
                            var result = FileUploadHelper.UploadFileAsync(item, "Product", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                var videoTypes = new[] {
                                    "mp4", "mov", "mkv", "webm", "ogg", "avi", "wmv",
                                    "flv", "swf", "mpe"
                                };

                                var videoFileExt = System.IO.Path.GetExtension(item.FileName).Substring(1).ToLower();
                                if (videoTypes.Contains(videoFileExt))
                                    result.Type = (int)EnumMediaFileType.Video;
                                else
                                    result.Type = (int)EnumMediaFileType.Other;
                                returnList.Add(result);
                            }

                            returnList.Add(result);
                        }
                    }

                    resModel.Data = returnList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                resModel.Code = (int)EnumCommonCode.Error;
                resModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
            }

            return resModel;
        }

        [RequestSizeLimit(3000000000)]
        [HttpPost]
        [Route("category/upload")]
        public async Task<ApiResponseModel> UploadCategoryMedia(List<IFormFile> files)
        {
            var resModel = new ApiResponseModel();
            try
            {
                if (!files.HasData())
                    files = Request.Form.Files != null ? Request.Form.Files.ToList() : null;

                if (files.HasData())
                {
                    var returnList = new List<ApiResponseUploadFileModel>();
                    foreach (var item in files)
                    {
                        var imageTypes = new[] { "png", "jpg", "jpeg", "tiff", "gif" };

                        var fileExt = System.IO.Path.GetExtension(item.FileName).Substring(1);
                        if (imageTypes.Contains(fileExt))
                        {
                            //Upload image
                            var result = FileUploadHelper.UploadImageAsync(item, "Avatars", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                result.Type = (int)EnumMediaFileType.Image;
                                returnList.Add(result);
                            }
                        }
                    }

                    resModel.Data = returnList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                resModel.Code = (int)EnumCommonCode.Error;
                resModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
            }

            return resModel;
        }

        [RequestSizeLimit(3000000000)]
        [HttpPost]
        [Route("linksetting/upload")]
        public async Task<ApiResponseModel> UploadLinkSettingMedia(List<IFormFile> files)
        {
            var resModel = new ApiResponseModel();
            try
            {
                if (!files.HasData())
                    files = Request.Form.Files != null ? Request.Form.Files.ToList() : null;

                if (files.HasData())
                {
                    var returnList = new List<ApiResponseUploadFileModel>();
                    foreach (var item in files)
                    {
                        var imageTypes = new[] { "png", "jpg", "jpeg", "tiff", "gif" };

                        var fileExt = System.IO.Path.GetExtension(item.FileName).Substring(1);
                        if (imageTypes.Contains(fileExt))
                        {
                            //Upload image
                            var result = FileUploadHelper.UploadImageAsync(item, "LinkSetting", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                result.Type = (int)EnumMediaFileType.Image;
                                returnList.Add(result);
                            }
                        }
                    }

                    resModel.Data = returnList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                resModel.Code = (int)EnumCommonCode.Error;
                resModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
            }

            return resModel;
        }

        [RequestSizeLimit(3000000000)]
        [HttpPost]
        [Route("bottle/upload")]
        public async Task<ApiResponseModel> UploadBottleMedia(List<IFormFile> files)
        {
            var resModel = new ApiResponseModel();
            try
            {
                if (!files.HasData())
                    files = Request.Form.Files != null ? Request.Form.Files.ToList() : null;

                if (files.HasData())
                {
                    var returnList = new List<ApiResponseUploadFileModel>();
                    foreach (var item in files)
                    {
                        var imageTypes = new[] { "png", "jpg", "jpeg", "tiff", "gif" };

                        var fileExt = System.IO.Path.GetExtension(item.FileName).Substring(1);
                        if (imageTypes.Contains(fileExt))
                        {
                            //Upload image
                            var result = FileUploadHelper.UploadImageAsync(item, "Bottle", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                result.Type = (int)EnumMediaFileType.Image;
                                returnList.Add(result);
                            }
                        }
                    }

                    resModel.Data = returnList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                resModel.Code = (int)EnumCommonCode.Error;
                resModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
            }

            return resModel;
        }

        [RequestSizeLimit(3000000000)]
        [HttpPost]
        [Route("box/upload")]
        public async Task<ApiResponseModel> UploadBoxMedia(List<IFormFile> files)
        {
            var resModel = new ApiResponseModel();
            try
            {
                if (!files.HasData())
                    files = Request.Form.Files != null ? Request.Form.Files.ToList() : null;

                if (files.HasData())
                {
                    var returnList = new List<ApiResponseUploadFileModel>();
                    foreach (var item in files)
                    {
                        var imageTypes = new[] { "png", "jpg", "jpeg", "tiff", "gif" };

                        var fileExt = System.IO.Path.GetExtension(item.FileName).Substring(1);
                        if (imageTypes.Contains(fileExt))
                        {
                            //Upload image
                            var result = FileUploadHelper.UploadImageAsync(item, "Box", true).Result;

                            await Task.FromResult(result);

                            if (result != null)
                            {
                                result.Type = (int)EnumMediaFileType.Image;
                                returnList.Add(result);
                            }
                        }
                    }

                    resModel.Data = returnList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                resModel.Code = (int)EnumCommonCode.Error;
                resModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
            }

            return resModel;
        }

        [HttpDelete]
        public ApiResponseModel DeleteMediaFiles(List<CdnMediaFile> files)
        {
            var resModel = new ApiResponseModel();
            try
            {
                if (files.HasData())
                {
                    foreach (var item in files)
                    {
                        FileUploadHelper.DeleteMediaFile(item.Url);
                    }

                    resModel.Message = ManagerResource.LB_DELETE_SUCCESS;
                }
                else
                {
                    resModel.Code = (int)EnumCommonCode.Error;
                    resModel.Message = ManagerResource.COMMON_ERROR_DATA_INVALID;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());

                resModel.Code = (int)EnumCommonCode.Error;
                resModel.Message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
            }

            return resModel;
        }
    }
}
