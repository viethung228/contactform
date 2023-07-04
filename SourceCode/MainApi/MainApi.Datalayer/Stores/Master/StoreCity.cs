using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Repositories;
using MainApi.SharedLibs;
using System.Collections.Generic;

namespace MainApi.DataLayer.Stores
{
    public interface IStoreCity
    {
        List<IdentityCity> GetByPage(dynamic filter, int currentPage, int pageSize);
        int Insert(IdentityCity identity);
        bool Update(IdentityCity identity);
        IdentityCity GetById(int id);
        bool Delete(int id);
        List<IdentityCity> GetList();
        List<IdentityCity> GetListByPrefecture(int prefectureId);
    }

    public class StoreCity : IStoreCity
    {
        private readonly string _conStr;
        private RpsCity m;

        public StoreCity() : this("MainDBConn")
        {

        }

        public StoreCity(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            m = new RpsCity(_conStr);
        }

        public bool Delete(int id)
        {
            return m.Delete(id);
        }

        public IdentityCity GetById(int id)
        {
            return m.GetById(id);
        }

        public List<IdentityCity> GetByPage(IdentityCity filter, int currentPage, int pageSize)
        {
            return m.GetByPage(filter, currentPage, pageSize);
        }

        public List<IdentityCity> GetByPage(dynamic filter, int currentPage, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        public List<IdentityCity> GetList()
        {
            return m.GetList();
        }

        public List<IdentityCity> GetListByPrefecture(int prefectureId)
        {
            return m.GetListByPrefecture(prefectureId);
        }

        public int Insert(IdentityCity identity)
        {
            return m.Insert(identity);
        }

        public bool Update(IdentityCity identity)
        {
            return m.Update(identity);
        }
    }
}
