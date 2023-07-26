using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MainApi.DataLayer.Entities;
using Manager.WebApp.Settings;

namespace Manager.WebApp.Models
{
    public class SettingsViewModel
    {
        /// <summary>
        /// Forcus on current settings type
        /// </summary>
        public string CurrentSettingsType { get; set; }

        /// <summary>
        /// Contains all settings type instances
        /// </summary>
        public SiteSettings SystemSestings { get; set; }

        public List<IdentityEmailServer> EmailServers { get; set; }

        public bool EmailPasswordChanged { get; set; }

        public IEnumerable<SelectListItem> TimeZoneList()
        {
            var timeZoneList = TimeZoneInfo
            .GetSystemTimeZones()
            .Select(t => new SelectListItem
            {
                Text = t.DisplayName,
                Value = t.Id,
                Selected = !string.IsNullOrEmpty(this.SystemSestings.General.TimeZoneId) && t.Id == this.SystemSestings.General.TimeZoneId
            });

            return timeZoneList;
        }
    }

    public class BusinessSettingsViewModel
    {
        /// <summary>
        /// Forcus on current settings type
        /// </summary>
        public string CurrentSettingsType { get; set; }

        /// <summary>
        /// Contains all settings type instances
        /// </summary>
        public BusinessSettings Settings { get; set; }       
    }
}