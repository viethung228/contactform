using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.WebApp.Models
{
    public class ImportModel
    {
        public IFormFile ImportFile { get; set; }
    }
}
