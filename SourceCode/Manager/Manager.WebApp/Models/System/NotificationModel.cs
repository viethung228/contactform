using MainApi.DataLayer.Entities;
using System.Collections.Generic;

namespace Manager.WebApp.Models
{
    public class ManageNotificationModel : CommonPagingModel
    {
        public List<NotificationItemModel> SearchResults { get; set; }
    }

    public class NotificationItemModel
    {
        public IdentityNotification NotifInfo { get; set; }
    }

    public class NotificationMarkIsReadModel
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public bool IsRead { get; set; }
    }
}