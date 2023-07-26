using MainApi.DataLayer.Entities;
using System.Collections.Generic;
using System;
using MainApi.DataLayer.Entities;
using Manager.WebApp.Resources;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Manager.WebApp.Models
{
    public class ManageCompanyModel : CommonPagingModel
    {
        public List<IdentityCompany> SearchResults { get; set; }
    }
    public class CompanyDetailModel : IdentityCompany
    {
        public string CurrentTab { get; set; }
    }

    public class CompanyUpdateModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile image_file_upload { get; set; }
        public string CoverImage { get; set; }

    }


}
