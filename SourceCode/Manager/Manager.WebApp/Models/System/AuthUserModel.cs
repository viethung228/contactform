namespace Manager.WebApp.Models
{
    public class AuthUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? StaffId { get; set; }
        public int? AgencyId { get; set; }
    }
}