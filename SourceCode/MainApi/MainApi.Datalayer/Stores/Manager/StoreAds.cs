using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Entities.Entities.Business;
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
    public interface IStoreAds
    {
        IdentityAds RemoveAds(string id);
        IdentityAds GetStatus(string id);

    }
    public class StoreAds : IStoreAds
    {
        private readonly string _conStr;
        RpsAds r;

        public StoreAds() : this("MainDBConn")
        {

        }

        public StoreAds(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            r = new RpsAds(_conStr);
        }

        public IdentityAds RemoveAds(string id)
        {
            return r.RemoveAds(id);
        }
        public IdentityAds GetStatus(string id)
        {
            return r.GetStatus(id);
        }

    }
}
