using MainApi.Resources;
using System;
using System.ComponentModel;
using System.Resources;

namespace MainApi.Helpers
{
    public enum EnumNotificationTargetType
    {
        KyoKai = 10,
        Agent = 20,
        Course = 30,
        Seller = 40,
        Customer = 50
    }

    public enum EnumNotificationActionTypeForManager
    {
        CourseCreated = 0,
        CourseUpdated = 1
    }
    public enum EnumNotificationActionTypeForSeller
    {
        SellerCreated = 0,
        SellerUpdated = 1
    }


    public enum EnumNotificationActionTypeForCustomer
    {
        CustomerCreated = 0,
        CustomerUpdated = 1
    }

    public enum EnumNotificationWarningType
    {
        ProgressUnsendFirstEmail = 1
    }

    public enum EnumSellerType
    {
        [Description("Beauty")]
        Beauty = 0,

        [Description("Kyokai")]
        Kyokai = 1,

        [Description("一次代理店")]
        FirstLevelAgent = 2,

        [Description("二次代理店")]
        SecondLevelAgent = 3,

        [Description("Ngoại bang")]
        External = 4,
    }

    public enum EnumCommisionDefaultType
    {
        [Description("Platform")]
        ForPlatform = 0,

        [Description("Chi phí bán")]
        ForSeller = 1,

        [Description("Chi phí tạo video")]
        ForCreator = 2
    }

    public enum EnumOrderStatus
    {
        Waiting = 0,
        Paid = 1
    }

    public enum EnumPaymentMethod
    {
        Bank = 0,
        CashOnDelivery = 1,
        Credit = 2
    }

    public enum EnumCourseStatus
    {
        [Description("Chờ duyệt")]
        Pending = 0,

        [Description("Đã duyệt")]
        Active = 1
    }

    public enum EnumPaymentStatus
    {
        Waiting = 0,
        Paid = 1
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

    public enum EnumEmailScheduleCustomerTargetType
    {
        [Description("Tất cả khách hàng")]
        All = 0,

        [Description("Khách đang xem nhà")]
        FollowingHouse = 10
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
        public static string CachePrefix = "CONTACT_FORM_";
        public static string Accounts = CachePrefix + "ACCOUNTS";
        public static string RegionByCountry = CachePrefix +  "REGIONS_{0}";
        public static string PrefecturesByRegion = CachePrefix + "PREFECTURES_{0}";
        public static string PrefecturesByCountry = CachePrefix + "PREFECTURES_BY_COUNTRY_{0}";
        public static string CitiesByPrefecture = CachePrefix + "CITIES_{0}";

        public static string HouseCategories = CachePrefix + "HOUSECATS";
        public static string ProgressStatuses = CachePrefix + "PROGRESS_STATUSES";
        public static string EmailSchedules = CachePrefix + "EMAIL_TEMPLATES";
        public static string EmailForms = CachePrefix + "EMAIL_FORMS";

        public static string Categories = CachePrefix + "CATEGORIES_";
        public static string Coins = CachePrefix + "COINS_";
        public static string LinkSettings = CachePrefix + "LINKSETTINGS_";
        public static string Adss = CachePrefix + "ADSS_";
        public static string Users = CachePrefix + "USERS_";
        public static string ProductProperties = CachePrefix + "PRODUCTS_PROPERTY_";
        public static string Volumes = CachePrefix + "VOLUMES_";
        public static string Brands = CachePrefix + "BRANDS_";
        public static string Origins = CachePrefix + "ORIGINS_";
        public static string Boxes = CachePrefix + "BOXES_";
        public static string Companies = CachePrefix + "COMPANIES_";
        public static string Events = CachePrefix + "EVENTS_";
        public static string Properties = CachePrefix + "PROPETIES_";
        public static string PropertyValues = CachePrefix + "PROPERTY_VALUES_";
        public static string Noshies = CachePrefix + "NOSHIES_";
        public static string MessageCards = CachePrefix + "MESSAGECARDS_";
        public static string RecipientInfos = CachePrefix + "RECIPIENT_INFOs_";
        public static string Bottles = CachePrefix + "BOTTLE_INFOs_";
        public static string Products = CachePrefix + "PRODUCT_INFOs_";
        public static string CategoryProperties = CachePrefix + "CATEGORY_PROPERTY_INFOs_";
    }

    public static class EnumFormatInfoCacheKeys
    {
        public static string CachePrefix = "OGATORE_HIT_";
        //Detail info
        public static string City = CachePrefix + "CITY_{0}";
        public static string Coin = CachePrefix + "COIN_{0}";
        public static string LinkSetting = CachePrefix + "LINKSETTING_{0}";
        public static string Ads = CachePrefix + "ADS_{0}";
        public static string Region = CachePrefix + "REGION_{0}";
        public static string Prefecture = CachePrefix + "PREFECTURE_{0}";
        public static string Station = CachePrefix + "STATION_{0}";
        public static string EmailServers = CachePrefix + "EMAIL_SERVERS_{0}";
        public static string EmailSettings = CachePrefix + "EMAIL_SETTINGS_{0}_{1}";
        public static string EmailIdsSynchronized = CachePrefix + "EMAIL_SYNC_{0}_{1}";
        public static string User = CachePrefix + "USER_{0}";
        public static string SalesAccount = CachePrefix + "SALESACCOUNT_{0}";
        public static string SalesMonthAccount = CachePrefix + "SALESMONTH_ACCOUNT_{0}";

        public static string Employee = CachePrefix + "EMPLOYEE_{0}";
        public static string ProgressStatus = CachePrefix + "PROGRESS_STATUS_{0}";
        public static string EmailSchedule = CachePrefix + "EMAIL_TEMPLATE_{0}";
        public static string EmailForm = CachePrefix + "EMAILFORM_{0}";
        public static string EmailFormComponents = CachePrefix + "EMAILFORM_COMPS_{0}";
        public static string LineUser = CachePrefix + "LINE_USER_{0}";


        public static string Product = CachePrefix + "Product_{0}";
        public static string Volume = CachePrefix + "Volume_{0}";
        public static string Category = CachePrefix + "CATEGORY_{0}";
        public static string Label = CachePrefix + "LABEL_{0}";
        public static string Event = CachePrefix + "EVENT_{0}";
        public static string Property = CachePrefix + "PROPERTY_{0}";
        public static string PropertyValue = CachePrefix + "PROPERTY_VALUE_{0}";
        public static string ProductProperty = CachePrefix + "PRODUCT_PROPERTY_{0}";
        public static string CategoryProperty = CachePrefix + "CATEGORY_PROPERTY_{0}";

        public static string BankAccount = CachePrefix + "BANK_ACCOUNT_{0}";
        public static string Order = CachePrefix + "ORDER_{0}";
        public static string OrderItem = CachePrefix + "ORDER_ITEM_{0}";
        public static string Origin = CachePrefix + "ORIGIN_{0}";
        public static string Brand = CachePrefix + "BRAND_{0}";
        public static string Box = CachePrefix + "BOX_{0}";
        public static string Company = CachePrefix + "COMPANY_{0}";
        public static string RecipientInfo = CachePrefix + "RECIPIENT_INFO_{0}";
        public static string Noshi = CachePrefix + "NOSHI_{0}";
        public static string MessageCard = CachePrefix + "MESSAGE_CARD_{0}";
        public static string Bottle = CachePrefix + "BOTTLE_{0}";
    }

    public enum EnumDirection
    {
        North = 0,
        South = 1,
        West = 2,
        East = 3,
        NorthEast = 4,
        NorthWest = 5,
        SouthWest = 6,
        SouthEast = 7
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
        DayOfWeek = 1,
        TimeFrame = 2,
        Platforms = 3,
        Region = 4,
        ProductLine = 5,
        TopProductLine = 6
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