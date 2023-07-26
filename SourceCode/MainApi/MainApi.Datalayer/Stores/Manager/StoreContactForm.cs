using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Repositories.Manager;
using MainApi.SharedLibs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MainApi.DataLayer.Stores
{
    public interface IStoreContactForm
    {
        IdentityDependent InsertDependent(IdentityDependent identity);
        IdentityAllowance InsertAllowance(IdentityAllowance identity);
        IdentityAllowanceDetail InsertAllowanceDetail(IdentityAllowanceDetail identity);
        IdentityContactForm InsertContactForm(IdentityContactForm identity);
        List<IdentityDependent> GetDependentsByFormId(int formId);
        IdentityAllowance GetAllowanceByFormId(int formId);
        IdentityAllowanceDetail GetAllowanceDetailByAllowanceId(int allowanceId);
        IdentityContactForm GetContactFormByFormId(int formId);
        List<IdentityContactForm> GetContactFormByEmployeeId(int id);
        IdentityContactForm DeleteContactForm(int formId);
        List<IdentityContactForm> GetAllCompany(IdentityContactForm identity, int currentpage, int pagesize);
        List<IdentityContactForm> GetContactFormByCompanyName(string keyword, string companyName, int currentpage, int pagesize);
        List<IdentityContactForm> GetByPage(string keyword, int currentpage, int pagesize);
        List<IdentityContactForm> GetList();
    }
    public class StoreContactForm : IStoreContactForm
    {
        private readonly string _conStr;
        RpsContactForm r;

        public StoreContactForm() : this("MainDBConn")
        {

        }

        public StoreContactForm(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            r = new RpsContactForm(_conStr);
        }
        public List<IdentityContactForm> GetByPage(string keyword, int currentpage, int pagesize)
        {
            return r.GetByPage(keyword, currentpage, pagesize);
        }
        public List<IdentityContactForm> GetList()
        {
            return r.GetList();
        }
        public IdentityDependent InsertDependent(IdentityDependent identity)
        {
            return r.InsertDependent(identity);
        }
        public List<IdentityContactForm> GetContactFormByCompanyName(string keyword, string companyName, int currentpage, int pagesize)
        {
            return r.GetContactFormByCompanyName(keyword, companyName, currentpage, pagesize);
        }
        public List<IdentityContactForm> GetAllCompany(IdentityContactForm identity, int currentpage, int pagesize)
        {
            return r.GetAllCompany(identity, currentpage, pagesize);
        }
        public IdentityAllowance InsertAllowance(IdentityAllowance identity)
        {
            return r.InsertAllowance(identity);
        }
        public IdentityAllowanceDetail InsertAllowanceDetail(IdentityAllowanceDetail identity)
        {
            return r.InsertAllowanceDetail(identity);
        }
        public IdentityContactForm InsertContactForm(IdentityContactForm identity)
        {
            return r.InsertContactForm(identity);
        }
        public IdentityContactForm DeleteContactForm(int formId)
        {
            return r.DeleteContactForm(formId);
        }
        public List<IdentityDependent> GetDependentsByFormId(int formId)
        {
            return r.GetDependentsByFormId(formId);
        }
        public IdentityAllowance GetAllowanceByFormId(int formId)
        {
            return r.GetAllowanceByFormId(formId);
        }
        public IdentityAllowanceDetail GetAllowanceDetailByAllowanceId(int allowanceId)
        {
            return r.GetAllowanceDetailByAllowanceId(allowanceId);
        }
        public IdentityContactForm GetContactFormByFormId(int formId)
        {
            return r.GetContactFormByFormId(formId);
        }

        public List<IdentityContactForm> GetContactFormByEmployeeId(int id)
        {
            return r.GetContactFormByEmployeeId(id);
        }
    }
}
