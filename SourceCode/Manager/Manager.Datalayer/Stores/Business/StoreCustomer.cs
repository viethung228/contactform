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
    public interface IStoreCustomer
    {
        List<IdentityCustomer> GetByPage(IdentityCustomer filter, int currentPage, int pageSize);
        string Insert(IdentityCustomer identity);
        List<string> GetAllCustomerNameHasRevenueHistory();
        bool Update(IdentityCustomer identity);
        bool ChangePassword(IdentityCustomer identity);
        bool UpdateAvatar(IdentityCustomer identity);
        bool Delete(int id);
        bool LockAccount(IdentityCustomer identity);
        bool UnLockAccount(IdentityCustomer identity);
        IdentityCustomer GetById(string id);
        IdentityCustomer GetByCustomerName(string userName);
        IdentityCustomer GetByStaffId(int id);
        IdentityCustomer Login(IdentityCustomer identity);

        List<IdentityPermission> GetPermissionsByCustomer(string userId);
        List<IdentityMenu> GetRootMenuByCustomerId(string userId);
        List<IdentityMenu> GetChildMenuByCustomerId(string userId, int parentId);
        List<IdentityMenu> GetAllDislayMenu();
        List<IdentityCustomer> GetActiveCustomersByParent(int parentId);

        bool UpdateQrcode(int staffId, string imagePath);
    }

    public class StoreCustomer : IStoreCustomer
    {
        private readonly string _conStr;
        private RpsCustomer m;

        public StoreCustomer() : this("MainDBConn")
        {

        }

        public StoreCustomer(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            m = new RpsCustomer(_conStr);
        }

        #region  Common

        public List<IdentityCustomer> GetByPage(IdentityCustomer filter, int currentPage, int pageSize)
        {
            return m.GetByPage(filter, currentPage, pageSize);
        }

        public string Insert(IdentityCustomer identity)
        {
            return m.Insert(identity);
        }
        public List<string> GetAllCustomerNameHasRevenueHistory()
        {
            return m.GetAllCustomerNameHasRevenueHistory();
        }

        public bool Update(IdentityCustomer identity)
        {
            return m.Update(identity);
        }

        public bool ChangePassword(IdentityCustomer identity)
        {
            return m.ChangePassword(identity);
        }

        public bool UpdateAvatar(IdentityCustomer identity)
        {
            return m.UpdateAvatar(identity);
        }

        public bool LockAccount(IdentityCustomer identity)
        {
            return m.LockAccount(identity);
        }

        public bool UnLockAccount(IdentityCustomer identity)
        {
            return m.UnLockAccount(identity);
        }

        public IdentityCustomer GetByStaffId(int id)
        {
            return m.GetByStaffId(id);
        }

        public IdentityCustomer GetById(string id)
        {
            return m.GetById(id);
        }

        public IdentityCustomer GetByCustomerName(string userName)
        {
            return m.GetByUserName(userName);
        }

        public bool Delete(int id)
        {
            return m.Delete(id);
        }

        public IdentityCustomer Login(IdentityCustomer identity)
        {
            return m.Login(identity);
        }

        public List<IdentityPermission> GetPermissionsByCustomer(string userId)
        {
            return m.GetPermissionsByCustomer(userId);
        }

        public List<IdentityMenu> GetRootMenuByCustomerId(string userId)
        {
            return m.GetRootMenuByCustomerId(userId);
        }

        public List<IdentityMenu> GetChildMenuByCustomerId(string userId, int parentId)
        {
            return m.GetChildMenuByCustomerId(userId, parentId);
        }

        public List<IdentityMenu> GetAllDislayMenu()
        {
            return m.GetAllDislayMenu();
        }

        public List<IdentityCustomer> GetActiveCustomersByParent(int parentId)
        {
            return m.GetActiveCustomersByParent(parentId);
        }

        public bool UpdateQrcode(int staffId, string imagePath)
        {
            return m.UpdateQrcode(staffId, imagePath);
        }
        #endregion
    }
}
