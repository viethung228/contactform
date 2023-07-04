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
    public class RpsCoin
    {
        private readonly string _conStr;

        public RpsCoin(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsCoin()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }
        #region Common
        public static IdentityCoin ExtractCoinData(IDataReader reader)
        {
            var record = new IdentityCoin();

            //Seperate properties
            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.userId = reader["UserId"].ToString();
            record.point = Utils.ConvertToInt32(reader["Point"]);

            return record;
        }
        public static CoinHistory ExtractHistoryCoinData(IDataReader reader)
        {
            var record = new CoinHistory();

            //Seperate properties
            record.ValueChange = reader["ValueChange"].ToString();
            record.CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime)reader["CreatedDate"] : DateTime.MinValue;
            record.SourceType = Utils.ConvertToInt32(reader["SourceType"]);
            record.TotalCount = reader["TotalCount"] == null ? 0 : Utils.ConvertToInt32(reader["TotalCount"]);

            return record;
        }
        #endregion
        public IdentityCoin GetPointById(string id)
        {
            //Common syntax           
            var sqlCmd = @"Coin_GetPointById";
            var info = new IdentityCoin();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@userId", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractCoinData(returnObj);
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
        public CoinHistory UpdateCoin(string userId, int valueChange, int sourceType)
        {
            //Common syntax           
            var sqlCmd = @"Coin_UpdatePoint";
            var info = new CoinHistory();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserId", userId},
                {"@ValueChange",valueChange },
                {"@SourceType",sourceType}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractHistoryCoinData(returnObj);
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
        public List<CoinHistory> GetHistoryPointById(string id, int currentpage, int pagesize)
        {
            //Common syntax           
            var sqlCmd = @"CoinHistory_GetByPage";
            var info = new List<CoinHistory>();
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
                            var record = ExtractHistoryCoinData(returnObj);

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
