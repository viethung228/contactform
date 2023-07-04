using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Repositories;
using MainApi.SharedLibs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApi.DataLayer.Stores
{
    public interface IStoreRegion
    {
        List<IdentityRegion> GetByPage(IdentityRegion filter, int currentPage, int pageSize);
        int Insert(IdentityRegion identity);
        bool Update(IdentityRegion identity);
        IdentityRegion GetById(int id);
        bool Delete(int id);
        List<IdentityRegion> GetList(int countryId);
    }

    public class StoreRegion : IStoreRegion
    {
        private readonly string _conStr;
        private RpsRegion m;

        public StoreRegion() : this("MainDBConn")
        {

        }

        public StoreRegion(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            m = new RpsRegion(_conStr);
        }

        public bool Delete(int id)
        {
            return m.Delete(id);
        }

        public IdentityRegion GetById(int id)
        {
            return m.GetById(id);
        }

        public List<IdentityRegion> GetByPage(IdentityRegion filter, int currentPage, int pageSize)
        {
            return m.GetByPage(filter, currentPage, pageSize);
        }

        public List<IdentityRegion> GetList(int countryId)
        {
            return m.GetList(countryId);
        }

        public int Insert(IdentityRegion identity)
        {
            return m.Insert(identity);
        }

        public bool Update(IdentityRegion identity)
        {
            return m.Update(identity);
        }
    }
}
