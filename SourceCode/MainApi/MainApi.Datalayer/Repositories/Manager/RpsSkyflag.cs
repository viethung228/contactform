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
    public class RpsSkyflag
    {
        private readonly string _conStr;

        public RpsSkyflag(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsSkyflag()
        {
            _conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        #region Common
        public static IdentitySkyflag ExtractWebhookData(IDataReader reader)
        {
            var record = new IdentitySkyflag();

            //Seperate properties
            record.suid = reader["suid"].ToString();
            record.spram1 = reader["spram1"].ToString();
            record.spram2 = reader["spram2"].ToString();
            record.xad = Utils.ConvertToInt32(reader["xad"]);
            record.approve_date = Convert.ToDateTime(reader["approve_date"]);
            record.cv_date = Convert.ToDateTime(reader["cv_date"]);
            record.price = Utils.ConvertToInt32(reader["price"]);
            record.cv_id = Utils.ConvertToInt32(reader["cv_id"]);
            record.wallpoint = Utils.ConvertToInt32(reader["wallpoint"]);
            record.mcv_no = Utils.ConvertToInt32(reader["mcv_no"]);
            record.display_name = reader["display_name"].ToString();
            record.OS = reader["OS"].ToString();
            return record;
        }
        #endregion
        public IdentitySkyflag Insert(IdentitySkyflag identity)
        {
            //Common syntax           
            var sqlCmd = @"Skyflag_Insert";
            var info = new IdentitySkyflag();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@suid", identity.suid},
                {"@spram1", identity.spram1 },
                {"@spram2", identity.spram2 },
                {"@xad", identity.xad },
                {"@approve_date", DateTime.UtcNow },
                {"@cv_date", identity.cv_date },
                {"@price", identity.price },
                {"@cv_id", identity.cv_id },
                {"@wallpoint", identity.wallpoint },
                {"@mcv_no", identity.mcv_no == 0 ?null:identity.mcv_no },
                {"@display_name", identity.display_name },
                {"@OS", identity.OS }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractWebhookData(returnObj);
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
        public bool Update(IdentitySkyflag identity)
        {
            //Common syntax           
            var sqlCmd = @"Skyflag_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@suid", identity.suid},
                {"@spram1", identity.spram1 },
                {"@spram2", identity.spram2 },
                {"@xad", identity.xad },
                {"@approve_date", identity.approve_date },
                {"@cv_date", identity.cv_date },
                {"@price", identity.price },
                {"@cv_id", identity.cv_id },
                {"@wallpoint", identity.wallpoint },
                {"@mcv_no", identity.mcv_no },
                {"@display_name", identity.display_name },
                {"@OS", identity.OS }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);

                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return true;
        }
        public IdentitySkyflag GetById(string id)
        {
            //Common syntax           
            var sqlCmd = @"Skyflag_GetBySuid";
            var info = new IdentitySkyflag();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@suid", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractWebhookData(returnObj);
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
