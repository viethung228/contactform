using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Entities.Entities.Business;
using MainApi.DataLayer.Repositories.Manager;
using MainApi.SharedLibs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApi.DataLayer.Stores
{
    public interface IStoreLinkSetting
    {
        IdentityLinkSetting GetById(int id);
        IdentityLinkSetting GetLinkByName(string settingName);
        List<IdentityLinkSetting> GetByPage(IdentityLinkSetting identity, int currentpage, int pagesize);
        IdentityLinkSetting Update(int id, string link, string settingName, bool? type);
        IdentityLinkSetting Insert(string settingName, bool? type, string link);
    }
    public class StoreLinkSetting : IStoreLinkSetting
    {
        private readonly string _conStr;
        RpsLinkSetting r;

        public StoreLinkSetting() : this("MainDBConn")
        {

        }

        public StoreLinkSetting(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            r = new RpsLinkSetting(_conStr);
        }

        public IdentityLinkSetting Update(int id, string link, string settingName, bool? type)
        {
            return r.Update(id, link, settingName, type);
        }
        public IdentityLinkSetting Insert(string settingName, bool? type, string link)
        {
            return r.Insert(settingName, type, link);
        }
        public List<IdentityLinkSetting> GetByPage(IdentityLinkSetting identity, int currentpage, int pagesize)
        {
            return r.GetByPage(identity, currentpage, pagesize);
        }
        public IdentityLinkSetting GetById(int id)
        {
            return r.GetById(id);
        }

        public IdentityLinkSetting GetLinkByName(string settingName)
        {
            return r.GetLinkByName(settingName);
        }
    }
}
