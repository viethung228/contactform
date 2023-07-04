namespace MainApi.DataLayer.Entities
{
    public class IdentityAds : IdentityCommon
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool RemoveAds { get; set; }
        public DateTime UpdatedDate { get; set; }


    }   
}
