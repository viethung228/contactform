using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Repositories;
using MainApi.SharedLibs;
using System.Collections.Generic;

namespace MainApi.DataLayer.Stores
{
    public interface IStorePrefecture
    {
        List<IdentityPrefecture> GetByPage(IdentityPrefecture filter, int currentPage, int pageSize);
        int Insert(IdentityPrefecture identity);
        bool Update(IdentityPrefecture identity);
        IdentityPrefecture GetById(int id);
        bool Delete(int id);
        List<IdentityPrefecture> GetList();
        List<IdentityPrefecture> GetListByRegion(int regionId);
        List<IdentityPrefecture> GetListByCountry(int regionId);
    }

    public class StorePrefecture : IStorePrefecture
    {
        private readonly string _conStr;
        private RpsPrefecture m;

        public StorePrefecture() : this("MainDBConn")
        {

        }

        public StorePrefecture(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            m = new RpsPrefecture(_conStr);
        }

        public bool Delete(int id)
        {
            return m.Delete(id);
        }

        public IdentityPrefecture GetById(int id)
        {
            return m.GetById(id);
        }

        public List<IdentityPrefecture> GetByPage(IdentityPrefecture filter, int currentPage, int pageSize)
        {
            return m.GetByPage(filter, currentPage, pageSize);
        }

        public List<IdentityPrefecture> GetList()
        {
            return m.GetList();
        }

        public List<IdentityPrefecture> GetListByCountry(int countryId)
        {
            return m.GetListByCountry(countryId);
        }

        public List<IdentityPrefecture> GetListByRegion(int regionId)
        {
            return m.GetListByRegion(regionId);
        }

        public int Insert(IdentityPrefecture identity)
        {
            return m.Insert(identity);
        }

        public bool Update(IdentityPrefecture identity)
        {
            return m.Update(identity);
        }
    }
}
