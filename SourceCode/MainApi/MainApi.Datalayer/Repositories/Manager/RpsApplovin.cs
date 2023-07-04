using MainApi.DataLayer.Entities;
using MainApi.DataLayer.Entities.Entities.Business;
using MainApi.SharedLibs;
using MainApi.SharedLibs.Extensions;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApi.DataLayer.Repositories.Manager
{
    public class RpsApplovin
    {
        private readonly string _conStr;

        public RpsApplovin(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsApplovin()
        {
            _conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        #region Common
        public static IdentityApplovin ExtractApplovinData(IDataReader reader)
        {
            var record = new IdentityApplovin();

            //Seperate properties
            record.ad_unit_id = reader["ad_unit_id"].ToString();
            record.ad_unit_name = reader["ad_unit_name"].ToString();
            record.amount = Utils.ConvertToInt32(reader["amount"]);

            record.cc = reader["cc"].ToString();
            record.currency = reader["currency"].ToString();
            record.custom_data = reader["custom_data"].ToString();
            record.event_id = reader["event_id"].ToString();
            record.event_token = reader["event_token"].ToString();
            record.event_token_all = reader["event_token_all"].ToString();
            record.idfa = reader["idfa"].ToString();
            record.idfv = reader["idfv"].ToString();
            record.ip = reader["ip"].ToString();
            record.network_name = reader["network_name"].ToString();
            record.package_name = reader["package_name"].ToString();
            record.placement = reader["placement"].ToString();
            record.platform = reader["platform"].ToString();
            record.user_id = reader["user_id"].ToString();
            record.ts = Convert.ToDateTime(reader["ts"]);
            return record;
        }
        #endregion
        public IdentityApplovin Insert(IdentityApplovin identity)
        {
            //Common syntax           
            var sqlCmd = @"Applovin_Insert";
            var info = new IdentityApplovin();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@idfa", identity.idfa},
                {"@user_id", identity.user_id },
                {"@event", identity.event_id },
                {"@token", identity.event_token },
                {"@ts", identity.ts },
                {"@amount", identity.amount }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractApplovinData(returnObj);
                        }
                        else info = null;
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }
    }
}
