using MainApi.DataLayer.Entities;
using MainApi.SharedLibs;
using MainApi.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MainApi.DataLayer.Repositories
{
    public class RpsCity
    {
        private readonly string _conStr;

        public RpsCity(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsCity()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        #region Common

        public List<IdentityCity> GetByPage(dynamic filter, int currentPage, int pageSize)
        {
            //Common syntax           
            var sqlCmd = @"City_GetByPage";
            List<IdentityCity> listData = null;

            //For paging 
            int offset = (currentPage - 1) * pageSize;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@keyword", filter.keyword },
                {"@offset", offset},
                {"@page_size", pageSize},
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        listData = ParsingListCityFromReader(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return listData;
        }
       
        private List<IdentityCity> ParsingListCityFromReader(IDataReader reader)
        {
            List<IdentityCity> listData = listData = new List<IdentityCity>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractCityData(reader);

                //Extends information
                //record.CreatedDate = reader["CreatedDate"] == DBNull.Value ? null : (DateTime?)reader["CreatedDate"];
                //record.LastUpdated = reader["LastUpdated"] == DBNull.Value ? null : (DateTime?)reader["LastUpdated"];

                if (listData == null)
                    record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

                listData.Add(record);
            }

            return listData;
        }

        public static IdentityCity ExtractCityData(IDataReader reader)
        {
            var record = new IdentityCity();

            //Seperate properties
            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.PrefectureId = Utils.ConvertToInt32(reader["PrefectureId"]);
            record.Name = reader["Name"].ToString();            
            record.Furigana = reader["Furigana"].ToString();

            return record;
        }

        public int Insert(IdentityCity identity)
        {
            //Common syntax           
            var sqlCmd = @"City_Insert";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Name", identity.Name},
                {"@PrefectureId", identity.PrefectureId },
                {"@Furigana", identity.Furigana }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);

                    newId = Convert.ToInt32(returnObj);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public bool Update(IdentityCity identity)
        {
            //Common syntax
            var sqlCmd = @"City_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@Name", identity.Name},
                {"@PrefectureId", identity.PrefectureId },
                {"@Furigana", identity.Furigana }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public IdentityCity GetById(int id)
        {
            IdentityCity info = null;
            var sqlCmd = @"City_GetById";

            var parameters = new Dictionary<string, object>
            {
                {"@id", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractCityData(reader);
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

        public bool Delete(int id)
        {
            //Common syntax            
            var sqlCmd = @"City_Delete";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@id", id},
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public List<IdentityCity> GetList()
        {
            //Common syntax            
            var sqlCmd = @"City_GetList";

            //For parameters
            var parameters = new Dictionary<string, object>
            {                
            };

            List<IdentityCity> listData = new List<IdentityCity>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractCityData(reader);

                            listData.Add(record);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return listData;
        }

        public List<IdentityCity> GetListByPrefecture(int PrefectureId)
        {
            //Common syntax            
            var sqlCmd = @"City_GetListByPrefecture";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@PrefectureId", PrefectureId},
            };

            List<IdentityCity> listData = new List<IdentityCity>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractCityData(reader);

                            listData.Add(record);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return listData;
        }

        #endregion
    }
}
