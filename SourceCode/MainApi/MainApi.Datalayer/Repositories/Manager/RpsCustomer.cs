using MainApi.DataLayer;
using MainApi.DataLayer.Entities;
using MainApi.SharedLibs;
using MainApi.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Manager.Datalayer.Repositories
{
    public class RpsCustomer
    {
        private readonly string _conStr;

        public RpsCustomer(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsCustomer()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        #region Common

        public List<IdentityCustomer> GetByPage(IdentityCustomer filter, int currentPage, int pageSize)
        {
            //Common syntax           
            var sqlCmd = @"Customer_GetByPage";
            List<IdentityCustomer> listData = null;

            //For paging 
            int offset = (currentPage - 1) * pageSize;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", filter.Keyword},
                {"@RoleId", filter.RoleId},
                {"@LockedEnable", filter.LockoutEnabled },
                {"@Offset", offset},
                {"@PageSize", pageSize},
                {"@ParentId", filter.ParentId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        listData = ParsingListCustomerFromReader(reader);
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

        private List<IdentityCustomer> ParsingListCustomerFromReader(IDataReader reader)
        {
            List<IdentityCustomer> listData = listData = new List<IdentityCustomer>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractCustomerData(reader);

                listData.Add(record);
            }

            return listData;
        }

        public static IdentityCustomer ExtractCustomerData(IDataReader reader)
        {
            var record = new IdentityCustomer();

            //Seperate properties
            if (reader.HasColumn("TotalCount"))
                record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

            record.Id = reader["Id"].ToString();
            record.UserName = reader["UserName"].ToString();
            record.Avatar = reader["Avatar"].ToString();
            record.Email = reader["Email"].ToString();
            record.EmailConfirmed = Utils.ConvertToBoolean(reader["EmailConfirmed"]);
            record.FullName = reader["FullName"].ToString();
            record.PhoneNumber = reader["PhoneNumber"].ToString();
            record.StaffId = Utils.ConvertToInt32(reader["StaffId"]);
            record.ParentId = Utils.ConvertToInt32(reader["ParentId"]);
            record.LockoutEnabled = Utils.ConvertToBoolean(reader["LockoutEnabled"]);
            record.ReceiveAllUpdate = Utils.ConvertToBoolean(reader["ReceiveAllUpdate"]);
            record.RemoveAds = Utils.ConvertToBoolean(reader["RemoveAds"]);
            record.CreatedDateUtc = reader["CreatedDateUtc"] != DBNull.Value ? (DateTime)reader["CreatedDateUtc"] : DateTime.MinValue;
            record.LockoutEndDateUtc = reader["LockoutEndDateUtc"] != DBNull.Value ? (DateTime?)reader["LockoutEndDateUtc"] : null;
            record.SecurityStamp = reader["SecurityStamp"].ToString();

            return record;
        }

        public static IdentityPermission ExtractPermissionData(IDataReader reader)
        {
            var record = new IdentityPermission();

            //Seperate properties
            record.Action = reader["ActionName"].ToString();
            record.Controller = reader["AccessName"].ToString();

            return record;
        }

        public static IdentityMenu ExtractMenuData(IDataReader reader)
        {
            var item = new IdentityMenu();

            item.Id = Utils.ConvertToInt32(reader["Id"]);
            item.ParentId = reader["ParentId"] != null ? Utils.ConvertToInt32(reader["ParentId"]) : 0;
            item.Area = reader["Area"].ToString();
            item.Name = reader["Name"].ToString();
            item.Title = reader["Title"].ToString();
            item.Desc = reader["Desc"].ToString();
            item.Action = reader["Action"].ToString();
            item.Controller = reader["Controller"].ToString();
            item.Visible = (Utils.ConvertToInt32(reader["Visible"]) == 1) ? true : false;
            item.Authenticate = (Utils.ConvertToInt32(reader["Authenticate"]) == 1) ? true : false;
            item.CssClass = reader["CssClass"].ToString();
            item.SortOrder = reader["SortOrder"] != null ? Utils.ConvertToInt32(reader["SortOrder"]) : 0;
            item.AbsoluteUri = reader["AbsoluteUri"].ToString();
            item.Active = (Utils.ConvertToInt32(reader["Active"]) == 1) ? true : false;
            item.IconCss = reader["IconCss"].ToString();
            item.CheckPermission = Utils.ConvertToBoolean(reader["CheckPermission"]);

            return item;
        }

        private IdentityMenuLang ExtractMenuLangData(IDataReader reader)
        {
            var item = new IdentityMenuLang();
            item.Id = Utils.ConvertToInt32(reader["Id"]);
            item.MenuId = Utils.ConvertToInt32(reader["MenuId"]);
            item.Title = reader["Title"].ToString();
            item.LangCode = reader["LangCode"].ToString();

            return item;
        }

        public string Insert(IdentityCustomer identity)
        {
            //Common syntax           
            var sqlCmd = @"Customer_Insert";
            var newId = string.Empty;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserName", identity.UserName },
                {"@Email", identity.Email },
                {"@PhoneNumber", identity.PhoneNumber },
                {"@FullName", identity.FullName },
                {"@PasswordHash", identity.PasswordHash },
                {"@ParentId", identity.ParentId },
                {"@ReceiveAllUpdate", identity.ReceiveAllUpdate },
                {"@SecurityStamp",identity.SecurityStamp }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);

                    newId = returnObj.ToString();
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public bool Update(IdentityCustomer identity)
        {
            //Common syntax           
            var sqlCmd = @"Customer_Update";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@UserName", identity.UserName },
                {"@Email", identity.Email },
                {"@PhoneNumber", identity.PhoneNumber },
                {"@FullName", identity.FullName },
                {"@ReceiveAllUpdate", identity.ReceiveAllUpdate },
                {"@Avatar", identity.Avatar },
                {"@RemoveAds",identity.RemoveAds }

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
        public bool ConfirmEmail(int id)
        {
            //Common syntax           
            var sqlCmd = @"Customer_ConfirmEmail";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", id}
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
        public bool ChangePassword(IdentityCustomer identity)
        {
            //Common syntax           
            var sqlCmd = @"Customer_ChangePassword";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@PasswordHash", identity.PasswordHash }
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

        public bool UpdateAvatar(IdentityCustomer identity)
        {
            //Common syntax           
            var sqlCmd = @"Customer_UpdateAvatar";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@Avatar", identity.Avatar }
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

        public bool LockAccount(IdentityCustomer identity)
        {
            //Common syntax           
            var sqlCmd = @"Customer_LockAccount";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@LockoutEndDateUtc", identity.LockoutEndDateUtc }
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

        public bool UnLockAccount(IdentityCustomer identity)
        {
            //Common syntax           
            var sqlCmd = @"Customer_UnLockAccount";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id }
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

        public IdentityCustomer Login(IdentityCustomer identity)
        {
            IdentityCustomer info = null;
            var sqlCmd = @"Customer_Login";

            var parameters = new Dictionary<string, object>
            {
                {"@UserName", identity.UserName},
                {"@Password", identity.PasswordHash}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractCustomerData(reader);
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

        public IdentityCustomer GetById(string id)
        {
            IdentityCustomer info = null;
            var sqlCmd = @"Customer_GetById";

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractCustomerData(reader);
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
        public IdentityCustomer GetByEmail(string email)
        {
            IdentityCustomer info = null;
            var sqlCmd = @"Customer_GetByEmail";

            var parameters = new Dictionary<string, object>
            {
                {"@Email", email}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractCustomerData(reader);
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
        public IdentityCustomer GetByStaffId(int id)
        {
            IdentityCustomer info = null;
            var sqlCmd = @"Customer_GetByStaffId";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractCustomerData(reader);
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

        public IdentityCustomer GetByUserName(string userName)
        {
            IdentityCustomer info = null;
            var sqlCmd = @"Customer_GetByUserName";

            var parameters = new Dictionary<string, object>
            {
                {"@UserName", userName}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractCustomerData(reader);
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

        public IdentityCustomer GetDetail(int id)
        {
            IdentityCustomer info = null;
            var sqlCmd = @"Customer_GetDetail";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractCustomerData(reader);
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
            var sqlCmd = @"Customer_Delete";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@StaffId", id}
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

        public List<IdentityCustomer> GetList()
        {
            //Common syntax            
            var sqlCmd = @"Customer_GetList";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
            };

            List<IdentityCustomer> listData = new List<IdentityCustomer>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractCustomerData(reader);
                            record.Id = record.StaffId.ToString();
                            record.SecurityStamp = "";
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

        public List<IdentityPermission> GetPermissionsByCustomer(string userId)
        {
            List<IdentityPermission> list = new List<IdentityPermission>();
            var sqlCmd = @"Customer_GetAllPermissionById";

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", userId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var r = ExtractPermissionData(reader);

                            list.Add(r);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return list;
        }

        public List<IdentityCustomer> GetCustomersByPermission(int agencyId, string actionName, string controllerName)
        {
            //Common syntax           
            var sqlCmd = @"Customer_GetListByPermission";
            List<IdentityCustomer> listData = new List<IdentityCustomer>();

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@AgencyId", agencyId},
                {"@ActionName", actionName},
                {"@AccessName", controllerName }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var record = new IdentityCustomer();
                            record.Id = reader["Id"].ToString();
                            record.StaffId = Utils.ConvertToInt32(reader["StaffId"]);
                            record.ParentId = Utils.ConvertToInt32(reader["ParentId"]);
                            record.UserName = reader["UserName"].ToString();

                            listData.Add(record);
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                var record = new IdentityCustomer();
                                record.Id = reader["Id"].ToString();
                                record.StaffId = Utils.ConvertToInt32(reader["StaffId"]);
                                record.ParentId = Utils.ConvertToInt32(reader["ParentId"]);
                                record.UserName = reader["UserName"].ToString();

                                listData.Add(record);
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


        public List<IdentityMenu> GetRootMenuByCustomerId(string userId)
        {
            List<IdentityMenu> list = new List<IdentityMenu>();
            var sqlCmd = @"Menu_GetRootMenuByCustomerId";

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", userId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var r = ExtractMenuData(reader);

                            list.Add(r);
                        }

                        if (list.HasData())
                        {
                            //All languages
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var langItem = ExtractMenuLangData(reader);
                                    foreach (var item in list)
                                    {
                                        if (item.Id == langItem.MenuId)
                                        {
                                            item.LangList.Add(langItem);
                                        }
                                    }
                                }
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

            return list;
        }

        public List<IdentityMenu> GetChildMenuByCustomerId(string userId, int parentId)
        {
            List<IdentityMenu> list = new List<IdentityMenu>();
            var sqlCmd = @"Menu_GetChildMenuByCustomerId";

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", userId},
                {"@ParentId", parentId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var r = ExtractMenuData(reader);

                            list.Add(r);
                        }

                        if (list.HasData())
                        {
                            //All languages
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var langItem = ExtractMenuLangData(reader);
                                    foreach (var item in list)
                                    {
                                        if (item.Id == langItem.MenuId)
                                        {
                                            item.LangList.Add(langItem);
                                        }
                                    }
                                }
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

            return list;
        }

        public List<IdentityMenu> GetAllDislayMenu()
        {
            List<IdentityMenu> list = new List<IdentityMenu>();
            var sqlCmd = @"Menu_GetAllDisplayMenu";

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var r = ExtractMenuData(reader);

                            list.Add(r);
                        }

                        if (list.HasData())
                        {
                            //All languages
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var langItem = ExtractMenuLangData(reader);
                                    foreach (var item in list)
                                    {
                                        if (item.Id == langItem.MenuId)
                                        {
                                            item.LangList.Add(langItem);
                                        }
                                    }
                                }
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

            return list;
        }

        public List<IdentityCustomer> GetListReceiveAllUpdate(int agencyId)
        {
            //Common syntax            
            var sqlCmd = @"Customer_GetListReceiveAllUpdate";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@AgencyId", agencyId},
            };

            List<IdentityCustomer> listData = new List<IdentityCustomer>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractCustomerData(reader);

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
    }
}
