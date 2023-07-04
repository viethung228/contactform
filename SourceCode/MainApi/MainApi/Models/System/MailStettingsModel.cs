using MainApi.DataLayer.Entities;
using MainApi.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MainApi.Helpers.LanguagesProvider;
namespace MainApi.Models
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class MailContent
    {
        public string To { get; set; }      
        public string Subject { get; set; }     
        public string Body { get; set; }            

    }
}