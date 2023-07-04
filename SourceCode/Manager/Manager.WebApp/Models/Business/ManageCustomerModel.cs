using MainApi.DataLayer.Entities;
using System.Collections.Generic;
using System;
using MainApi.DataLayer.Entities.Entities;
using Manager.WebApp.Resources;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Manager.WebApp.Models
{
    public class ManageCustomerModel : CommonPagingModel
    {
        public List<IdentityCustomer> SearchResults { get; set; }
    }
    public class CustomerDetailModel : IdentityCustomer
    {
        public string CurrentTab { get; set; }
    }

    public class CustomerUpdateModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool ReceiveAllUpdate { get; set; }
        public bool RemoveAds { get; set; }
        public IFormFile image_file_upload { get; set; }
        public string CoverImage { get; set; }

    }


}
