using MainApi.DataLayer.Entities;
using Manager.Datalayer.Repositories;
using MainApi.SharedLibs;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MainApi.DataLayer;

namespace MainApi.DataLayer.Stores
{
    public interface IStoreUser
    {
        List<IdentityUser> GetByPage(IdentityUser filter, int currentPage, int pageSize);
        string Insert(IdentityUser identity);
        bool Update(IdentityUser identity);
        bool ConfirmEmail(int id);
        bool ChangePassword(IdentityUser identity);
        bool UpdateAvatar(IdentityUser identity);
        bool Delete(int id);
        bool LockAccount(IdentityUser identity);
        bool UnLockAccount(IdentityUser identity);
        IdentityUser GetById(string id);
        IdentityUser GetByStaffId(int id);
        IdentityUser GetByUserName(string userName);
        IdentityUser GetByEmail(string email);
        IdentityUser GetDetail(int id);
        IdentityUser Login(IdentityUser identity);
        List<IdentityUser> GetList();
        List<IdentityPermission> GetPermissionsByUser(string userId);
        List<IdentityMenu> GetRootMenuByUserId(string userId);
        List<IdentityMenu> GetChildMenuByUserId(string userId, int parentId);
        List<IdentityMenu> GetAllDislayMenu();

        List<IdentityUser> GetUsersByPermission(int agencyId, string ationName, string controllerName);
        List<IdentityUser> GetListReceiveAllUpdate(int agencyId);
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

        public List<IdentityUser> GetList()
        {
            return m.GetList();
        }
        public string Insert(IdentityUser identity)
        {
            return m.Insert(identity);
        }

        public bool Update(IdentityUser identity)
        {
            return m.Update(identity);
        }

        public bool ConfirmEmail(int id)
        {
            return m.ConfirmEmail(id);
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

        public IdentityUser GetDetail(int id)
        {
            return m.GetDetail(id);
        }

        public IdentityUser GetById(string id)
        {
            return m.GetById(id);
        }
        public IdentityUser GetByEmail(string email)
        {
            return m.GetByEmail(email);
        }
        public IdentityUser GetByStaffId(int id)
        {
            return m.GetByStaffId(id);
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

        public List<IdentityUser> GetUsersByPermission(int agencyId, string actionName, string controllerName)
        {
            return m.GetUsersByPermission(agencyId, actionName, controllerName);
        }

        public List<IdentityUser> GetListReceiveAllUpdate(int agencyId)
        {
            return m.GetListReceiveAllUpdate(agencyId);
        }

        #endregion        
    }
}
