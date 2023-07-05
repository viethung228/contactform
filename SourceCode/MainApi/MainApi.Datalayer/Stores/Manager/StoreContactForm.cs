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
    }
}
