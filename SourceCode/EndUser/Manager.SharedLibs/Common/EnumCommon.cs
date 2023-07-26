
namespace Manager.SharedLibs
{
    public enum EnumMediaFileType
    {
        Image = 1,
        Video = 2,
        ConstructImage = 3,
        Other = 99
    }

    public class CommonAction
    {
        public static string Like = "LIKE";
        public static string UnLike = "UNLIKE";
        public static string Share = "SHARE";
        public static string Report = "REPORT";
        public static string Save = "SAVE";
        public static string RatingScore = "RATINGSCORE";
        public static string Comment = "COMMENT";
        public static string CommentReply = "COMMENT_REPLY";
        public static string Follow = "FOLLOW";
    }

    public class CommonObject
    {
        public static string Post = "POST";
        public static string Member = "MEMBER";
        public static string Comment = "COMMENT";
        public static string Video = "VIDEO";
        public static string Photo = "PHOTO";
    }
}
