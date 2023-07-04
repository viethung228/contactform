using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MainApi.Helpers;
using MainApi.Models;
using System.Diagnostics;
using System.IO;

namespace MainApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;          

            return View();
        }

        [RequestParamsValidation]
        public IActionResult Privacy(int id = 1)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public FileResult DownloadResult(string file)
        {
            //Build the File Path.
            string path = Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SearchResultsFolder);
            path = path + "/" + file;

            //if (System.IO.File.Exists(path))
            //{
            //    //Read the File data into Byte Array.
            //    byte[] bytes = System.IO.File.ReadAllBytes(path);

            //    //Send the File to Download.
            //    return File(bytes, "application/octet-stream", file);
            //}

            return null;
        }
    }
}
