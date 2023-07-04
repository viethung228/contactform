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
    public interface IStoreCoin
    {
        IdentityCoin GetPointById(string id);
        List<CoinHistory> GetHistoryPointById(string id, int currentpage, int pagesize);
        CoinHistory UpdateCoin(string userId, int valueChange, int sourceType);
    }
    public class StoreCoin : IStoreCoin
    {
        private readonly string _conStr;
        RpsCoin r;

        public StoreCoin() : this("MainDBConn")
        {

        }

        public StoreCoin(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            r = new RpsCoin(_conStr);
        }

        public IdentityCoin GetPointById(string id)
        {
            return r.GetPointById(id);
        }
        public CoinHistory UpdateCoin(string userId, int valueChange, int sourceType)
        {
            return r.UpdateCoin(userId, valueChange, sourceType);
        }
        public List<CoinHistory> GetHistoryPointById(string id, int currentpage, int pagesize)
        {
            return r.GetHistoryPointById(id, currentpage, pagesize);
        }
    }
}
