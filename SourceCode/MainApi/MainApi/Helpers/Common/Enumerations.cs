using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApi.Helpers
{
    public static class EnumCommonCode
    {
        public static int Success = 1;
        public static int Error = -1;
        public static int Error_Info_NotFound = -2;
    }

    public static class EnumSearchType
    {
        public static string Keyword = "keyword";
        public static string Address = "address";
    }

    public static class EnumResultType
    {
        public static string File = "file";
        public static string Text = "text";
    }

    public static class EnumSearchEngine
    {
        public static string Google = "google";
        public static string Yahoo = "yahoo";
        public static string Bing = "bing";
    }

    public enum EnumNotificationUserType
    {
        Manager = 0,
        Customer = 1
    }
    public enum SourceCoinType
    {
        PlayGame = 2,
        SkyFlag = 1,
        Attendance = 3,
        InAppPurchase = 4,
        RemoveAds = 5,
        All = 10,
        WatchAds = 6
    }

    public static class NameLinkSetting
    {
        public static string FAQ = "FAQs";
        public static string GoogleForm = "Google Form";
        public static string MyPage = "MyPage";
        public static string News = "News";

    }
}
