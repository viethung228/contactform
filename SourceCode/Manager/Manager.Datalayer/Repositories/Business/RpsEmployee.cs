using Manager.DataLayer;
using MainApi.DataLayer.Entities;
using Manager.SharedLibs;
using Manager.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MainApi.DataLayer;

namespace Manager.Datalayer.Repositories
{
    public class RpsEmployee
    {
        private readonly string _conStr;

        public RpsEmployee(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsEmployee()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        #region Common

        public List<IdentityEmployee> GetByPage(IdentityEmployee filter, int currentPage, int pageSize)
        {
            //Common syntax           
            var sqlCmd = @"Employee_GetByPage";
            List<IdentityEmployee> listData = null;

            //For paging 
            int offset = (currentPage - 1) * pageSize;

            //For parms
            var parms = new Dictionary<string, object>
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
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
                    {
                        listData = ParsingListEmployeeFromReader(reader);
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

        private List<IdentityEmployee> ParsingListEmployeeFromReader(IDataReader reader)
        {
            List<IdentityEmployee> listData = listData = new List<IdentityEmployee>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractEmployeeData(reader);

                listData.Add(record);
            }

            return listData;
        }

        public static IdentityEmployee ExtractEmployeeData(IDataReader reader)
        {
            var record = new IdentityEmployee();

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
            record.CreatedDateUtc = reader["CreatedDateUtc"] != DBNull.Value ? (DateTime)reader["CreatedDateUtc"] : DateTime.MinValue;
            record.LockoutEndDateUtc = reader["LockoutEndDateUtc"] != DBNull.Value ? (DateTime?)reader["LockoutEndDateUtc"] : null;
            record.Qrcode = reader["Qrcode"].ToString();
            record.ReceiveAllUpdate = Utils.ConvertToBoolean(reader["ReceiveAllUpdate"]);

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

        public string Insert(IdentityEmployee identity)
        {
            //Common syntax           
            var sqlCmd = @"Employee_Insert";
            var newId = string.Empty;

            //For parms
            var parms = new Dictionary<string, object>
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
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parms);

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
        public List<string> GetAllEmployeeNameHasRevenueHistory()
        {
            //Common syntax           
            var sqlCmd = @"Revenue_GetAllEmployeeName";
            var result = new List<string>();

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractEmployeeData(reader);
                            result.Add(record.UserName);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return result;
        }
        public bool Update(IdentityEmployee identity)
        {
            //Common syntax           
            var sqlCmd = @"Employee_Update";
            //For parms
            var parms = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@UserName", identity.UserName },
                {"@Email", identity.Email },
                {"@PhoneNumber", identity.PhoneNumber },
                {"@FullName", identity.FullName },
                {"@ReceiveAllUpdate", identity.ReceiveAllUpdate },
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parms);

                    var delRoleParms = new Dictionary<string, object>
                    {
                        {"@UserId", identity.Id }
                    };

                    //Clear old roles
                    MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, @"Employee_DeleteRoles", delRoleParms);

                    if (identity.Roles.HasData())
                    {
                        foreach (var roleId in identity.Roles)
                        {
                            var userRoleParms = new Dictionary<string, object>
                            {
                                {"@UserId", identity.Id },
                                {"@RoleId", roleId },
                            };

                            //Update role
                            MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, @"Employee_AddToRole", userRoleParms);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool ChangePassword(IdentityEmployee identity)
        {
            //Common syntax           
            var sqlCmd = @"Employee_ChangePassword";
            //For parms
            var parms = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@PasswordHash", identity.PasswordHash }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parms);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool UpdateAvatar(IdentityEmployee identity)
        {
            //Common syntax           
            var sqlCmd = @"Employee_UpdateAvatar";
            //For parms
            var parms = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@Avatar", identity.Avatar }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parms);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool LockAccount(IdentityEmployee identity)
        {
            //Common syntax           
            var sqlCmd = @"Employee_LockAccount";
            //For parms
            var parms = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@LockoutEndDateUtc", identity.LockoutEndDateUtc }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parms);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool UnLockAccount(IdentityEmployee identity)
        {
            //Common syntax           
            var sqlCmd = @"Employee_UnLockAccount";
            //For parms
            var parms = new Dictionary<string, object>
            {
                {"@Id", identity.Id }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parms);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public IdentityEmployee Login(IdentityEmployee identity)
        {
            IdentityEmployee info = null;
            var sqlCmd = @"Employee_Login";

            var parms = new Dictionary<string, object>
            {
                {"@UserName", identity.UserName},
                {"@Password", identity.PasswordHash}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
                    {
                        if (reader.Read())
                        {
                            info = ExtractEmployeeData(reader);
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

        public IdentityEmployee GetById(string id)
        {
            IdentityEmployee info = null;
            var sqlCmd = @"Employee_GetById";

            var parms = new Dictionary<string, object>
            {
                {"@UserId", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
                    {
                        if (reader.Read())
                        {
                            info = ExtractEmployeeData(reader);
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

        public IdentityEmployee GetByUserName(string userName)
        {
            IdentityEmployee info = null;
            var sqlCmd = @"Employee_GetByUserName";

            var parms = new Dictionary<string, object>
            {
                {"@UserName", userName}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
                    {
                        if (reader.Read())
                        {
                            info = ExtractEmployeeData(reader);
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

        public IdentityEmployee GetByStaffId(int id)
        {
            IdentityEmployee info = null;
            var sqlCmd = @"Employee_GetByStaffId";

            var parms = new Dictionary<string, object>
            {
                {"@Id", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
                    {
                        if (reader.Read())
                        {
                            info = ExtractEmployeeData(reader);
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
            var sqlCmd = @"Employee_Delete";

            //For parms
            var parms = new Dictionary<string, object>
            {
                {"@StaffId", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parms);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public List<IdentityEmployee> GetList()
        {
            //Common syntax            
            var sqlCmd = @"Employee_GetList";

            //For parms
            var parms = new Dictionary<string, object>
            {
            };

            List<IdentityEmployee> listData = new List<IdentityEmployee>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractEmployeeData(reader);

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

        public List<IdentityEmployee> GetActiveEmployeesByParent(int parentId)
        {
            //Common syntax            
            var sqlCmd = @"Employee_GetActiveEmployeesByParent";

            //For parms
            var parms = new Dictionary<string, object>
            {
                {"@ParentId", parentId}
            };

            List<IdentityEmployee> listData = new List<IdentityEmployee>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
                    {
                        while (reader.Read())
                        {
                            var record = new IdentityEmployee();
                            record.Id = reader["Id"].ToString();
                            record.StaffId = Utils.ConvertToInt32(reader["StaffId"]);

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

        public bool UpdateQrcode(int staffId, string imagePath)
        {
            //Common syntax            
            var sqlCmd = @"Employee_UpdateQrcode";

            //For parms
            var parms = new Dictionary<string, object>
            {
                {"@StaffId", staffId},
                {"@Qrcode", imagePath},
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parms);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        #endregion

        public List<IdentityPermission> GetPermissionsByEmployee(string userId)
        {
            List<IdentityPermission> list = new List<IdentityPermission>();
            var sqlCmd = @"Employee_GetAllPermissionById";

            var parms = new Dictionary<string, object>
            {
                {"@UserId", userId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
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

        public List<IdentityMenu> GetRootMenuByEmployeeId(string userId)
        {
            List<IdentityMenu> list = new List<IdentityMenu>();
            var sqlCmd = @"Menu_GetRootMenuByEmployeeId";

            var parms = new Dictionary<string, object>
            {
                {"@UserId", userId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
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

        public List<IdentityMenu> GetChildMenuByEmployeeId(string userId, int parentId)
        {
            List<IdentityMenu> list = new List<IdentityMenu>();
            var sqlCmd = @"Menu_GetChildMenuByEmployeeId";

            var parms = new Dictionary<string, object>
            {
                {"@UserId", userId},
                {"@ParentId", parentId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
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
    }
}
