﻿using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Repositories;
using MainApi.SharedLibs;
using System.Collections.Generic;
using System.Configuration;

namespace MainApi.DataLayer.Stores
{
    public interface IStoreNotification
    {
        IdentityNotification GetById(int id);
        bool Delete(int id);

        int SinglePush(IdentityNotification identity);
        int MultiplePush(List<int> listIds, IdentityNotification identity);
        List<IdentityNotification> GetByUser(dynamic filter, int currentPage, int pageSize);
        int CountUnread(dynamic filter);
        bool MarkIsRead(IdentityNotification identity);

        #region Notification warning

        IdentityNotificationWarning GetLastWarning(IdentityNotificationWarning identity);
        int WarningInsert(IdentityNotificationWarning identity);

        #endregion
    }

    public class StoreNotification : IStoreNotification
    {
        private readonly string _connStr;
        private RpsNotification m;

        public StoreNotification() : this("MainDBConn")
        {

        }

        public StoreNotification(string connectionStringName)
        {
            _connStr = AppConfiguration.GetAppsetting(connectionStringName);
            m = new RpsNotification(_connStr);
        }

        #region  Common
        public IdentityNotification GetById(int id)
        {
            return m.GetById(id);
        }

        public bool Delete(int id)
        {
            return m.Delete(id);
        }

        public int SinglePush(IdentityNotification identity)
        {
            return m.SinglePush(identity);
        }

        public int MultiplePush(List<int> listIds, IdentityNotification identity)
        {
            return m.MultiplePush(listIds, identity);
        }

        public List<IdentityNotification> GetByUser(dynamic filter, int currentPage, int pageSize)
        {
            return m.GetByUser(filter, currentPage, pageSize);
        }

        public int CountUnread(dynamic identity)
        {
            return m.CountUnread(identity);
        }

        public bool MarkIsRead(IdentityNotification identity)
        {
            return m.MarkIsRead(identity);
        }

        #endregion

        #region Notification warning

        public IdentityNotificationWarning GetLastWarning(IdentityNotificationWarning identity)
        {
            return m.GetLastWarning(identity);
        }

        public int WarningInsert(IdentityNotificationWarning identity)
        {
            return m.WarningInsert(identity);
        }

        #endregion
    }
}
