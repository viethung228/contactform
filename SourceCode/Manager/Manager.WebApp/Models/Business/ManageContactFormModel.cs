using MainApi.DataLayer.Entities;
using System.Collections.Generic;

namespace Manager.WebApp.Models
{
    public class ManageContactFormModel : CommonPagingModel
    {
        public List<IdentityContactForm> SearchResults { get; set; }
    }
    public class ContactFormDetailModel : IdentityContactForm
    {
        public string CurrentTab { get; set; }
    }
    public class ContactFormFullDetailModel
    {
        public ContactFormFullDetailModel()
        {
            ContactForm = new IdentityContactForm();
            Allowance = new IdentityAllowance();
            AllowanceDetail = new IdentityAllowanceDetail();
            Dependents = new List<IdentityDependent>();
        }
        public IdentityContactForm ContactForm { get; set; }
        public IdentityAllowance Allowance { get; set; }
        public IdentityAllowanceDetail AllowanceDetail { get; set; }
        public List<IdentityDependent> Dependents { get; set; }

    }


}
