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
    public class RpsRevenue
    {
        private readonly string _conStr;

        public RpsRevenue(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsRevenue()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }
        #region Common
        public static IdentityRevenue ExtractRevenueData(IDataReader reader)
        {
            var record = new IdentityRevenue();

            //Seperate properties
            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.UserId = reader["UserId"].ToString();
            record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);
            record.TotalRevenue = Utils.ConvertToInt32(reader["TotalRevenue"]);
            record.Price = Convert.ToSingle(reader["Price"]);
            record.Coin = Utils.ConvertToInt32(reader["Coin"]);
            record.CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime)reader["CreatedDate"] : DateTime.MinValue;
            record.SourceType = Utils.ConvertToInt32(reader["SourceType"]);
            return record;
        }

        #endregion
        public List<IdentityRevenue> GetHistoryAll(IdentityRevenue identity, int pagesize, int currentpage)
        {
            //Common syntax           
            var sqlCmd = @"Revenue_GetHistoryAll";
            var info = new List<IdentityRevenue>();
            int offset = (currentpage - 1) * pagesize;
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Offset", offset},
                {"@PageSize", pagesize },
                {"@FromDate", identity.FromDate },
                {"@ToDate", identity.ToDate },
                {"@SourceType", identity.SourceType },
                {"@UserId", identity.UserId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (returnObj.Read())
                        {
                            //Get common information
                            var record = ExtractRevenueData(returnObj);
                            info.Add(record);
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

        public List<IdentityRevenue> GetHistoryById(string id, int pagesize, int currentpage)
        {
            //Common syntax           
            var sqlCmd = @"Revenue_GetHistoryById";
            var info = new List<IdentityRevenue>();
            int offset = (currentpage - 1) * pagesize;
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserId", id},
                {"@Offset", offset},
                {"@PageSize", pagesize }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (returnObj.Read())
                        {
                            //Get common information
                            var record = ExtractRevenueData(returnObj);
                            info.Add(record);
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
