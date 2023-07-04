using MainApi.DataLayer.Entities;
using MainApi.SharedLibs;
using MainApi.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MainApi.DataLayer.Repositories
{
    public class RpsPrefecture
    {
        private readonly string _conStr;

        public RpsPrefecture(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsPrefecture()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        #region Common

        public List<IdentityPrefecture> GetByPage(IdentityPrefecture filter, int currentPage, int pageSize)
        {
            //Common syntax           
            var sqlCmd = @"Prefecture_GetAll";
            List<IdentityPrefecture> listData = null;

            //For paging 
            int offset = (currentPage - 1) * pageSize;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", filter.Keyword },
                {"@Offset", offset},
                {"@PageSize", pageSize},
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        listData = ParsingListPrefectureFromReader(reader);
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
       
        private List<IdentityPrefecture> ParsingListPrefectureFromReader(IDataReader reader)
        {
            List<IdentityPrefecture> listData = listData = new List<IdentityPrefecture>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractPrefectureData(reader);

                //Extends information
                //record.CreatedDate = reader["CreatedDate"] == DBNull.Value ? null : (DateTime?)reader["CreatedDate"];
                //record.LastUpdated = reader["LastUpdated"] == DBNull.Value ? null : (DateTime?)reader["LastUpdated"];

                if (listData == null)
                    record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

                listData.Add(record);
            }

            return listData;
        }

        public static IdentityPrefecture ExtractPrefectureData(IDataReader reader)
        {
            var record = new IdentityPrefecture();

            //Seperate properties
            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.RegionId = Utils.ConvertToInt32(reader["RegionId"]);
            record.Name = reader["Name"].ToString();            
            record.Furigana = reader["Furigana"].ToString();

            return record;
        }

        public int Insert(IdentityPrefecture identity)
        {
            //Common syntax           
            var sqlCmd = @"Prefecture_Insert";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Name", identity.Name},
                {"@RegionId", identity.RegionId },
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

        public bool Update(IdentityPrefecture identity)
        {
            //Common syntax
            var sqlCmd = @"Prefecture_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@Name", identity.Name},
                {"@RegionId", identity.RegionId },
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

        public IdentityPrefecture GetById(int id)
        {
            IdentityPrefecture info = null;
            var sqlCmd = @"Prefecture_GetById";

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
                            info = ExtractPrefectureData(reader);
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
            var sqlCmd = @"Prefecture_Delete";

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

        public List<IdentityPrefecture> GetList()
        {
            //Common syntax            
            var sqlCmd = @"Prefecture_GetList";

            //For parameters
            var parameters = new Dictionary<string, object>
            {                
            };

            List<IdentityPrefecture> listData = new List<IdentityPrefecture>();
            List<IdentityCity> cities = new List<IdentityCity>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractPrefectureData(reader);

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

        public List<IdentityPrefecture> GetListByRegion(int region_id)
        {
            //Common syntax            
            var sqlCmd = @"Prefecture_GetListByRegion";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@RegionId", region_id},
            };

            List<IdentityPrefecture> listData = new List<IdentityPrefecture>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractPrefectureData(reader);

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

        public List<IdentityPrefecture> GetListByCountry(int country_id)
        {
            //Common syntax            
            var sqlCmd = @"Prefecture_GetListByCountry";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@country_id", country_id},
            };

            List<IdentityPrefecture> listData = new List<IdentityPrefecture>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractPrefectureData(reader);

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
