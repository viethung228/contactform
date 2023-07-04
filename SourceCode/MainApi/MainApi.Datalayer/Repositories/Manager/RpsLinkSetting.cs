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
    public class RpsLinkSetting
    {
        private readonly string _conStr;

        public RpsLinkSetting(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsLinkSetting()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }
        #region Common
        public static IdentityLinkSetting ExtractLinkSettingData(IDataReader reader)
        {
            var record = new IdentityLinkSetting();

            //Seperate properties
            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.SettingName = reader["SettingName"].ToString();
            record.Link = reader["Link"].ToString();
            record.Type = Utils.ConvertToBoolean(reader["Type"]);
            record.UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? (DateTime)reader["UpdatedDate"] : DateTime.MinValue;
            record.TotalCount = !reader.HasColumn("TotalCount") || reader["TotalCount"] == null ? 0 : Utils.ConvertToInt32(reader["TotalCount"]);
            return record;
        }
        #endregion
        public IdentityLinkSetting GetById(int id)
        {
            //Common syntax           
            var sqlCmd = @"LinkSetting_GetById";
            var info = new IdentityLinkSetting();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@id", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractLinkSettingData(returnObj);
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

        public IdentityLinkSetting GetLinkByName(string settingName)
        {
            //Common syntax           
            var sqlCmd = @"LinkSetting_GetLinkByName";
            var info = new IdentityLinkSetting();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@SettingName", settingName}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractLinkSettingData(returnObj);
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

        public IdentityLinkSetting Update(int id, string link, string settingName, bool? type)
        {
            //Common syntax           
            var sqlCmd = @"LinkSetting_Update";
            var info = new IdentityLinkSetting();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@id", id},
                {"@link",link },
                {"@settingName", settingName},
                {"@type",type }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractLinkSettingData(returnObj);
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

        public IdentityLinkSetting Insert(string settingName, bool? type, string link)
        {
            //Common syntax           
            var sqlCmd = @"LinkSetting_Update";
            var info = new IdentityLinkSetting();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@id", null},
                {"@settingName", settingName},
                {"@type",type },
                {"@link",link }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractLinkSettingData(returnObj);
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

        public List<IdentityLinkSetting> GetByPage(IdentityLinkSetting identity, int currentpage, int pagesize)
        {
            //Common syntax           
            var sqlCmd = @"LinkSetting_GetByPage";
            var info = new List<IdentityLinkSetting>();
            int offset = (currentpage - 1) * pagesize;
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", identity.Keyword},
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
                            var record = ExtractLinkSettingData(returnObj);

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
