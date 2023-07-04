namespace MainApi.DataLayer.Entities
{
    public class IdentityCoin : IdentityCommon
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public int point { get; set; }
        
    }
    public class CoinHistory {
        public string ValueChange { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SourceType { get; set; }
        public int? TotalCount { get; set; }
    }
    public class CoinHistoryView
    {
        public string ValueChange { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SourceType { get; set; }
        public int TotalCount { get; set; }
    }
}
