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
    public interface IStoreSkyflag
    {
        bool Update(IdentitySkyflag identity);
        IdentitySkyflag GetById(string id);
        IdentitySkyflag Insert(IdentitySkyflag identity);
    }
    public class StoreSkyflag : IStoreSkyflag
    {
        private readonly string _conStr;
        RpsSkyflag r;

        public StoreSkyflag() : this("MainDBConn")
        {

        }

        public StoreSkyflag(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            r = new RpsSkyflag(_conStr);
        }

        public bool Update(IdentitySkyflag identity)
        {
            return r.Update(identity);
        }
        public IdentitySkyflag GetById(string id)
        {
            return r.GetById(id);
        }
        public IdentitySkyflag Insert(IdentitySkyflag identity)
        {
            return r.Insert(identity);
        }
    }
}
