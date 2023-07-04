using MainApi.DataLayer.Entities;
using System.Collections.Generic;
using System;
using MainApi.DataLayer.Entities.Entities;
using Manager.WebApp.Resources;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Manager.WebApp.Models
{
    public class ManageUserModel : CommonPagingModel
    {
        public List<IdentityUser> SearchResults { get; set; }
    }
    public class UserDetailModel : IdentityUser
    {
        public string CurrentTab { get; set; }
    }

    public class UserUpdateModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool ReceiveAllUpdate { get; set; }
        public bool RemoveAds { get; set; }
        public IFormFile image_file_upload { get; set; }
        public string CoverImage { get; set; }

    }


}
