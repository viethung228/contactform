using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.WebApp.Helpers
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
    }

    public static class EnumJapaneseImperialNengo
    {
        [Description("令和")]
        public static string Reiwa = "reiwa";
        [Description("平成")]
        public static string Heisei = "heisei";
        [Description("昭和")]
        public static string Shouwa = "shouwa";
        [Description("大正")]
        public static string Taishou = "taishou";
    }
}
