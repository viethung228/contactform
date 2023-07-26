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
    public interface IStoreUser
    {
        List<IdentityUser> GetByPage(IdentityUser filter, int currentPage, int pageSize);
        string Insert(IdentityUser identity);
        List<string> GetAllUserNameHasRevenueHistory();
        bool Update(IdentityUser identity);
        bool ChangePassword(IdentityUser identity);
        bool UpdateAvatar(IdentityUser identity);
        bool Delete(int id);
        bool LockAccount(IdentityUser identity);
        bool UnLockAccount(IdentityUser identity);
        IdentityUser GetById(string id);
        IdentityUser GetByUserName(string userName);
        IdentityUser GetByStaffId(int id);
        IdentityUser Login(IdentityUser identity);

        List<IdentityPermission> GetPermissionsByUser(string userId);
        List<IdentityMenu> GetRootMenuByUserId(string userId);
        List<IdentityMenu> GetChildMenuByUserId(string userId, int parentId);
        List<IdentityMenu> GetAllDislayMenu();
        List<IdentityUser> GetActiveUsersByParent(int parentId);

        bool UpdateQrcode(int staffId, string imagePath);
    }

    public class StoreUser : IStoreUser
    {
        private readonly string _conStr;
        private RpsUser m;

        public StoreUser() : this("MainDBConn")
        {

        }

        public StoreUser(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            m = new RpsUser(_conStr);
        }

        #region  Common

        public List<IdentityUser> GetByPage(IdentityUser filter, int currentPage, int pageSize)
        {
            return m.GetByPage(filter, currentPage, pageSize);
        }

        public string Insert(IdentityUser identity)
        {
            return m.Insert(identity);
        }
        public List<string> GetAllUserNameHasRevenueHistory()
        {
            return m.GetAllUserNameHasRevenueHistory();
        }

        public bool Update(IdentityUser identity)
        {
            return m.Update(identity);
        }

        public bool ChangePassword(IdentityUser identity)
        {
            return m.ChangePassword(identity);
        }

        public bool UpdateAvatar(IdentityUser identity)
        {
            return m.UpdateAvatar(identity);
        }

        public bool LockAccount(IdentityUser identity)
        {
            return m.LockAccount(identity);
        }

        public bool UnLockAccount(IdentityUser identity)
        {
            return m.UnLockAccount(identity);
        }

        public IdentityUser GetByStaffId(int id)
        {
            return m.GetByStaffId(id);
        }

        public IdentityUser GetById(string id)
        {
            return m.GetById(id);
        }

        public IdentityUser GetByUserName(string userName)
        {
            return m.GetByUserName(userName);
        }

        public bool Delete(int id)
        {
            return m.Delete(id);
        }

        public IdentityUser Login(IdentityUser identity)
        {
            return m.Login(identity);
        }

        public List<IdentityPermission> GetPermissionsByUser(string userId)
        {
            return m.GetPermissionsByUser(userId);
        }

        public List<IdentityMenu> GetRootMenuByUserId(string userId)
        {
            return m.GetRootMenuByUserId(userId);
        }

        public List<IdentityMenu> GetChildMenuByUserId(string userId, int parentId)
        {
            return m.GetChildMenuByUserId(userId, parentId);
        }

        public List<IdentityMenu> GetAllDislayMenu()
        {
            return m.GetAllDislayMenu();
        }

        public List<IdentityUser> GetActiveUsersByParent(int parentId)
        {
            return m.GetActiveUsersByParent(parentId);
        }

        public bool UpdateQrcode(int staffId, string imagePath)
        {
            return m.UpdateQrcode(staffId, imagePath);
        }
        #endregion
    }
}
