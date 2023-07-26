using Manager.WebApp.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Web;

namespace Manager.WebApp.Helpers
{
    #region Notification Enums

    public enum EnumNotificationTargetType
    {
        Order = 10,
        Product = 20,
        Customer = 30
    }

    public enum EnumNotificationActionTypeForManager
    {
        ProductCreated = 0,
        ProductUpdated = 1,

        CustomerCreated = 100,
        CustomerUpdated = 101,
        CustomerRegister = 102,

        OrderCreated = 200,
    }

    public enum EnumNotificationActionTypeForCustomer
    {

    }

    #endregion

    public enum EnumSalesType
    {
        [Description("すべて")]
        All = 0,

        [Description("会員")]
        Official = 1
    }

    public enum EnumCourseType
    {
        [Description("すべて公開")]
        All = 0,

        [Description("〇日ごとに公開")]
        Official = 1
    }

    public enum EnumCourseStatus
    {
        [Description("承認待ち")]
        Pending = 0,

        [Description("承認")]
        Active = 1
	}
	
    public enum EnumOrderStatus
    {
        Pending = 0,
        Confirm = 1,
        ShippingPartial = 2,
        ShippingAll = 3,
        Paid = 4,
        Cancel = 9
    }
    public enum EnumNoshiDesign
    {
        [Description("リボン飾りを付けて包装します。")]
        のし無しでラッピング = 0, // No noshi
        [Description("ギフト箱を包装してからのしを貼ります。")]
        外のし = 1, //outside: After wrapping the gift box, attach the noshi.
        [Description("ギフト箱に「のし」を張ってから包装します。（内祝は、内のしをお選びください。※飾れる木箱は、「のし」と「包装」、どちらも対応しておりません。")]
        内のし = 2 //inside: The gift box is covered with "noshi" and then wrapped
    }
    public enum EnumBoxType
    {
        [Description("Type 1 ")]
        BoxType1 = 1,

        [Description("Type 2")]
        BoxType2 = 2,

        [Description("Type 3")]
        BoxType3 = 3,

        [Description("Type 4")]
        BoxType4 = 4,
        [Description("Type 5")]
        BoxType5 = 5,
    }

    public enum EnumCommisionDefaultType
    {
        [Description("プラットフォーム利用料")]
        ForPlatform = 0,

        [Description("販売手数料")]
        ForSeller = 1,

        [Description("動画作成コスト")]
        ForCreator = 2
    }

    public enum EnumStatus
    {
        [LocalizedDescription("LB_ACTIVE", typeof(ManagerResource))]
        Activated = 1,

        [LocalizedDescription("LB_LOCKED", typeof(ManagerResource))]
        Locked = 0
    }

    public enum EnumElementType
    {
        Text = 0,
        Dropdown = 1,
        DropdownGroup=2,
    }

    public enum EnumProjectStatus
    {
        [LocalizedDescription("LB_LOCKED", typeof(ManagerResource))]
        Locked = 0,

        [LocalizedDescription("LB_ACTIVE", typeof(ManagerResource))]
        Activated = 1       
    }

    public static class EnumListCacheKeys
    {
        public static string Accounts = "ILV_ACCOUNTS";
    }

    public static class EnumFormatInfoCacheKeys
    {
        public static string CachePrefix = "HH_";
        public static string JWTAuthToken = CachePrefix + "JWT_{0}";

        //Detail info
        public static string City = CachePrefix + "CITY_{0}";
        public static string Region = CachePrefix+ "REGION_{0}";
        public static string Station = CachePrefix + "STATION_{0}";
        public static string EmailServers = CachePrefix + "EMAIL_SERVERS_{0}";
        public static string EmailSettings = CachePrefix + "EMAIL_SETTINGS_{0}_{1}";
        public static string EmailIdsSynchronized = CachePrefix + "EMAIL_SYNC_{0}_{1}";

        public static string ChicnchillOrder = CachePrefix + "CHICNCHILL_ORDER_{0}";
        public static string AmazonSalesMonth = CachePrefix + "AMAZON_SALES_MONTH_{0}";
        public static string ChicnchillSalesMonth = CachePrefix + "CHICNCHILL_SALES_MONTH_{0}";
        public static string AmazonInventory = CachePrefix + "AMAZON_INVENTORY_{0}";
        public static string AmazonAccessToken = CachePrefix + "AMAZON_ACESSTOKEN";
        public static string AmazonSTSToken = CachePrefix + "AMAZON_STSTOKEN";
        public static string ILVProduct = CachePrefix + "ILV_PRODUCT_{0}";
        public static string ILVProductLine = CachePrefix + "ILV_PRODUCTLINE_{0}";
        public static string SalesAccount = CachePrefix + "ILV_SALESACCOUNT_{0}";
        public static string SalesMonthAccount = CachePrefix + "ILV_SALESMONTH_ACCOUNT_{0}";

        public static string House = CachePrefix + "HOUSE_{0}";
        public static string HouseCategory = CachePrefix + "HOUSE_CAT_{0}";
    }

    public enum EnumSourceCoinType
    {
        [Display(Name = "Tất cả")]
        All = 10,
        [Display(Name = "Mua qua app")]
        AppPurchase = 4,
        [Display(Name = "Dịch vụ gỡ quảng cáo")]
        RemoveAds = 5
    }

    public enum EnumDirection
    {
        [Description("北")]
        North = 0,

        [Description("南")]
        South = 1,

        [Description("ウェスト")]
        West = 2,

        [Description("東")]
        East = 3,

        [Description("北東")]
        NorthEast = 4,

        [Description("北北西")]
        NorthWest = 5,

        [Description("南西")]
        SouthWest = 6,

        [Description("南東")]
        SouthEast = 7
    }

    public enum EnumHouseTransactionMethod
    {
        [Description("売主")]
        DirectWithSeller = 0        
    }

    public enum EnumHouseStatus
    {
        [Description("空室")]
        Empty = 0
    }

    public enum EnumHouseHandoverType
    {
        [Description("相談")]
        Discussion = 0
    }

    public enum EnumHouseNearPlaceType
    {
        [Description("駅頭")]
        Station = 0,

        [Description("高校")]
        HighSchool = 100,

        [Description("中学校")]
        JuniorHighSchool = 101
    }

    public enum EnumEmailScheduleCustomerTargetType
    {
        [Description("Tất cả khách hàng")]
        All = 0,

        [Description("Khách đang xem nhà")]
        FollowingHouse = 10
    }

    public enum EnumEmailSettingTypes
    {
        [LocalizedDescription("LB_EMAIL_OUTGOING", typeof(ManagerResource))]
        OutGoing = 0,

        [LocalizedDescription("LB_EMAIL_INCOMING", typeof(ManagerResource))]
        InComing = 1
    }

    public enum EnumMonth
    {
        [Description("1")]
        January = 1,

        [Description("2")]
        February = 2,

        [Description("3")]
        March = 3,

        [Description("4")]
        April = 4,

        [Description("5")]
        May = 5,

        [Description("6")]
        June = 6,

        [Description("7")]
        July = 7,

        [Description("8")]
        August = 8,

        [Description("9")]
        September = 9,

        [Description("10")]
        October = 10,

        [Description("11")]
        November = 11,

        [Description("12")]
        December = 12,
    }

    public static class EnumMessageTypes
    {
        public static int Text = 1;
        public static int Image = 2;
        public static int File = 3;
    }

    public enum EnumChartType
    {
        Line = 0,
        Column = 1,
        Pie = 2,
        HorizontalBar = 3
    }

    public enum EnumSalesPlatform
    {
        Chicnchill = 0,
        Amazon = 1,
        Etsy = 2,
    }

    public enum EnumSalesReportType
    {
        MonthlySales = 0,
    //    DayOfWeek = 1,
    //    TimeFrame = 2,
    //    Platforms = 3,
    //    Region = 4,
    //    ProductLine = 5,
    //    TopProductLine = 6
    }

    public enum EnumDayOfWeek
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thusday = 5,
        Friday = 6,
        Saturday = 7
    }

    public enum EnumTimeFrameInDay
    {
        _12AM = 0,
        _2AM = 2,
        _4AM = 4,
        _6AM = 6,
        _8AM = 8,
        _10AM = 10,
        _12PM = 12,
        _14PM = 14,
        _16PM = 16,
        _18PM = 18,
        _20PM = 20,
        _22PM = 22

    }

    public enum EnumSalesChartTimeType
    {
        Day = 0,
        Hour = 1
    }

    public enum EnumGender
    {
        [Description("女性")]
        Female = 0,

        [Description("男")]
        Male = 1,
    }

    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;
        private readonly ResourceManager _resource;
        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resource = new ResourceManager(resourceType);
            _resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                string displayName = _resource.GetString(_resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", _resourceKey)
                    : displayName;
            }
        }
    }

    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }    
}