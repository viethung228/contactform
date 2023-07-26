using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.WebApp.Models
{
    #region AWSOrderMetrics
    public class AWSOrderMetricsResponseModel
    {
        public List<AWSOrderMetricsPayload> payload { get; set; }
    }

    public class ApiAmazonSalesModel 
    {
        public string marketplaceIds { get; set; }
        public string granularity { get; set; }
        public string interval { get; set; }
        public string granularityTimeZone { get; set; }
        public string buyerType { get; set; }
        public string fulfillmentNetwork { get; set; }
        public string firstDayOfWeek { get; set; }
        public string asin { get; set; }
        public string sku { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AWSOrderMetricsAverageUnitPrice
    {
        public double amount { get; set; }
        public string currencyCode { get; set; }
    }

    public class AWSOrderMetricsPayload
    {
        public string interval { get; set; }
        public int unitCount { get; set; }
        public int orderItemCount { get; set; }
        public int orderCount { get; set; }
        public AWSOrderMetricsAverageUnitPrice averageUnitPrice { get; set; }
        public AWSOrderMetricsTotalSales totalSales { get; set; }
    }
    

    public class AWSOrderMetricsTotalSales
    {
        public double amount { get; set; }
        public string currencyCode { get; set; }
    }

    #endregion

    
}
