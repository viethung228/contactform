using System;
using System.Collections.Generic;

namespace Manager.WebApp.Models
{
    public class AWSOrderResponseModel
    {
        public AWSPayload payload { get; set; }
    }

    public class AWSAutomatedShippingSettings
    {
        public bool HasAutomatedShippingSettings { get; set; }
    }

    public class AWSBuyerInfo
    {
        public string BuyerEmail { get; set; }
    }

    public class AWSDefaultShipFromLocationAddress
    {
        public string StateOrRegion { get; set; }
        public string AddressLine1 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }
    }

    public class AWSOrder
    {
        public AWSBuyerInfo BuyerInfo { get; set; }
        public string AmazonOrderId { get; set; }
        public DateTime EarliestDeliveryDate { get; set; }
        public DateTime EarliestShipDate { get; set; }
        public string SalesChannel { get; set; }
        public AWSAutomatedShippingSettings AutomatedShippingSettings { get; set; }
        public string OrderStatus { get; set; }
        public int NumberOfItemsShipped { get; set; }
        public string OrderType { get; set; }
        public bool IsPremiumOrder { get; set; }
        public bool IsPrime { get; set; }
        public string FulfillmentChannel { get; set; }
        public int NumberOfItemsUnshipped { get; set; }
        public bool HasRegulatedItems { get; set; }
        public string IsReplacementOrder { get; set; }
        public bool IsSoldByAB { get; set; }
        public DateTime LatestShipDate { get; set; }
        public string ShipServiceLevel { get; set; }
        public AWSDefaultShipFromLocationAddress DefaultShipFromLocationAddress { get; set; }
        public bool IsISPU { get; set; }
        public string MarketplaceId { get; set; }
        public DateTime LatestDeliveryDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public AWSShippingAddress ShippingAddress { get; set; }
        public bool IsAccessPointOrder { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsBusinessOrder { get; set; }
        public AWSOrderTotal OrderTotal { get; set; }
        public List<string> PaymentMethodDetails { get; set; }
        public bool IsGlobalExpressEnabled { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string ShipmentServiceLevelCategory { get; set; }
        public string SellerOrderId { get; set; }
    }

    public class AWSOrderTotal
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class AWSPayload
    {
        public List<AWSOrder> Orders { get; set; }
        public string NextToken { get; set; }
        public DateTime CreatedBefore { get; set; }
    }

    public class AWSShippingAddress
    {
        public string StateOrRegion { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
    }
}
