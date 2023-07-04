using MainApi.DataLayer.Entities;
using MainApi.Resources;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using static MainApi.Helpers.LanguagesProvider;

namespace MainApi.Models
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
    public class MailRequestWithHTML
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
        public string Link { get; set; }
        public string IdUser { get; set; }
    }
}