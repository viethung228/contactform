using MainApi.DataLayer.Entities.Entities.Business;
using MainApi.DataLayer.Repositories.Manager;
using MainApi.SharedLibs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApi.DataLayer.Stores.Manager
{
    public interface IStoreApplovin
    {
        IdentityApplovin Insert(IdentityApplovin identity);
    }
    public class StoreApplovin : IStoreApplovin
    {
        private readonly string _conStr;
        RpsApplovin r;

        public StoreApplovin() : this("MainDBConn")
        {

        }

        public StoreApplovin(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            r = new RpsApplovin(_conStr);
        }

        public IdentityApplovin Insert(IdentityApplovin identity)
        {
            return r.Insert(identity);
        }
    }
}
