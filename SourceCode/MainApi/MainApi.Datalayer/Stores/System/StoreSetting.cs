﻿using MainApi.DataLayer.Entities;
using Manager.Datalayer.Repositories;
using MainApi.SharedLibs;
using System.Collections.Generic;

namespace MainApi.DataLayer.Stores
{
    public interface IStoreSetting
    {
        List<IdentitySetting> LoadSettings(string settingType);
        bool SaveSettings(List<IdentitySetting> list);
    }

    public class StoreSetting : IStoreSetting
    {
        private readonly string _conStr;
        private RpsSetting m;

        public StoreSetting() : this("MainDBConn")
        {

        }

        public StoreSetting(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            m = new RpsSetting(_conStr);
        }

        #region  Common

        public List<IdentitySetting> LoadSettings(string settingType)
        {
            return m.LoadSettings(settingType);
        }
       
        public bool SaveSettings(List<IdentitySetting> list)
        {
            return m.SaveSettings(list);
        }
        
        #endregion        
    }
}
