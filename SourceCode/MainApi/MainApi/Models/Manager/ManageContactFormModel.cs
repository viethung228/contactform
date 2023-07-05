using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace MainApi.Models
{
    public class ManageContactFormModel : CommonPagingModel
    {
        public List<ContactFormDetailModel> SearchResults { get; set; }
    }

    public class ContactFormDetailModel : IdentityContactForm
    {

    }
    public class DependentDetailModel :IdentityDependent
    {

    }
    public class AllowanceDetailModel : IdentityAllowance
    {

    }
    public class AllowanceTypeDetailModel : IdentityAllowanceDetail
    {

    }
}
