using MainApi.DataLayer.Entities;

namespace MainApi.Models
{
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