using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Entities.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MainApi.Models
{
    public class ManageUserModel : CommonPagingModel
    {
        public List<UserDetailModel> SearchResults { get; set; }
    }

    public class UserDetailModel : IdentityCustomer
    {

    }
    public class UserUpdateModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool? ReceiveAllUpdate { get; set; }
        public bool? RemoveAds { get; set; }
        public IFormFile image_file_upload { get; set; }
        public string CoverImage { get; set; }

    }
}
