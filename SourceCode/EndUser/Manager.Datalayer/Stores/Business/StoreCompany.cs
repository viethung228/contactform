using MainApi.DataLayer.Entities;
using Manager.Datalayer.Repositories;
using Manager.SharedLibs;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Manager.DataLayer;
using MainApi.DataLayer;

namespace Manager.DataLayer.Stores
{
    public interface IStoreCompany
    {
        List<IdentityCompany> GetByPage(IdentityCompany filter, int currentPage, int pageSize);
        string Insert(IdentityCompany identity);
        bool Update(IdentityCompany identity);
        bool ChangePassword(IdentityCompany identity);
        bool UpdateAvatar(IdentityCompany identity);
        bool Delete(int id);
        bool LockAccount(IdentityCompany identity);
        bool UnLockAccount(IdentityCompany identity);
        IdentityCompany GetById(string id);
        IdentityCompany GetByCompanyName(string userName);
        IdentityCompany GetByStaffId(int id);
        IdentityCompany Login(IdentityCompany identity);

        List<IdentityPermission> GetPermissionsByCompany(string userId);
        List<IdentityMenu> GetRootMenuByCompanyId(string userId);
        List<IdentityMenu> GetChildMenuByCompanyId(string userId, int parentId);
        List<IdentityMenu> GetAllDislayMenu();
    }

    public class StoreCompany : IStoreCompany
    {
        private readonly string _conStr;
        private RpsCompany m;

        public StoreCompany() : this("MainDBConn")
        {

        }

        public StoreCompany(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            m = new RpsCompany(_conStr);
        }

        #region  Common

        public List<IdentityCompany> GetByPage(IdentityCompany filter, int currentPage, int pageSize)
        {
            return m.GetByPage(filter, currentPage, pageSize);
        }

        public string Insert(IdentityCompany identity)
        {
            return m.Insert(identity);
        }

        public bool Update(IdentityCompany identity)
        {
            return m.Update(identity);
        }

        public bool ChangePassword(IdentityCompany identity)
        {
            return m.ChangePassword(identity);
        }

        public bool UpdateAvatar(IdentityCompany identity)
        {
            return m.UpdateAvatar(identity);
        }

        public bool LockAccount(IdentityCompany identity)
        {
            return m.LockAccount(identity);
        }

        public bool UnLockAccount(IdentityCompany identity)
        {
            return m.UnLockAccount(identity);
        }

        public IdentityCompany GetByStaffId(int id)
        {
            return m.GetByCompanyId(id);
        }

        public IdentityCompany GetById(string id)
        {
            return m.GetById(id);
        }

        public IdentityCompany GetByCompanyName(string userName)
        {
            return m.GetByCompanyName(userName);
        }

        public bool Delete(int id)
        {
            return m.Delete(id);
        }

        public IdentityCompany Login(IdentityCompany identity)
        {
            return m.Login(identity);
        }

        public List<IdentityPermission> GetPermissionsByCompany(string userId)
        {
            return m.GetPermissionsByCompany(userId);
        }

        public List<IdentityMenu> GetRootMenuByCompanyId(string userId)
        {
            return m.GetRootMenuByCompanyId(userId);
        }

        public List<IdentityMenu> GetChildMenuByCompanyId(string userId, int parentId)
        {
            return m.GetChildMenuByCompanyId(userId, parentId);
        }

        public List<IdentityMenu> GetAllDislayMenu()
        {
            return m.GetAllDislayMenu();
        }
       
        #endregion
    }
}
