﻿using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MainApi.Helpers
{
    public class ControllerOperations
    {
        public string ControllerName { get; set; }
        public List<IdentityOperation> AllOperations { get; set; }

        public ControllerOperations()
        {
            AllOperations = new List<IdentityOperation>();
        }
    }

    public class Constant
    {
        public static string DATEFORMAT_yyyyMMddHHmmss = "yyyyMMddHHmmss";
        public static string DATEFORMAT_ddMMyyyyHHmmss = "ddMMyyyyHHmmss";
        public static string DATE_FORMAT_ddMMyyyy = "dd/MM/yyyy";

        public const string PREFIX_WEB_LOGIN_MODEL_REDIS_KEY = "WEB_LOGIN_MODEL";
        public const string PREFIX_API_LOGIN_MODEL_REDIS_KEY = "API_LOGIN_MODEL";


        public static List<ControllerOperations> GetAllControllerOperations()
        {
            List<ControllerOperations> myList = new List<ControllerOperations>();

            //Dashboard controller
            var dashBoard = new ControllerOperations();
            var dashBoardCtrlName = "Home";
            dashBoard.ControllerName = dashBoardCtrlName;
            dashBoard.AllOperations.Add(new IdentityOperation { ControllerName = dashBoardCtrlName, ActionName = "Index", OperationName = "Show Dashboard" });

            //Account controller
            var account = new ControllerOperations();
            var accountCtrlName = "Account";
            account.ControllerName = accountCtrlName;
            account.AllOperations.Add(new IdentityOperation { ControllerName = accountCtrlName, ActionName = "ChangePassword", OperationName = "Change password" });
            account.AllOperations.Add(new IdentityOperation { ControllerName = accountCtrlName, ActionName = "Profile", OperationName = "View Profile" });

            //Role controller
            var role = new ControllerOperations();
            var roleCtrlName = "RolesAdmin";
            role.ControllerName = roleCtrlName;
            role.AllOperations.Add(new IdentityOperation { ControllerName = roleCtrlName, ActionName = "Index", OperationName = "View Roles" });
            role.AllOperations.Add(new IdentityOperation { ControllerName = roleCtrlName, ActionName = "Create", OperationName = "Create Role" });
            role.AllOperations.Add(new IdentityOperation { ControllerName = roleCtrlName, ActionName = "Update", OperationName = "Edit Role" });

            //UsersAdmin controller: Manage users 
            var userAdmin = new ControllerOperations();
            var userAdminCtrlName = "UsersAdmin";
            userAdmin.ControllerName = userAdminCtrlName;
            userAdmin.AllOperations.Add(new IdentityOperation { ControllerName = userAdminCtrlName, ActionName = "Index", OperationName = "View Users" });
            userAdmin.AllOperations.Add(new IdentityOperation { ControllerName = userAdminCtrlName, ActionName = "Create", OperationName = "Create User" });
            userAdmin.AllOperations.Add(new IdentityOperation { ControllerName = userAdminCtrlName, ActionName = "Edit", OperationName = "Edit User" });
            userAdmin.AllOperations.Add(new IdentityOperation { ControllerName = userAdminCtrlName, ActionName = "DeleteUser", OperationName = "Delete User" });
            userAdmin.AllOperations.Add(new IdentityOperation { ControllerName = userAdminCtrlName, ActionName = "ResetPassword", OperationName = "Reset User Password" });
            userAdmin.AllOperations.Add(new IdentityOperation { ControllerName = userAdminCtrlName, ActionName = "Details", OperationName = "View User Detail" });

            //System controller
            var system = new ControllerOperations();
            var systemCtrlName = "System";
            system.ControllerName = systemCtrlName;
            system.AllOperations.Add(new IdentityOperation { ControllerName = systemCtrlName, ActionName = "UserActivity", OperationName = "User Activities" });

            //AccessRole
            var accessRole = new ControllerOperations();
            var accessRoleCtrlName = "AccessRoles";
            accessRole.ControllerName = accessRoleCtrlName;
            accessRole.AllOperations.Add(new IdentityOperation { ControllerName = accessRoleCtrlName, ActionName = "Index", OperationName = "View Access List" });
            accessRole.AllOperations.Add(new IdentityOperation { ControllerName = accessRoleCtrlName, ActionName = "UpdateAccessRoles", OperationName = "Update Access Roles" });

            myList.Add(dashBoard);
            myList.Add(account);
            myList.Add(role);
            myList.Add(userAdmin);
            myList.Add(system);
            myList.Add(accessRole);

            return myList;
        }

        public static List<string> GetAllControllers(string path)
        {
            var filesName = new List<string>();
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            //var files = Directory.EnumerateFiles(path);
            var files = GetControllerNames();
            if (files != null && files.Count() > 0)
            {
                foreach (var item in files)
                {
                    filesName.Add(Path.GetFileNameWithoutExtension(item).Replace("Controller", "").Replace("Controllers", ""));
                }
            }

            filesName = IgnoreSomeControllers(filesName);

            return filesName.OrderBy(x => x).ToList();
        }

        private static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }

        public static List<string> GetControllerNames()
        {
            List<string> controllerNames = new List<string>();
            GetSubClasses<Controller>().ForEach(
                type => controllerNames.Add(type.Name));
            return controllerNames;
        }

        public static List<string> IgnoreSomeControllers(List<string> ignoreList)
        {
            if (ignoreList.Count > 0)
            {
                ignoreList.Remove("BaseAdmin");
                ignoreList.Remove("Base");
                ignoreList.Remove("BaseAuthed");
                ignoreList.Remove("Error");
                ignoreList.Remove("Template");
            }

            return ignoreList;
        }
    }

    public class ActionMethod
    {
        public static string Api = "API";
        public static string Web = "WEB";
    }

    public class ActionType
    {
        public static string Login = "login";
        public static string Register = "register";
        public static string RefreshToken = "refresh_token";
        public static string CreateOTP = "create_otp";
        public static string VerifyOTP = "verify_otp";
        public static string ChangePassword1 = "changepwd1";
        public static string ChangePassword2 = "changepwd2";
        public static string UpdateProfile = "update_profile";
        public static string ChangeAuthMethod = "changeauthmethod";
        public static string RecoverPassword1 = "recover_password1";
        public static string RecoverPassword2 = "recover_password2";
        public static string ActiveAccount = "active_account";
    }

    public class OTPType
    {
        public static string OTPAPP = "OTPAPP";        
        public static string OTPSMS = "OTPSMS";

        public static string ODPAPP = "ODPAPP";
        public static string ODPSMS = "ODPSMS";

        public static string PASSWORD2 = "PASSWORD2";
    }

    public class UserPasswordLevel
    {
        public static int Level1 = 1;
        public static int Level2 = 2;
    }

    public class PasswordLevelType
    {
        public static string Level1 = "level1";
        public static string Level2 = "level2";
    }

    public class UserSex
    {
        public static int Male = 1;
        public static int Female = 0;
    }

    public class RecoverPasswordMethod {
        //EMAIL, OTPSMS, ODPSMS, OTPAPP, ODPAPP or ID of security question
        public static string EMAIL = "EMAIL";
        public static string OTPSMS = "OTPSMS";
        public static string ODPSMS = "ODPSMS";
        public static string OTPAPP = "OTPAPP";
        public static string ODPAPP = "ODPAPP";
        public static string QUESTIONID = "ID";
    }

    public class ActiveAccountMethod
    {
        public static string Email = "email";
        public static string Phone = "phone";
    }
}