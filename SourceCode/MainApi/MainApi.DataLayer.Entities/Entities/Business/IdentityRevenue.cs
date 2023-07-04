namespace MainApi.DataLayer.Entities
{
    public class IdentityRevenue : IdentityCommon
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public float Price { get; set; }
        public int Coin { get; set; }
        public int SourceType { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalRevenue { get; set; }

    }
    public class RevenueViewModel
    {
        public string UserName { get; set; }
        public float Price { get; set; }
        public int Coin { get; set; }
        public string SourceType { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalCount { get; set; }
        public int TotalRevenue { get; set; }
    }
}
