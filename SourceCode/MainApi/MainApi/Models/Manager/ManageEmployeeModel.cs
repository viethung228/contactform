using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MainApi.Models
{
    public class ManageEmployeeModel : CommonPagingModel
    {
        public List<EmployeeDetailModel> SearchResults { get; set; }
    }

    public class EmployeeDetailModel : IdentityEmployee
    {

    }

    public class EmployeeUpdateModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string EmployeeName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool? ReceiveAllUpdate { get; set; }
        public bool? RemoveAds { get; set; }
        public IFormFile image_file_upload { get; set; }
        public string CoverImage { get; set; }

    }
}
