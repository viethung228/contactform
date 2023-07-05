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
    public interface IStoreEmployee
    {
        List<IdentityEmployee> GetByPage(IdentityEmployee filter, int currentPage, int pageSize);
        string Insert(IdentityEmployee identity);
        List<string> GetAllEmployeeNameHasRevenueHistory();
        bool Update(IdentityEmployee identity);
        bool ChangePassword(IdentityEmployee identity);
        bool UpdateAvatar(IdentityEmployee identity);
        bool Delete(int id);
        bool LockAccount(IdentityEmployee identity);
        bool UnLockAccount(IdentityEmployee identity);
        IdentityEmployee GetById(string id);
        IdentityEmployee GetByEmployeeName(string userName);
        IdentityEmployee GetByStaffId(int id);
        IdentityEmployee Login(IdentityEmployee identity);

        List<IdentityPermission> GetPermissionsByEmployee(string userId);
        List<IdentityMenu> GetRootMenuByEmployeeId(string userId);
        List<IdentityMenu> GetChildMenuByEmployeeId(string userId, int parentId);
        List<IdentityMenu> GetAllDislayMenu();
        List<IdentityEmployee> GetActiveEmployeesByParent(int parentId);

        bool UpdateQrcode(int staffId, string imagePath);
    }

    public class StoreEmployee : IStoreEmployee
    {
        private readonly string _conStr;
        private RpsEmployee m;

        public StoreEmployee() : this("MainDBConn")
        {

        }

        public StoreEmployee(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            m = new RpsEmployee(_conStr);
        }

        #region  Common

        public List<IdentityEmployee> GetByPage(IdentityEmployee filter, int currentPage, int pageSize)
        {
            return m.GetByPage(filter, currentPage, pageSize);
        }

        public string Insert(IdentityEmployee identity)
        {
            return m.Insert(identity);
        }
        public List<string> GetAllEmployeeNameHasRevenueHistory()
        {
            return m.GetAllEmployeeNameHasRevenueHistory();
        }

        public bool Update(IdentityEmployee identity)
        {
            return m.Update(identity);
        }

        public bool ChangePassword(IdentityEmployee identity)
        {
            return m.ChangePassword(identity);
        }

        public bool UpdateAvatar(IdentityEmployee identity)
        {
            return m.UpdateAvatar(identity);
        }

        public bool LockAccount(IdentityEmployee identity)
        {
            return m.LockAccount(identity);
        }

        public bool UnLockAccount(IdentityEmployee identity)
        {
            return m.UnLockAccount(identity);
        }

        public IdentityEmployee GetByStaffId(int id)
        {
            return m.GetByStaffId(id);
        }

        public IdentityEmployee GetById(string id)
        {
            return m.GetById(id);
        }

        public IdentityEmployee GetByEmployeeName(string userName)
        {
            return m.GetByUserName(userName);
        }

        public bool Delete(int id)
        {
            return m.Delete(id);
        }

        public IdentityEmployee Login(IdentityEmployee identity)
        {
            return m.Login(identity);
        }

        public List<IdentityPermission> GetPermissionsByEmployee(string userId)
        {
            return m.GetPermissionsByEmployee(userId);
        }

        public List<IdentityMenu> GetRootMenuByEmployeeId(string userId)
        {
            return m.GetRootMenuByEmployeeId(userId);
        }

        public List<IdentityMenu> GetChildMenuByEmployeeId(string userId, int parentId)
        {
            return m.GetChildMenuByEmployeeId(userId, parentId);
        }

        public List<IdentityMenu> GetAllDislayMenu()
        {
            return m.GetAllDislayMenu();
        }

        public List<IdentityEmployee> GetActiveEmployeesByParent(int parentId)
        {
            return m.GetActiveEmployeesByParent(parentId);
        }

        public bool UpdateQrcode(int staffId, string imagePath)
        {
            return m.UpdateQrcode(staffId, imagePath);
        }
        #endregion
    }
}
