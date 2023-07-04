﻿
using Manager.SharedLibs;
using MainApi.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Manager.SharedLibs.Extensions;
using MainApi.DataLayer;

namespace Manager.DataLayer.Repositories
{
    public class RpsAccessRoles
    {
        private readonly string _conStr;
        public RpsAccessRoles(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsAccessRoles()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        public IQueryable<IdentityAccess> GetAllAccess()
        {
            List<IdentityAccess> list = new List<IdentityAccess>();
            List<IdentityAccessLang> langList = new List<IdentityAccessLang>();
            List<IdentityOperation> operationList = new List<IdentityOperation>();
            List<IdentityOperationLang> operationLang = new List<IdentityOperationLang>();

            using (var conn = new SqlConnection(_conStr))
            {
                using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, @"Access_GetAll", null))
                {
                    while (reader.Read())
                    {
                        var item = (IdentityAccess)Activator.CreateInstance(typeof(IdentityAccess));

                        item.Id = reader["Id"].ToString();
                        item.AccessName = reader["AccessName"].ToString();
                        item.Active = Convert.ToBoolean(reader["Active"]);
                        item.Description = reader["Description"].ToString();
                        list.Add(item);
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            langList.Add(ParsingAccessLangFromReader(reader));
                        }
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            operationList.Add(ParsingOperationFromReader(reader));
                        }
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            operationLang.Add(ParsingOperationLangFromReader(reader));
                        }
                    }

                    var hasLangList = langList.HasData();
                    var hasOperationList = operationList.HasData();
                    var hasOperationLangList = operationLang.HasData();
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            if (hasLangList)
                                item.LangList = langList.Where(x => x.AccessId == item.Id).ToList();

                            if (hasOperationList)
                                item.OperationsList = operationList.Where(x => x.AccessId == item.Id).ToList();

                            if (item.OperationsList.HasData())
                            {
                                foreach (var op in item.OperationsList)
                                {
                                    op.LangList = operationLang.Where(x => x.OperationId == op.Id).ToList();
                                }
                            }
                        }
                    }
                }
            }
            return list.AsQueryable<IdentityAccess>();
        }
        public IQueryable<IdentityAccessRoles> GetAccessByRoleId(string RoleId)
        {
            List<IdentityAccessRoles> list = new List<IdentityAccessRoles>();

            using (var conn = new SqlConnection(_conStr))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@RoleId", RoleId}
                };
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT DISTINCT b.Id as AccessId, b.AccessName,b.Description as AccessDescription FROM aspnetaccessroles as a
                      LEFT JOIN aspnetaccess b on a.AccessId = b.Id
                      WHERE a.RoleId = @RoleId and b.Active = 1
                      Order by b.AccessName ASC
                ", parameters);
                while (reader.Read())
                {
                    var item = (IdentityAccessRoles)Activator.CreateInstance(typeof(IdentityAccessRoles));

                    item.AccessId = reader[0].ToString();
                    item.AccessName = reader[1].ToString();
                    item.AccessDescription = reader[2].ToString();
                    list.Add(item);
                }
            }
            return list.AsQueryable<IdentityAccessRoles>();
        }

        public IQueryable<IdentityOperation> GetAllOperations()
        {
            List<IdentityOperation> list = new List<IdentityOperation>();

            using (var conn = new SqlConnection(_conStr))
            {
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT a.Id, a.OperationName, a.Enabled, a.AccessId, a.ActionName , b.AccessName, a.IndexOrder
                      FROM aspnetoperations as a 
                      LEFT JOIN aspnetaccess as b ON a.AccessId = b.Id
                      Order by b.AccessName ASC,a.IndexOrder ASC, a.ActionName ASC", null);
                while (reader.Read())
                {
                    var item = (IdentityOperation)Activator.CreateInstance(typeof(IdentityOperation));

                    item.Id = Convert.ToInt32(reader[0]);
                    item.OperationName = reader[1].ToString();
                    item.Enabled = Convert.ToBoolean(reader[2]);
                    item.AccessId = reader[3].ToString();
                    item.ActionName = reader[4].ToString();
                    item.AccessName = reader[5].ToString();
                    item.IndexOrder = Utils.ConvertToInt32(reader["IndexOrder"]);
                    list.Add(item);
                }
            }
            return list.AsQueryable<IdentityOperation>();
        }

        public IQueryable<IdentityOperation> GetOperationsByAccessId(string AccessId)
        {
            List<IdentityOperation> list = new List<IdentityOperation>();

            using (var conn = new SqlConnection(_conStr))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@AccessId", AccessId}
                };
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT DISTINCT b.Id,b.OperationName,b.Enabled,a.Id as AccessId, b. ActionName FROM aspnetaccess a 
                    RIGHT JOIN aspnetoperations b ON a.Id = b.AccessId
                    LEFT JOIN aspnetaccessroles c ON b.Id = c.OperationId
                    Where a.Id = @AccessId and a.Active = 1
                    Order by b.OperationName ASC", parameters);
                while (reader.Read())
                {
                    var item = (IdentityOperation)Activator.CreateInstance(typeof(IdentityOperation));

                    item.Id = Convert.ToInt32(reader[0]);
                    item.OperationName = reader[1].ToString();
                    item.Enabled = Convert.ToBoolean(reader[2]);
                    item.AccessId = reader[3].ToString();
                    item.ActionName = reader[4].ToString();
                    list.Add(item);
                }
            }
            return list.AsQueryable<IdentityOperation>();
        }

        public IQueryable<IdentityAccessRoles> GetAccessRolesByAccessId(string AccessId)
        {
            List<IdentityAccessRoles> list = new List<IdentityAccessRoles>();

            using (var conn = new SqlConnection(_conStr))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@AccessId", AccessId}
                };
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT DISTINCT a.RoleId,b.Name as RoleName FROM aspnetaccessroles as a
                      LEFT JOIN aspnetroles b on a.RoleId = b.Id
                      WHERE a.AccessId = @AccessId
                ", parameters);
                while (reader.Read())
                {
                    var item = (IdentityAccessRoles)Activator.CreateInstance(typeof(IdentityAccessRoles));

                    item.RoleId = reader[0].ToString();
                    item.RoleName = reader[1].ToString();
                    list.Add(item);
                }
            }
            return list.AsQueryable<IdentityAccessRoles>();
        }

        public IQueryable<IdentityAccessRoles> GetPermissionByRoleId(string RoleId)
        {
            List<IdentityAccessRoles> list = new List<IdentityAccessRoles>();

            using (var conn = new SqlConnection(_conStr))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@RoleId", RoleId}
                };
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT DISTINCT a.RoleId, a.OperationId FROM aspnetaccessroles as a
                      LEFT JOIN aspnetroles b on a.RoleId = b.Id
                      WHERE a.RoleId = @RoleId
                ", parameters);
                while (reader.Read())
                {
                    var item = (IdentityAccessRoles)Activator.CreateInstance(typeof(IdentityAccessRoles));

                    item.RoleId = reader[0].ToString();
                    item.OperationId = Convert.ToInt32(reader[1]);
                    list.Add(item);
                }
            }
            return list.AsQueryable<IdentityAccessRoles>();
        }

        //Before update accessroles: Clear all old data by roleId
        public void DeleteAllAccessOfRole(string RoleId)
        {
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parameters = new Dictionary<string, object>
                {
                    {"@RoleId", RoleId}
                };

                    MsSqlHelper.ExecuteNonQuery(conn, @"DELETE FROM aspnetaccessroles WHERE RoleId = @RoleId", parameters);
                }
            }
            catch
            {

            }
        }

        //Update the accessroles of Role
        public bool UpdateAccessOfRole(string[] operations, string RoleId)
        {
            bool success = false;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (operations.Length != 0)
                    {
                        string query = @"insert into aspnetaccessroles(RoleId,OperationId) values ";
                        foreach (var id in operations)
                        {
                            query += "('" + RoleId + "','" + id + "'),";
                        }
                        query = query.Remove(query.Length - 1);
                        MsSqlHelper.ExecuteNonQuery(conn, query, null);
                    }
                    success = true;
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        //Check permission of user in the Controller(AccessName) and the Action
        public bool CheckPermission(string UserId, string AccessName, string ActionName)
        {
            bool hasPermission = false;
            var count = 0;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"@UserId", UserId},
                        {"@AccessName", AccessName},
                        {"@ActionName", ActionName}
                    };
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT a.*,d.AccessName,c.OperationName FROM aspnetuserroles a 
	                LEFT JOIN aspnetaccessroles b ON a.RoleId = b.RoleId
	                LEFT JOIN aspnetoperations c ON b.OperationId = c.Id
	                LEFT JOIN aspnetaccess d ON c.AccessId = d.Id
	                WHERE c.ActionName = @ActionName AND (a.UserId = @UserId AND a.ParentId = 0)
	                AND d.AccessName = @AccessName AND d.Active = 1", parameters))
                    {
                        if (reader.Read())
                        {
                            count++;
                        }
                    }

                    if (count > 0)
                        hasPermission = true;
                }
            }
            catch
            {
                hasPermission = false;
            }
            return hasPermission;
        }

        //Get all permissions by user
        public List<IdentityPermission> GetPermissionsByUser(string UserId)
        {
            List<IdentityPermission> list = new List<IdentityPermission>();

            var sqlCmd = "Users_GetAllPermissionById";
            using (var conn = new SqlConnection(_conStr))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@UserId", UserId}
                };
                //var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                //    @"SELECT c.ActionName,d.AccessName FROM aspnetuserroles a 
                // LEFT JOIN aspnetaccessroles b ON a.RoleId = b.RoleId
                // LEFT JOIN aspnetoperations c ON b.OperationId = c.Id
                // LEFT JOIN aspnetaccess d ON c.AccessId = d.Id
                // WHERE 1=1 AND a.UserId = @UserId AND d.Active = 1", parameters);

                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                while (reader.Read())
                {
                    var item = new IdentityPermission();
                    item.Action = reader["ActionName"].ToString();
                    item.Controller = reader["AccessName"].ToString();

                    list.Add(item);
                }
            }
            return list;
        }
        private IdentityOperation ParsingOperationFromReader(IDataReader reader)
        {
            var item = (IdentityOperation)Activator.CreateInstance(typeof(IdentityOperation));

            try
            {
                item.Id = Utils.ConvertToInt32(reader["Id"]);
                item.AccessId = reader["AccessId"].ToString();
                item.OperationName = reader["OperationName"].ToString();
                item.ActionName = reader["ActionName"].ToString();
                item.IndexOrder = Utils.ConvertToInt32(reader["IndexOrder"]);
            }
            catch (Exception ex)
            {
            }

            return item;
        }

        private IdentityMenu ParsingMenuFromReader(IDataReader reader)
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

        private IdentityMenuLang ParsingMenuLangFromReader(IDataReader reader)
        {
            var item = new IdentityMenuLang();
            item.Id = Utils.ConvertToInt32(reader["Id"]);
            item.MenuId = Utils.ConvertToInt32(reader["MenuId"]);
            item.Title = reader["Title"].ToString();
            item.LangCode = reader["LangCode"].ToString();

            return item;
        }

        public int AddNewLang(IdentityMenuLang identity)
        {
            //Common syntax           
            var sqlCmd = @"Menu_AddNewLang";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@MenuId", identity.MenuId},
                {"@Title", identity.Title},
                {"@LangCode", identity.LangCode }
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
                var strError = "Failed to execute Menu_AddNewLang. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public int UpdateLang(IdentityMenuLang identity)
        {
            //Common syntax           
            var sqlCmd = @"Menu_UpdateLang";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@MenuId", identity.MenuId},
                {"@Title", identity.Title},
                {"@LangCode", identity.LangCode }
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
                var strError = "Failed to execute Menu_UpdateLang. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return newId;
        }
        public IdentityMenuLang GetLangDetail(int id)
        {
            var sqlCmd = @"Menu_GetLangDetail";
            IdentityMenuLang info = null;

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
                            info = ParsingMenuLangFromReader(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute Menu_GetLangDetail. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return info;
        }

        public bool DeleteLang(int Id)
        {
            //Common syntax            
            var sqlCmd = @"Menu_DeleteLang";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id},
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
                var strError = "Failed to execute Menu_DeleteLang. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public List<IdentityMenu> GetAllDislayMenu()
        {
            //Common syntax           
            var sqlCmd = @"Menu_GetAllDisplayMenu";
            List<IdentityMenu> listMenu = new List<IdentityMenu>();           

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            listMenu.Add(ParsingMenuFromReader(reader));
                        }

                        if (listMenu != null && listMenu.Count > 0)
                        {
                            //All languages
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var langItem = ParsingMenuLangFromReader(reader);
                                    foreach (var item in listMenu)
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
                var strError = "Failed to execute Menu_GetAllDisplayMenu. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return listMenu;
        }

        //Get all root menu by user id (Access and Menu is active, this user has pemission to access)
        public List<IdentityMenu> GetRootMenuByUserId(string UserId)
        {
            //List<IdentityMenu> listMenu = new List<IdentityMenu>();

            //using (var conn = new SqlConnection(_conStr))
            //{
            //    var parameters = new Dictionary<string, object>
            //        {
            //            {"@UserId", UserId}
            //        };
            //    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
            //    @"SELECT DISTINCT e.* FROM aspnetuserroles a 
            // LEFT JOIN aspnetaccessroles b ON a.RoleId = b.RoleId
            // LEFT JOIN aspnetoperations c ON b.OperationId = c.Id
            // LEFT JOIN aspnetaccess d ON c.AccessId = d.Id
            // RIGHT JOIN aspnetmenus e ON (e.Action = c.ActionName AND e.Controller = d.AccessName) 
            //    OR (e.Action IS NULL AND e.Controller IS NULL)
            //    WHERE a.UserId = @UserId AND d.Active = 1 AND e.Active = 1 AND (e.ParentId is null OR e.ParentId = 0)
            //    ORDER BY e.SortOrder
            //    ", parameters))
            //    {
            //        while (reader.Read())
            //        {
            //            listMenu.Add(ParsingMenuFromReader(reader));
            //        }
            //    }

            //}
            //return listMenu.AsQueryable<IdentityMenu>();

            //Common syntax           
            var sqlCmd = @"Menu_GetRootMenuByUserId";
            List<IdentityMenu> listMenu = new List<IdentityMenu>();

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", UserId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            listMenu.Add(ParsingMenuFromReader(reader));
                        }

                        if (listMenu != null && listMenu.Count > 0)
                        {
                            //All languages
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var langItem = ParsingMenuLangFromReader(reader);
                                    foreach (var item in listMenu)
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
                var strError = "Failed to execute Menu_GetRootMenuByUserId. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return listMenu;
        }

        //Get all child menu by user id and parent menu id (Access and Menu is active, this user has pemission to access)
        public List<IdentityMenu> GetChildMenuByUserId(string UserId, int ParentId)
        {
            //List<IdentityMenu> listMenu = new List<IdentityMenu>();

            //using (var conn = new SqlConnection(_conStr))
            //{
            //    var parameters = new Dictionary<string, object>
            //        {
            //            {"@UserId", UserId},
            //            {"@ParentId",ParentId}
            //        };
            //    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
            //    @"SELECT DISTINCT e.* FROM aspnetuserroles a 
            //    LEFT JOIN aspnetaccessroles b ON a.RoleId = b.RoleId
            //    LEFT JOIN aspnetoperations c ON b.OperationId = c.Id
            //    LEFT JOIN aspnetaccess d ON c.AccessId = d.Id
            //    RIGHT JOIN aspnetmenus e ON (e.Action = c.ActionName AND e.Controller = d.AccessName) 
            //    AND (e.Action IS NOT NULL AND e.Controller IS NOT NULL)
            //    WHERE a.UserId = @UserId AND d.Active = 1 AND e.Active = 1 and e.ParentId = @ParentId
            //    ORDER BY e.SortOrder
            //    ", parameters))
            //    {
            //        while (reader.Read())
            //        {
            //            listMenu.Add(ParsingMenuFromReader(reader));
            //        }
            //    }

            //}
            //return listMenu.AsQueryable<IdentityMenu>();

            //Common syntax           
            var sqlCmd = @"Menu_GetChildMenuByUserId";
            List<IdentityMenu> listMenu = new List<IdentityMenu>();
            var parameters = new Dictionary<string, object>
            {
                {"@UserId", UserId},
                {"@ParentId", ParentId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            listMenu.Add(ParsingMenuFromReader(reader));
                        }

                        if (listMenu != null && listMenu.Count > 0)
                        {
                            //All languages
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var langItem = ParsingMenuLangFromReader(reader);
                                    foreach (var item in listMenu)
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
                var strError = "Failed to execute Menu_GetChildMenuByUserId. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return listMenu;
        }

        //Get all active menus
        public IQueryable<IdentityMenu> GetAllMenus()
        {
            List<IdentityMenu> listMenu = new List<IdentityMenu>();

            using (var conn = new SqlConnection(_conStr))
            {
                //var parameters = new Dictionary<string, object>
                //    {
                //        {"@UserId", UserId}
                //    };
                using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                @"SELECT * FROM aspnetmenus ORDER BY SortOrder", null))
                {
                    while (reader.Read())
                    {
                        listMenu.Add(ParsingMenuFromReader(reader));
                    }
                }

            }
            return listMenu.AsQueryable<IdentityMenu>();
        }
        //Get all active menus
        public IQueryable<IdentityOperation> GetListOperationNotUse()
        {
            List<IdentityOperation> listOperations = new List<IdentityOperation>();

            var sqlCmd = @"Operation_GetListNotUse";
            using (var conn = new SqlConnection(_conStr))
            {
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null);
                while (reader.Read())
                {
                    //listOperations.Add(ParsingOperationFromReader(reader));
                    var op = new IdentityOperation();
                    op.AccessName = reader["AccessName"].ToString();
                    op.ActionName = reader["ActionName"].ToString();

                    listOperations.Add(op);
                }

            }
            return listOperations.AsQueryable<IdentityOperation>();
        }

        #region Menu
        public IdentityMenu GetMenuDetail(int id)
        {
            var sqlCmd = @"Menu_GetDetail";
            IdentityMenu info = null;

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
                            info = ParsingMenuFromReader(reader);
                        }

                        //All languages
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                var langItem = ParsingMenuLangFromReader(reader);
                                info.LangList.Add(langItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute Menu_GetDetail. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return info;
        }

        public string InsertMenu(IdentityMenu identity)
        {
            //Common syntax
            var conn = new SqlConnection(_conStr);
            var sqlCmd = @"Menu_Insert";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@ParentId", identity.ParentId},
                {"@Area", identity.Area},
                {"@Name", identity.Name },
                {"@Title", identity.Title},
                {"@Desc", identity.Desc},
                {"@Action", identity.Action },
                {"@Controller", identity.Controller},
                {"@Visible", identity.Visible },
                {"@Authenticate", identity.Authenticate},
                {"@CssClass", identity.CssClass },
                {"@SortOrder", identity.SortOrder },
                {"@AbsoluteUri", identity.AbsoluteUri },
                {"@Active", identity.Active },
                {"@IconCss", identity.IconCss },
            };

            try
            {
                MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute Menu_Insert. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return string.Empty;
        }

        public string UpdateMenu(IdentityMenu identity)
        {
            //Common syntax
            var conn = new SqlConnection(_conStr);
            var sqlCmd = @"Menu_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
               {"@ParentId", identity.ParentId},
                {"@Area", identity.Area},
                {"@Name", identity.Name },
                {"@Title", identity.Title},
                {"@Desc", identity.Desc},
                {"@Action", identity.Action },
                {"@Controller", identity.Controller},
                {"@Visible", identity.Visible },
                {"@Authenticate", identity.Authenticate},
                {"@CssClass", identity.CssClass },
                {"@SortOrder", identity.SortOrder },
                {"@AbsoluteUri", identity.AbsoluteUri },
                {"@Active", identity.Active },
                {"@IconCss", identity.IconCss },
            };

            try
            {
                MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute Menu_Update. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return string.Empty;
        }

        public string BuildUpdateSortingCmd(List<SortingElement> sortList)
        {
            var myCmd = new StringBuilder();
            var itemCmdFormat = @"UPDATE aspnetmenus SET SortOrder = {0}, ParentId = {1} WHERE 1=1 AND Id = {2}; ";
            if (sortList != null && sortList.Count > 0)
            {
                foreach (var item in sortList)
                {
                    var itemCmd = string.Format(itemCmdFormat, item.SortOrder, item.ParentId, item.id);
                    myCmd.Append(itemCmd);

                    if (item.children != null && item.children.Count > 0)
                    {
                        myCmd.Append(BuildUpdateSortingCmd(item.children));
                    }
                }
            }

            return myCmd.ToString();
        }

        public bool UpdateSorting(List<SortingElement> elements)
        {
            //Common syntax
            var conn = new SqlConnection(_conStr);
            var sqlCmd = string.Empty;

            try
            {
                sqlCmd = BuildUpdateSortingCmd(elements);

                MsSqlHelper.ExecuteNonQuery(conn, CommandType.Text, sqlCmd, null);

                return true;
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute UpdateSorting Menu. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }
        }

        public IdentityMenu GetMenuById(int Id)
        {
            var Menu = (IdentityMenu)Activator.CreateInstance(typeof(IdentityMenu));
            var conn = new SqlConnection(_conStr);
            var sqlCmd = @"Menu_GetById";
            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id},
            };
            try

            {
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                while (reader.Read())
                {
                    Menu = ParsingDataFromReader(reader);
                }
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute Menu_Update. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }
            return Menu;
        }
        public IdentityMenu ParsingDataFromReader(IDataReader reader)
        {
            var record = (IdentityMenu)Activator.CreateInstance(typeof(IdentityMenu));

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.ParentId = Utils.ConvertToInt32(reader["ParentId"]);
            record.Area = reader["Area"].ToString();
            record.Name = reader["Name"].ToString();
            record.Title = reader["Title"].ToString();
            record.Desc = reader["Desc"].ToString();
            record.Action = reader["Action"].ToString();
            record.Controller = reader["Controller"].ToString();
            record.Visible = (Utils.ConvertToInt32(reader["Visible"]) == 0 ? false : true);
            record.Authenticate = (Utils.ConvertToInt32(reader["Authenticate"]) == 0 ? false : true);
            record.CssClass = reader["CssClass"].ToString();
            record.SortOrder = Utils.ConvertToInt32(reader["SortOrder"]);
            record.AbsoluteUri = reader["AbsoluteUri"].ToString();
            record.Active = (Utils.ConvertToInt32(reader["Active"]) == 0 ? false : true);
            record.IconCss = reader["IconCss"].ToString();
            return record;
        }
        public void DeleteMenu(int Id)
        {
            //Common syntax
            var conn = new SqlConnection(_conStr);
            var sqlCmd = @"Menu_Delete";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id},
            };

            try
            {
                MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute Menu_Delete. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }
        }
        #endregion

        #region Access
        //Delete Access: Delete menus, operations which linkages to the access
        public bool DeleteAccess(string AccessId)
        {
            var isSuccess = false;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"@AccessId", AccessId}
                    };

                    var query = @"DELETE FROM aspnetoperations WHERE AccessId = @AccessId; DELETE FROM aspnetaccess WHERE Id = @AccessId; ";
                    MsSqlHelper.ExecuteNonQuery(conn, query, parameters);

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool CheckAccessDuplicate(IdentityAccess identity)
        {
            var existed = false;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"@AccessId", identity.Id},
                        {"@AccessName", identity.AccessName}
                    };

                    var query = @"SELECT 1 FROM aspnetaccess WHERE 1=1 AND AccessName = @AccessName AND Id != @AccessId";
                    var result = MsSqlHelper.ExecuteScalar(conn, CommandType.Text, query, parameters);

                    if (Convert.ToBoolean(result))
                    {
                        existed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                existed = false;
            }

            return existed;
        }

        //Create Access
        public bool CreateAccess(IdentityAccess identity)
        {
            var isSuccess = false;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"@AccessName", identity.AccessName},
                        {"@Description", identity.Description}
                    };

                    var query = @"INSERT INTO aspnetaccess(Id,AccessName,Active,Description) values(NEWID(),@AccessName,1,@Description)";
                    MsSqlHelper.ExecuteNonQuery(conn, query, parameters);

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        //Update Access
        public bool UpdateAccess(IdentityAccess identity)
        {
            var isSuccess = false;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"@AccessId", identity.Id},
                        {"@AccessName", identity.AccessName},
                        {"@Description", identity.Description}
                    };

                    var query = @"Update aspnetaccess SET AccessName = @AccessName, Description = @Description WHERE 1=1 AND Id = @AccessId;";
                    MsSqlHelper.ExecuteNonQuery(conn, query, parameters);

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public IdentityAccessLang GetDetailAccessLang(int Id)
        {
            var accessInfo = (IdentityAccessLang)Activator.CreateInstance(typeof(IdentityAccessLang));
            var conn = new SqlConnection(_conStr);
            var sqlCmd = @"AccessLang_GetById";
            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id},
            };
            try

            {
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                while (reader.Read())
                {
                    accessInfo = ParsingDataAccessLangFromReader(reader);
                }
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute AccessLang_GetById. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }
            return accessInfo;
        }
        public IdentityAccessLang ParsingDataAccessLangFromReader(IDataReader reader)
        {
            var record = (IdentityAccessLang)Activator.CreateInstance(typeof(IdentityAccessLang));

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.AccessId = reader["AccessId"].ToString();
            record.LangCode = reader["LangCode"].ToString();
            record.Description = reader["Description"].ToString();

            return record;
        }
        public IdentityAccess GetAccessDetail(string id)
        {
            var sqlCmd = @"Access_GetDetail";
            IdentityAccess info = null;

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
                            info = ParsingAccessFromReader(reader);
                        }

                        //All languages
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                var langItem = ParsingAccessLangFromReader(reader);
                                info.LangList.Add(langItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute Access_GetDetail. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return info;
        }

        private IdentityAccess ParsingAccessFromReader(IDataReader reader)
        {
            var item = (IdentityAccess)Activator.CreateInstance(typeof(IdentityAccess));

            try
            {
                item.Id = reader["Id"].ToString();
                item.AccessName = reader["AccessName"].ToString();
                item.Active = Convert.ToBoolean(reader[13]);
            }
            catch (Exception ex)
            {
            }

            return item;
        }

        //private IdentityAccessLang ParsingMenuLangFromReader(IDataReader reader)
        //{
        //    var item = new IdentityMenuLang();
        //    item.Id = Utils.ConvertToInt32(reader["Id"]);
        //    item.MenuId = Utils.ConvertToInt32(reader["MenuId"]);
        //    item.Title = reader["Title"].ToString();
        //    item.LangCode = reader["LangCode"].ToString();

        //    return item;
        //}
        #endregion
        #region AccessLang
        public int AddAccessLang(IdentityAccessLang identity)
        {
            //Common syntax           
            var sqlCmd = @"Access_AddLang";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@AccessId", identity.AccessId},
                {"@Description", identity.Description},
                {"@LangCode", identity.LangCode }
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
                var strError = "Failed to execute Access_AddLang. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public int UpdateAccessLang(IdentityAccessLang identity)
        {
            //Common syntax           
            var sqlCmd = @"Access_UpdateLang";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@AccessId", identity.AccessId},
                {"@Description", identity.Description},
                {"@LangCode", identity.LangCode }
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
                var strError = "Failed to execute Access_UpdateLang. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public IdentityAccessLang GetAccessLangDetail(int id)
        {
            var sqlCmd = @"Access_GetLangDetail";
            IdentityAccessLang info = null;

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
                            info = ParsingAccessLangFromReader(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute Access_GetLangDetail. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return info;
        }
        private IdentityAccessLang ParsingAccessLangFromReader(IDataReader reader)
        {
            var item = new IdentityAccessLang();
            item.Id = Utils.ConvertToInt32(reader["Id"]);
            item.AccessId = reader["AccessId"].ToString();
            item.Description = reader["Description"].ToString();
            item.LangCode = reader["LangCode"].ToString();

            return item;
        }
        public bool DeleteAccessLang(int Id)
        {
            //Common syntax            
            var sqlCmd = @"Access_DeleteLang";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id},
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
                var strError = "Failed to execute Access_DeleteLang. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return true;
        }
        #endregion

        #region Operation
        public bool CheckOperationDuplicate(IdentityOperation identity)
        {
            var existed = false;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"@Id", identity.Id},
                        {"@AccessId", identity.AccessId},
                        {"@ActionName", identity.ActionName }
                    };

                    var query = @"SELECT 1 FROM aspnetoperations WHERE 1=1 AND AccessId = @AccessId AND ActionName = @ActionName AND Id != @Id";
                    var result = MsSqlHelper.ExecuteScalar(conn, CommandType.Text, query, parameters);

                    if (Convert.ToBoolean(result))
                    {
                        existed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                existed = false;
            }

            return existed;
        }

        //Create Access
        public bool CreateOperation(IdentityOperation identity)
        {
            var isSuccess = false;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"@AccessId", identity.AccessId},
                        {"@ActionName", identity.ActionName},
                        {"@OperationName", identity.OperationName },
                        {"@IndexOrder", identity.IndexOrder }
                    };

                    var query = @"INSERT INTO aspnetoperations(OperationName,Enabled,AccessId,ActionName,IndexOrder) values(@OperationName,1,@AccessId,@ActionName,@IndexOrder)";
                    MsSqlHelper.ExecuteNonQuery(conn, query, parameters);

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        //Update Access
        public bool UpdateOperation(IdentityOperation identity)
        {
            var isSuccess = false;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"@Id", identity.Id },
                        {"@AccessId", identity.AccessId},
                        {"@ActionName", identity.ActionName},
                        {"@OperationName", identity.OperationName },
                        {"@IndexOrder", identity.IndexOrder }
                    };

                    var query = @"Update aspnetoperations SET AccessId = @AccessId, ActionName = @ActionName, OperationName = @OperationName, IndexOrder=@IndexOrder WHERE 1=1 AND Id = @Id;";
                    MsSqlHelper.ExecuteNonQuery(conn, query, parameters);

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        //Delete Operation: aspnetaccessroles which linkages to the operation
        public bool DeleteOperation(int Id)
        {
            var isSuccess = false;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"@Id", Id}
                    };

                    var query = @"DELETE FROM aspnetaccessroles WHERE OperationId = @Id; DELETE FROM aspnetoperations WHERE Id = @Id; ";
                    MsSqlHelper.ExecuteNonQuery(conn, query, parameters);

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public IdentityOperation GetOperationDetail(int id)
        {
            var sqlCmd = @"Operations_GetDetail";
            IdentityOperation info = null;

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
                            info = ParsingOperationFromReader(reader);
                        }

                        //All languages
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                var langItem = ParsingOperationLangFromReader(reader);
                                info.LangList.Add(langItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute Access_GetDetail. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return info;
        }
        #endregion

        #region Operation Lang
        public int AddOperationLang(IdentityOperationLang identity)
        {
            //Common syntax           
            var sqlCmd = @"Operations_AddLang";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@OperationId", identity.OperationId},
                {"@OperationName", identity.OperationName},
                {"@LangCode", identity.LangCode }
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
                var strError = "Failed to execute Operations_AddLang. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public int UpdateOperationLang(IdentityOperationLang identity)
        {
            //Common syntax           
            var sqlCmd = @"Operations_UpdateLang";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@OperationId", identity.OperationId},
                {"@OperationName", identity.OperationName},
                {"@LangCode", identity.LangCode }
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
                var strError = "Failed to execute Operations_UpdateLang. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return newId;
        }
        public IdentityOperationLang GetOperationLangDetail(int id)
        {
            var sqlCmd = @"Operations_GetLangDetail";
            IdentityOperationLang info = null;

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
                            info = ParsingOperationLangFromReader(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute Operations_GetLangDetail. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return info;
        }
        private IdentityOperationLang ParsingOperationLangFromReader(IDataReader reader)
        {
            var item = new IdentityOperationLang();
            item.Id = Utils.ConvertToInt32(reader["Id"]);
            item.OperationId = Utils.ConvertToInt32(reader["OperationId"]);
            item.OperationName = reader["OperationName"].ToString();
            item.LangCode = reader["LangCode"].ToString();

            return item;
        }
        public bool DeleteOperationLang(int Id)
        {
            //Common syntax            
            var sqlCmd = @"Operations_DeleteLang";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id},
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
                var strError = "Failed to execute Operations_DeleteLang. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return true;
        }
        #endregion
    }
}
