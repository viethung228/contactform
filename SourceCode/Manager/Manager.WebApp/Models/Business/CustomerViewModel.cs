using Manager.WebApp.Resources;
using Manager.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MainApi.DataLayer.Entities;
using Manager.SharedLibs;
using Microsoft.AspNetCore.Mvc.Rendering;
using MainApi.DataLayer;

namespace Manager.WebApp.Models
{
    public class EmployeeViewModel : CommonPagingModel
    {
        public EmployeeViewModel()
        {

        }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_ROLE))]
        public string RoleId { get; set; }

        public List<IdentityRole> AllRoles { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IEnumerable<SelectListItem> UserRoleList
        {
            get
            {
                IEnumerable<SelectListItem> selectList = null;
                if (AllRoles.HasData())
                {
                    selectList = from role in AllRoles
                    select new SelectListItem
                    {
                        Text = role.Name,
                        Value = role.Id,
                        Selected = role.Id == this.RoleId
                    };
                }
                
                return selectList;
            }
        }

        public int Total { get; set; }

        public int SearchStatus { get; set; }

        public int PageNo { get; set; }

        public List<IdentityEmployee> SearchResult { get; set; }

        public IdentityEmployee EmployeeInfoViewModel { get; set; }

        public int IsLocked { get; set; }
    }
}