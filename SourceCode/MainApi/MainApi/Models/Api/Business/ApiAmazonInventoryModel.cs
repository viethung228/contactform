using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApi.Models
{
    #region AWSInventory

    public class AWSInventoryGranularity
    {
        public string granularityType { get; set; }
        public string granularityId { get; set; }
    }

    public class AWSInventoryInventorySummary
    {
        public string asin { get; set; }
        public string fnSku { get; set; }
        public string sellerSku { get; set; }
        public string condition { get; set; }
        public object lastUpdatedTime { get; set; }
        public string productName { get; set; }
        public int totalQuantity { get; set; }
        public AWSInventoryDetail inventoryDetails { get; set; }
    }

    public class AWSInventoryDetailFutureSupplyQuantity
    {
        public int reservedFutureSupplyQuantity { get; set; }
        public int futureSupplyBuyableQuantity { get; set; }
    }

    public class AWSInventoryDetailResearchingQuantity
    {
        public int totalResearchingQuantity { get; set; }
        public List<AWSInventoryDetailResearchingQuantityBreakdown> researchingQuantityBreakdown { get; set; }
        public int researchingQuantityInShortTerm { get; set; }
        public int researchingQuantityInMidTerm { get; set; }
        public int researchingQuantityInLongTerm { get; set; }
    }

    public class AWSInventoryDetailResearchingQuantityBreakdown
    {
        public string name { get; set; }
        public int quantity { get; set; }
    }

    public class AWSInventoryDetailReservedQuantity
    {
        public int totalReservedQuantity { get; set; }
        public int pendingCustomerOrderQuantity { get; set; }
        public int pendingTransshipmentQuantity { get; set; }
        public int fcProcessingQuantity { get; set; }
    }

    public class AWSInventoryDetail
    {
        public int fulfillableQuantity { get; set; }
        public int inboundWorkingQuantity { get; set; }
        public int inboundShippedQuantity { get; set; }
        public int inboundReceivingQuantity { get; set; }
        public AWSInventoryDetailReservedQuantity reservedQuantity { get; set; }
        public AWSInventoryDetailResearchingQuantity researchingQuantity { get; set; }
        public AWSInventoryDetailUnfulfillableQuantity unfulfillableQuantity { get; set; }
        public AWSInventoryDetailFutureSupplyQuantity futureSupplyQuantity { get; set; }
    }

    public class AWSInventoryDetailUnfulfillableQuantity
    {
        public int totalUnfulfillableQuantity { get; set; }
        public int customerDamagedQuantity { get; set; }
        public int warehouseDamagedQuantity { get; set; }
        public int distributorDamagedQuantity { get; set; }
        public int carrierDamagedQuantity { get; set; }
        public int defectiveQuantity { get; set; }
        public int expiredQuantity { get; set; }
    }

    public class AWSInventoryPagination
    {
        public string nextToken { get; set; }
    }

    public class AWSInventoryPayload
    {
        public AWSInventoryGranularity granularity { get; set; }
        public List<AWSInventoryInventorySummary> inventorySummaries { get; set; }
    }

    public class AWSInventoryResponseModel
    {
        public AWSInventoryPagination pagination { get; set; }
        public AWSInventoryPayload payload { get; set; }
    }

    public class ApiAmazonInventoryModel
    {
        public string details { get; set; }
        public string granularityType { get; set; }
        public string granularityId { get; set; }
        public string startDateTime { get; set; }
        public string sellerSkus { get; set; }
        public string marketplaceIds { get; set; }
        public string MaxResultsPerPage { get; set; }
        public string nextToken { get; set; }
    }

    #endregion
}
