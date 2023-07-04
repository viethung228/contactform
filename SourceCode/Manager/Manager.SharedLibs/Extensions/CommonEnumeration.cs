using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.SharedLib
{
    public enum ShareLibEnumTableType
    {
        AllUser = 0,
        User = 1,
        Order = 2,
        Response = 3,
    }

    public enum ShareLibEnumEventFilterType
    {
        MessageContent = 1,
        BuyProduct = 2,

    }

    public enum ShareLibEnumTableUserField
    {
        NewFriend =0,
        LoggedInDone = 1,
        //LoggedInUser = 2,
        AddFriendDate = 2,
        Sex = 3,
        Birthday = 4,
        Month = 5,
        Phone = 6,
        Email = 7,
        HomePrefecture = 8,
        MemberName = 9,
        MemberNameKana = 10,
    }

    public enum ShareLibEnumTableOrderType
    {
        Buy = 0,
        NotBuy = 1,
    }

    public enum ShareLibEnumTableOrderProductType
    {
        AllProduct = 0,
        DetailProduct = 1,
    }

    public enum ShareLibEnumTableOrderProductOperator
    {
        Buy = 0,

        [Description("<")]
        BuyLessThan = 1,

        [Description(">")]
        BuyMoreThan = 2,

        [Description("=")]
        BuyEqual = 3
    }

    public enum ShareLibEnumTableOrderOperator
    {
        Buy = 0,

        [Description("<")]
        BuyLessThan = 1,

        [Description(">")]
        BuyMoreThan = 2,

        [Description("=")]
        BuyEqual = 3
    }

    public enum ShareLibEnumTableResponseAnswerType
    {
        AllAnswer = 1,
        DetailAnswer = 2,
    }

    public enum ShareLibEnumTableResponseType
    {
        ResponseUsers = 0,
        AnswerQuestionDone = 1,
        AnswerQuestion = 2,
        AccessLink = 3,
        EventResponse = 4,
        EventNotResponse = 5,
    }

    public enum ShareLibEnumFilterDateTimeType
    {
        NoApply = 0,
        Period = 1,
        DaysAgo = 2
    }

    public enum ShareLibEnumEventFilterOperator
    {
        [Description("")]
        All = 0,

        [Description("=")]
        Equal = 1,

        [Description("<")]
        LessThan = 2,

        [Description("<=")]
        LessThanOrEqual = 3,

        [Description(">")]
        MoreThan = 4,

        [Description(">=")]
        MoreThanOrEqual = 5
    }

    public enum ShareLibEnumProductFilterType
    {
        AllProducts = 0,
        ProductDetail = 1,
    }

    public enum ShareLibEnumProductFilterOperator
    {
        [Description("")]
        NotBuy = 0,

        [Description("")]
        Buy = 1,

        [Description("<")]
        BuyLessThan = 2,

        [Description(">")]
        BuyMoreThan = 3,

        [Description("=")]
        BuyEqual = 4
    }

    public enum ShareLibEnumEventFilterDataType
    {
        Number = 0,
        Text = 1,
        //DateOnly = 2,
    }

    public enum ShareLibWarehouse
    {
        BOSS = 0,
        AMAZON = 1
    }

    public enum ShareLibEnumSortColumnBoss
    {
        Inventory = 1,
        Makeshop = 2,
        Rakuten = 3,
    }

    public static class ShareLibEnumExtensions
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
