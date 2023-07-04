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
    public class RpsAds
    {
        private readonly string _conStr;

        public RpsAds(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsAds()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }
        #region Common
        public static IdentityAds ExtractAdsData(IDataReader reader)
        {
            var record = new IdentityAds();

            //Seperate properties
            record.Id= Utils.ConvertToInt32(reader["Id"]);
            record.UserId =reader["UserId"].ToString();
            record.RemoveAds = Utils.ConvertToBoolean(reader["RemoveAds"]);
            record.UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? (DateTime)reader["UpdatedDate"] : DateTime.MinValue;

            return record;
        }
       
        #endregion
        public IdentityAds RemoveAds(string id)
        {
            //Common syntax           
            var sqlCmd = @"Ads_RemoveAds";
            var info = new IdentityAds();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserId", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractAdsData(returnObj);
                        }
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
        public IdentityAds GetStatus(string id)
        {
            //Common syntax           
            var sqlCmd = @"Ads_GetStatus";
            var info = new IdentityAds();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserId", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractAdsData(returnObj);
                        }
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
