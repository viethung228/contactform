using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Repositories.Manager;
using MainApi.SharedLibs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
        List<IdentityContactForm> GetEmployeeByCompanyName(string companyName, int currentpage, int pagesize);
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

        public IdentityDependent InsertDependent(IdentityDependent identity)
        {
            return r.InsertDependent(identity);
        }
        public List<IdentityContactForm> GetEmployeeByCompanyName(string companyName, int currentpage, int pagesize)
        {
            return r.GetEmployeeByCompanyName(companyName, currentpage, pagesize);
        }
        public List<IdentityContactForm> GetAllCompany(IdentityContactForm identity, int currentpage, int pagesize)
        {
            return r.GetAllCompany(identity,currentpage,pagesize);
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
