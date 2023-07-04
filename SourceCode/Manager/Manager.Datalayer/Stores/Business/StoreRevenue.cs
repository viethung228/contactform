using MainApi.DataLayer.Entities;
using Manager.DataLayer.Repositories;
using Manager.SharedLibs;
using MsSql.AspNet.Identity.Entities;
using System;
using System.Collections.Generic;

namespace Manager.DataLayer.Stores
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
