using MainApi.DataLayer.Entities;
using MainApi.SharedLibs;
using MainApi.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MainApi.DataLayer.Repositories
{
    public class RpsRegion
    {
        private readonly string _conStr;

        public RpsRegion(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsRegion()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        #region  Common

        public List<IdentityRegion> GetByPage(IdentityRegion filter, int currentPage, int pageSize)
        {
            //Common syntax           
            var sqlCmd = @"Region_GetAll";
            List<IdentityRegion> listData = null;

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
                        listData = ParsingListRegionFromReader(reader);
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
       
        private List<IdentityRegion> ParsingListRegionFromReader(IDataReader reader)
        {
            List<IdentityRegion> listData = listData = new List<IdentityRegion>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractRegionData(reader);

                //Extends information
                //record.CreatedDate = reader["CreatedDate"] == DBNull.Value ? null : (DateTime?)reader["CreatedDate"];
                //record.LastUpdated = reader["LastUpdated"] == DBNull.Value ? null : (DateTime?)reader["LastUpdated"];

                if (listData == null)
                    record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

                listData.Add(record);
            }

            return listData;
        }

        public static IdentityRegion ExtractRegionData(IDataReader reader)
        {
            var record = new IdentityRegion();

            //Seperate properties
            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();            
            record.Furigana = reader["Furigana"].ToString();

            return record;
        }

        public int Insert(IdentityRegion identity)
        {
            //Common syntax           
            var sqlCmd = @"Region_Insert";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Name", identity.Name},
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

        public bool Update(IdentityRegion identity)
        {
            //Common syntax
            var sqlCmd = @"Region_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@Name", identity.Name},
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

        public IdentityRegion GetById(int id)
        {
            IdentityRegion info = null;
            var sqlCmd = @"Region_GetById";

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
                            info = ExtractRegionData(reader);
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
            var sqlCmd = @"Region_Delete";

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

        public List<IdentityRegion> GetList(int countryId)
        {
            //Common syntax            
            var sqlCmd = @"Region_GetList";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@CountryId", countryId},
            };

            List<IdentityRegion> listData = new List<IdentityRegion>();
            //List<IdentityCity> cities = new List<IdentityCity>();
            List<IdentityPrefecture> prefectures = new List<IdentityPrefecture>();

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractRegionData(reader);

                            listData.Add(record);
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                prefectures.Add(RpsPrefecture.ExtractPrefectureData(reader));
                            }
                        }

                        if (listData.HasData())
                        {
                            foreach (var item in listData)
                            {
                                item.Prefectures = prefectures.Where(x => x.RegionId == item.Id).ToList();
                            }
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
