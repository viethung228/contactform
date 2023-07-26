namespace MainApi.DataLayer.Entities
{
    public class IdentityCompany : IdentityCommon
    {
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int ProviderId { get; set; }
        public string Code { get; set; }
        public string Avatar { get; set; }
        public List<string> Roles { get; set; }
        public List<IdentityCompanyClaim> Claims { get; set; }
        public string Qrcode { get; set; }
        public string FullName { get; set; }
        public int CompanyId { get; set; }
        public string Address { get; set; }
        public int ParentId { get; set; }
        public string HashingData { get; set; }

        public int Status { get; set; }
        public IdentityCompany()
        {
            this.Claims = new List<IdentityCompanyClaim>();
            this.Roles = new List<string>();
            this.Id = Guid.NewGuid().ToString();
            LockoutEnabled = true;
        }

        public IdentityCompany(string userName)
            : this()
        {
            this.CompanyName = userName;
        }

        //Filtering
        public string RoleId { get; set; }
    }

    public sealed class IdentityCompanyLogin
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Provider { get; set; }
        public string ProviderKey { get; set; }
    }

    public class IdentityCompanyClaim
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class IdentityStatisticsUCompanyByYear
    {
        public int id { get; set; }
        public int AgencyId { get; set; }
        public int year { get; set; }
        public int month_1 { get; set; }
        public int month_2 { get; set; }
        public int month_3 { get; set; }
        public int month_4 { get; set; }
        public int month_5 { get; set; }
        public int month_6 { get; set; }
        public int month_7 { get; set; }
        public int month_8 { get; set; }
        public int month_9 { get; set; }
        public int month_10 { get; set; }
        public int month_11 { get; set; }
        public int month_12 { get; set; }

        public DateTime? AgencyRegisteredDate { get; set; }
    }
}
