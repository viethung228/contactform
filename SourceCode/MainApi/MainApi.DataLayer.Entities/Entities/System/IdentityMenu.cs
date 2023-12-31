﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MainApi.DataLayer
{
    [Serializable]
    public class IdentityMenu
    {

        public IdentityMenu()
        {
            Visible = true;
            LangList = new List<IdentityMenuLang>();
        }


        public int Id { get; set; }
        public int? ParentId { get; set; }
        
        public string Area { get; set; }        
        public string Name { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        


        public string Action { get; set; }
        public string Controller { get; set; }
        public bool Visible { get; set; }
        public bool Authenticate { get; set; }
        public string CssClass { get; set; }
        public dynamic Data { get; set; }
        public string Roles { get; set; }
        public int? SortOrder { get; set; }
        public string AbsoluteUri { get; set; }
        public bool Active { get; set; }
        public string IconCss { get; set; }
        public bool CheckPermission { get; set; }
        public List<IdentityMenu> SubMenu { get; set; }

        public string FullDesc {
            get
            {
                string strFullDesc = Title;

                if (!string.IsNullOrEmpty(Desc))
                {
                    strFullDesc += " - " + Desc;
                }

                return strFullDesc;
            }
        }

        public bool HasChildren { 
            get {
                return SubMenu != null && SubMenu.Any();
            } 
        }

        public bool HasVisbleChildren
        {
            get
            {
                return SubMenu != null && SubMenu.Any();
                //return SubMenu != null && SubMenu.Any(m=>m.Visible);
            }
        }


        public int Levels
        {
            get
            {                
                string strMenuID = Id.ToString();
                int zeroCount =strMenuID.Count(c => c == '0');

                int maxLevels = 4;
                return maxLevels - zeroCount;
            }
        }

        public List<IdentityMenuLang> LangList { get; set; }

        public string CurrentTitleLang { get; set; }
    }

    [Serializable]
    public class IdentityMenuLang
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string Title { get; set; }
        public string LangCode { get; set; }
    }
}