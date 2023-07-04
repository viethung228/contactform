﻿using MainApi.Resources;
using MainApi.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace MainApi.Models
{
    public class LoginViewModel
    {
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_EMAIL_LOGIN))]
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_EMAIL_NOT_NULL))]
        //[EmailAddress(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_EMAIL_INVALID))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_PASSWORD))]
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_PASSWORD_NOT_NULL))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_REMEMBER_LOGIN))]
        public bool RememberMe { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_EMAIL))]
        //[Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        public string Email { get; set; }

        public string State { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class RegisterViewModel
    {
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_FULL_NAME))]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_EMAIL))]
        [EmailAddress]
        public string Email { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RolesList { get; set; }
        public bool IsActived { get; set; }

        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_PASSWORD))]
        public string Password { get; set; }
   
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_PHONE))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }
        public string Role { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_CONFIRM_PASSWORD))]
        [Compare("Password", ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_CONFRIM_PASSWORD_NOT_MATCH))]
        public string ConfirmPassword { get; set; }

        public int StaffId { get; set; }
    }

    public class AccountDetailViewModel
    {
        public string Id { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_USERNAME))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_FULL_NAME))]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_EMAIL))]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_ROLE))]
        public List<string> RolesList { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_PHONE))]
        public string PhoneNumber { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_CREATED_DATE))]
        public DateTime CreatedDateUtc { get; set; }

        public string Avatar { get; set; }

        public PagingInfo ActivityPagingInfo { get; set; }
    }

    public class PagingInfo
    {

        public int Total { get; set; }

        public int CurrentPage { get; set; }

        public int PageNo { get; set; }

        public int PageSize { get; set; }
    }

    public class AccountChangePasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_OLD_PASSWORD))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_NEW_PASSWORD))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_CONFIRM_PASSWORD))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_CONFRIM_PASSWORD_NOT_MATCH))]
        public string ConfirmNewPassword { get; set; }
    }

    public class AccountRecoverPasswordModel : CommonNotificationModel
    {
        public string Token { get; set; }

        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_NEW_PASSWORD))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_CONFIRM_PASSWORD))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_CONFRIM_PASSWORD_NOT_MATCH))]
        public string ConfirmNewPassword { get; set; }
    }

    public class AccountUpdateAvatarModel
    {
        public string Base64ImageStr { get; set; }
        public string ChangingDetected { get; set; }
    }
}