namespace MainApi.DataLayer.Entities
{
    public class IdentityLinkSetting : IdentityCommon
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string Link { get; set; }
        public bool Type { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CoverImage { get; set; }
    }    
}
