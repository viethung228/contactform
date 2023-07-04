using MainApi.Helpers;
using MainApi.SharedLibs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Serilog.Core;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using Ubiety.Dns.Core;

namespace MainApi.Controllers
{
    [Route("[controller]")]
    public class StaticController : Controller
    {
        private readonly ILogger<StaticController> _logger;
        public static char DirSeparator = System.IO.Path.DirectorySeparatorChar;

        public StaticController(ILogger<StaticController> logger)
        {
            _logger = logger;
        }

        //[OutputCache(CacheProfile = "DefaultOutputCache")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        [HttpGet]
        [Route("Media")]
        public ActionResult Media(string url)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + DirSeparator + url);

                if (System.IO.File.Exists(path))
                {
                    string contentType = GetMIMEType(path);

                    var lastMod = EpochTime.GetIntDate(System.IO.File.GetLastWriteTimeUtc(path));
                    var md5Path = Utility.Md5HashingData(path);
                    String etag = Utility.Md5HashingData(md5Path + lastMod); //e.g. "00amyWGct0y_ze4lIsj2Mw"

                    var currentMatch = string.Empty;
                    if (!String.IsNullOrEmpty(Request.Headers["If-None-Match"]))
                        currentMatch = Request.Headers["If-None-Match"].ToString();

                    if (!String.IsNullOrEmpty(Request.Headers["if-none-match"]))
                        currentMatch = Request.Headers["if-none-match"].ToString();

                    if (!String.IsNullOrEmpty(currentMatch))
                    {
                        if (currentMatch == etag)
                        {
                            // Response.StatusCode = 304;
                            // Response.StatusDescription = "Not Modified";
                            // return new EmptyResult();
                            //// return new HttpStatusCodeResult(HttpStatusCode.NotModified);
                            return new StatusCodeResult((int)HttpStatusCode.NotModified);
                        }
                    }
                    else
                    {
                        var bytes = System.IO.File.ReadAllBytes(path);
                        var fileName = Path.GetFileName(path);
                        // add ETag response header 
                        Response.Headers.Add(HeaderNames.ETag, new[] { etag });

                        return File(bytes, contentType, fileName);
                    }
                }
                else
                {
                    return GetDefaultImage();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return GetDefaultImage();
        }

        private FileContentResult GetDefaultImage()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + DirSeparator + "Content/images/no-image.png");
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "image/jpeg");
        }

        [NonAction]
        private string GetMIMEType(string fileName)
        {
            var provider =
                new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

    }
}
