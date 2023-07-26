using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MainApi.Models
{
    public class ManageCompanyModel : CommonPagingModel
    {
        public List<CompanyDetailModel> SearchResults { get; set; }
    }

    public class CompanyDetailModel : IdentityCompany
    {

    }

    public class CompanyUpdateModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile image_file_upload { get; set; }
        public string CoverImage { get; set; }

    }
}
