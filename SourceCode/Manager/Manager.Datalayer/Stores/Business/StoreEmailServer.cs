using MainApi.DataLayer.Entities;
using Manager.DataLayer.Repositories;
using Manager.SharedLibs;
using MsSql.AspNet.Identity.Entities;
using System;
using System.Collections.Generic;

namespace Manager.DataLayer.Stores
{
    public interface IStoreEmailServer
    {
        List<IdentityEmailServer> GetByPage(dynamic filter);

        List<IdentityEmailServer> GetListByAgency(int agencyId);

        IdentityEmailServer GetById(int id);

        int Insert(IdentityEmailServer identity);

        int Update(IdentityEmailServer identity);

        int Delete(IdentityEmailServer identity);

        IdentityEmailServer GetDetailById(int id);

        List<IdentityEmailSetting> GetEmailSettingsByStaff(int agencyId, int staffId);

        bool UpdateEmailSettings(List<IdentityEmailSetting> settings);

        int UpdateInboxCount(IdentityEmailSetting identity);
        int UpdateSentCount(IdentityEmailSetting identity);
    }

    public class StoreEmailServer : IStoreEmailServer
    {
        private readonly string _conStr;
        private RpsEmailServer m;

        public StoreEmailServer() : this("MainDBConn")
        {

        }

        public StoreEmailServer(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            m = new RpsEmailServer(_conStr);
        }

        #region --- EmailServer ---
        public List<IdentityEmailServer> GetByPage(dynamic filter)
        {
            return m.GetByPage(filter);
        }

        public List<IdentityEmailServer> GetListByAgency(int agencyId)
        {
            return m.GetListByAgency(agencyId);
        }

        public IdentityEmailServer GetById(int id)
        {
            return m.GetById(id);
        }

        public int Insert(IdentityEmailServer identity)
        {
            return m.Insert(identity);
        }

        public int Update(IdentityEmailServer identity)
        {
            return m.Update(identity);
        }

        public int Delete(IdentityEmailServer identity)
        {
            return m.Delete(identity);
        }

        public IdentityEmailServer GetDetailById(int id)
        {
            return m.GetDetailById(id);
        }

        public List<IdentityEmailSetting> GetEmailSettingsByStaff(int agencyId, int staffId)
        {
            return m.GetEmailSettingsByStaff(agencyId, staffId);
        }

        public bool UpdateEmailSettings(List<IdentityEmailSetting> settings)
        {
            return m.UpdateEmailSettings(settings);
        }

        public int UpdateInboxCount(IdentityEmailSetting identity)
        {
            return m.UpdateInboxCount(identity);
        }

        public int UpdateSentCount(IdentityEmailSetting identity)
        {
            return m.UpdateSentCount(identity);
        }

        #endregion
    }
}
