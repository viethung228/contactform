using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApi.Models
{
    public class AWSOrderItemBuyerInfo
    {
    }

    public class AWSOrderItemItemPrice
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class AWSOrderItemItemTax
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class AWSOrderItemOrderItem
    {
        public AWSOrderItemTaxCollection TaxCollection { get; set; }
        public AWSOrderItemProductInfo ProductInfo { get; set; }
        public AWSOrderItemBuyerInfo BuyerInfo { get; set; }
        public AWSOrderItemItemTax ItemTax { get; set; }
        public int QuantityShipped { get; set; }
        public AWSOrderItemItemPrice ItemPrice { get; set; }
        public string ASIN { get; set; }
        public string SellerSKU { get; set; }
        public string Title { get; set; }
        public string IsGift { get; set; }
        public bool IsTransparency { get; set; }
        public int QuantityOrdered { get; set; }
        public AWSOrderItemPromotionDiscountTax PromotionDiscountTax { get; set; }
        public AWSOrderItemPromotionDiscount PromotionDiscount { get; set; }
        public string OrderItemId { get; set; }
    }

    public class AWSOrderItemPayload
    {
        public List<AWSOrderItemOrderItem> OrderItems { get; set; }
        public string AmazonOrderId { get; set; }
    }

    public class AWSOrderItemProductInfo
    {
        public string NumberOfItems { get; set; }
    }

    public class AWSOrderItemPromotionDiscount
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class AWSOrderItemPromotionDiscountTax
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class AWSOrderItemModel
    {
        public AWSOrderItemPayload payload { get; set; }
    }

    public class AWSOrderItemTaxCollection
    {
        public string Model { get; set; }
        public string ResponsibleParty { get; set; }
    }


}
