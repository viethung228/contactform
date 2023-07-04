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
    public interface IStoreRevenue
    {
        List<IdentityRevenue> GetHistoryAll(IdentityRevenue identity, int pagesize, int currentpage);
        List<IdentityRevenue> GetHistoryById(string id, int pagesize, int currentpage);

    }
    public class StoreRevenue : IStoreRevenue
    {
        private readonly string _conStr;
        RpsRevenue r;

        public StoreRevenue() : this("MainDBConn")
        {

        }

        public StoreRevenue(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            r = new RpsRevenue(_conStr);
        }

        public List<IdentityRevenue> GetHistoryAll(IdentityRevenue identity, int pagesize, int currentpage)
        {
            return r.GetHistoryAll(identity, pagesize, currentpage);
        }
        public List<IdentityRevenue> GetHistoryById(string id, int pagesize, int currentpage)
        {
            return r.GetHistoryById(id, pagesize, currentpage);
        }


    }
}
