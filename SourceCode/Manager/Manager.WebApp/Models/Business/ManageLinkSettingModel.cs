using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Manager.WebApp.Models
{
    public class ManageLinkSettingModel : CommonPagingModel
    {
        public List<IdentityLinkSetting> SearchResults { get; set; }
    }

    public class LinkSettingDetailModel : IdentityLinkSetting
    {

    }

    public class LinkSettingUpdateModel
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string Link { get; set; }
        public bool Type { get; set; }
        public IFormFile image_file_upload { get; set; }
        public string CoverImage { get; set; }

    }
}
