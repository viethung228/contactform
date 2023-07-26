//using Manager.SharedLibs;
//using Manager.WebApp.Models;
//using Newtonsoft.Json;
//using System;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using RestSharp;
//using Serilog;
//using Manager.WebApp.Services.Amazon;
//using System.Web;
//using Manager.WebApp.Helpers;
//using MainApi.DataLayer.Entities;

//namespace Manager.WebApp.Services
//{
//    public class AmazonServices
//    {
//        private static readonly ILogger _logger = Log.ForContext(typeof(AmazonServices));
//        private const string AWSSellingPartnerApiEndPoint = "https://sellingpartnerapi-na.amazon.com";
//        static string content_form_urlencoded = "application/x-www-form-urlencoded";
//        static string AWSApiPathOrders = "/orders/v0/orders";
//        static string AWSApiPathOrderMetrics = "/sales/v1/orderMetrics";
//        static string AWSApiPathInventory = "/fba/inventory/v1/summaries";

//        public static STSTAccessToken GenerateTemporarySTS(AWSAuthHelper authInfo, IdentitySalesAccount accountInfo)
//        {

//            var restClient = new RestClient("https://sts.amazonaws.com");

//            IRestRequest restRequest = new RestRequest("/", Method.POST);

//            restRequest.AddHeader("Accept", "application/json");

//            restRequest.AddQueryParameter("Version", "2011-06-15");
//            restRequest.AddQueryParameter("Action", "AssumeRole");
//            restRequest.AddQueryParameter("RoleSessionName", "SellingSTSToken");

//            if (accountInfo != null)
//            {
//                restRequest.AddQueryParameter("RoleArn", accountInfo.AmazonSellingPartnerRole);
//            }
//            else
//            {
//                restRequest.AddQueryParameter("RoleArn", "arn:aws:iam::449026397127:role/SellingPartnerRole");
//            }
//            restRequest.AddQueryParameter("DurationSeconds", "3600");

//            var request = authInfo.SignRequest(restRequest, restClient, content_form_urlencoded, "us-east-1", "sts");

//            try
//            {
//                var result = restClient.Execute(request);

//                var tk = JsonConvert.DeserializeObject<STSTAccessToken>(result.Content);

//                return tk;
//            }
//            catch (Exception e)
//            {

//            }

//            return null;
//        }

//        public static AWSAuthHelper InitAWSAuthInfo(IdentitySalesAccount accountInfo = null)
//        {
//            var clientId = string.Empty;
//            var clientSecret = string.Empty;
//            var refreshToken = string.Empty;
//            var accessKey = string.Empty;
//            var secretKey = string.Empty;

//            if (accountInfo != null)
//            {
//                clientId = accountInfo.AmazonClientId;
//                clientSecret = accountInfo.AmazonClientSecret;
//                refreshToken = accountInfo.AmazonRefreshToken;
//                accessKey = accountInfo.AmazonAccessKey;
//                secretKey = accountInfo.AmmazonSecretKey;
//            }
//            else
//            {
//                clientId = @"amzn1.application-oa2-client.fbad0e299fac461d9e161888c7ce4b6a";
//                clientSecret = @"7fd8f354e3a61722fb735e9c84c838b7320f146dc86c494978b943991e057207";
//                refreshToken = @"Atzr|IwEBIMmtvrZ1s0Wn79XIGxUQyjII171NiQcX5phpS03Ua6xWUnk5cCjvZs64TZ-ciI0nWbTUkoiWt-8ZXhSQ1pUkr8vZv5YDZuuhuCU4C-Oa0LQQF585nyYn1l1FNHa-DplsPtxyX8CezOelKLveWo9K0A4DKqAGij0vqlIf2GbxzYmB6TNBVgdBEEmfxE8JP91kGBDVHQz0D_iyGu-g6vGpXx1Km4P_tdkgpGo_mTlJK3T5AQWtjmQ0JIhqk9Zae0WW3dYSS_Wpk1R53hYcUhPhuY-3dGWDnaZCVMPltJrKPHPOQ7-hXxa7OS9eqEuOAQa5fpU";
//                accessKey = @"AKIAWRDAPCPD75JIPYVV";
//                secretKey = @"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";
//            }

//            var awsApiAuthEndPoint = @"https://api.amazon.com/auth/o2/token";

            
//            var region = @"us-east-1";

//            var authInfo = new AWSAuthHelper(
//                 clientId, clientSecret, refreshToken, new Uri(awsApiAuthEndPoint),
//                 accessKey, secretKey, region
//            );

//            return authInfo;
//        }

//        public static async Task<AWSOrderResponseModel> GetOrdersAsync(ApiOrderModel apiModel, IdentitySalesAccount accountInfo)
//        {
//            AWSOrderResponseModel returnModel = null;
//            try
//            {
//                var authInfo = InitAWSAuthInfo(accountInfo);
//                var acccessToken = HelperAmazon.GetCurrentAccessToken(authInfo);
//                var stsToken = HelperAmazon.GetCurrentSTSToken(authInfo, accountInfo);

//                var restClient = new RestClient(AWSSellingPartnerApiEndPoint);

//                IRestRequest restRequest = new RestRequest(AWSApiPathOrders, Method.GET);

//                restRequest.AddHeader("Accept", "application/json");
//                restRequest.AddHeader("X-Amz-Security-Token", authInfo.SessionToken);
//                restRequest.AddHeader("x-amz-access-token", acccessToken);

//                restRequest.AddQueryParameter("MarketplaceIds", apiModel.MarketplaceIds);
//                restRequest.AddQueryParameter("CreatedAfter", apiModel.CreatedAfter);
//                restRequest.AddQueryParameter("MaxResultsPerPage", apiModel.MaxResultsPerPage.ToString());
//                restRequest.AddQueryParameter("Details", "true");

//                if (!string.IsNullOrEmpty(apiModel.CreatedBefore))
//                {
//                    restRequest.AddQueryParameter("CreatedBefore", apiModel.CreatedBefore);
//                }

//                if (!string.IsNullOrEmpty(apiModel.NextToken))
//                {
//                    restRequest.AddQueryParameter("NextToken", apiModel.NextToken);
//                }

//                var request = authInfo.SignRequest(restRequest, restClient, content_form_urlencoded, "us-east-1", "execute-api");

//                var result = await restClient.ExecuteAsync(request);

//                if (result.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    returnModel = JsonConvert.DeserializeObject<AWSOrderResponseModel>(result.Content);
//                }
//                else if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
//                {

//                }
//            }
//            catch (Exception ex)
//            {
//                var strError = string.Format("Failed when GetOrdersAsync because: {0}", ex.ToString());
//                _logger.Error(strError);
//            }

//            return returnModel;
//        }

//        public static async Task<AWSOrderMetricsResponseModel> GetOrderMetricsAsync(ApiAmazonSalesModel apiModel, IdentitySalesAccount accountInfo)
//        {
//            AWSOrderMetricsResponseModel returnModel = null;
//            try
//            {
//                var authInfo = InitAWSAuthInfo(accountInfo);
//                var acccessToken = HelperAmazon.GetCurrentAccessToken(authInfo);
//                var stsToken = HelperAmazon.GetCurrentSTSToken(authInfo, accountInfo);

//                var restClient = new RestClient(AWSSellingPartnerApiEndPoint);

//                IRestRequest restRequest = new RestRequest(AWSApiPathOrderMetrics, Method.GET);

//                restRequest.AddHeader("Accept", "application/json");
//                restRequest.AddHeader("X-Amz-Security-Token", authInfo.SessionToken);
//                restRequest.AddHeader("x-amz-access-token", acccessToken);

//                restRequest.AddQueryParameter("marketplaceIds", apiModel.marketplaceIds);
//                restRequest.AddQueryParameter("interval", apiModel.interval);
//                restRequest.AddQueryParameter("granularity", apiModel.granularity);

//                var request = authInfo.SignRequest(restRequest, restClient, content_form_urlencoded, "us-east-1", "execute-api");

//                var result = await restClient.ExecuteAsync(request);

//                if (result.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    returnModel = JsonConvert.DeserializeObject<AWSOrderMetricsResponseModel>(result.Content);
//                }
//                else if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
//                {

//                }
//            }
//            catch (Exception ex)
//            {
//                var strError = string.Format("Failed when GetOrdersAsync because: {0}", ex.ToString());
//                _logger.Error(strError);
//            }

//            return returnModel;
//        }

//        public static async Task<AWSInventoryResponseModel> GetInventoryAsync(ApiAmazonInventoryModel apiModel, IdentitySalesAccount accountInfo)
//        {
//            AWSInventoryResponseModel returnModel = null;
//            try
//            {
//                var authInfo = InitAWSAuthInfo(accountInfo);
//                var acccessToken = HelperAmazon.GetCurrentAccessToken(authInfo);
//                var stsToken = HelperAmazon.GetCurrentSTSToken(authInfo, accountInfo);

//                var restClient = new RestClient(AWSSellingPartnerApiEndPoint);

//                IRestRequest restRequest = new RestRequest(AWSApiPathInventory, Method.GET);

//                restRequest.AddHeader("Accept", "application/json");
//                restRequest.AddHeader("X-Amz-Security-Token", authInfo.SessionToken);
//                restRequest.AddHeader("x-amz-access-token", acccessToken);

//                restRequest.AddQueryParameter("marketplaceIds", apiModel.marketplaceIds);
//                restRequest.AddQueryParameter("granularityType", apiModel.granularityType);
//                restRequest.AddQueryParameter("granularityId", apiModel.granularityId);
//                restRequest.AddQueryParameter("details", (!string.IsNullOrEmpty(apiModel.details) && apiModel.details == "true") ? "true" : "false");

//                if (!string.IsNullOrEmpty(apiModel.sellerSkus))
//                {
//                    restRequest.AddQueryParameter("sellerSkus", apiModel.sellerSkus);
//                }

//                if (!string.IsNullOrEmpty(apiModel.nextToken))
//                {
//                    var encodedToken = HttpUtility.UrlEncode(apiModel.nextToken);
//                    restRequest.AddQueryParameter("nextToken", apiModel.nextToken);
//                }

//                var request = authInfo.SignRequest(restRequest, restClient, content_form_urlencoded, "us-east-1", "execute-api");

//                var result = await restClient.ExecuteAsync(request);

//                if (result.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    returnModel = JsonConvert.DeserializeObject<AWSInventoryResponseModel>(result.Content);
//                }
//                else if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
//                {

//                }
//            }
//            catch (Exception ex)
//            {
//                var strError = string.Format("Failed when GetInventoryAsync because: {0}", ex.ToString());
//                _logger.Error(strError);
//            }

//            return returnModel;
//        }

//        public static async Task<AWSOrderItemModel> GetOrderItemAsync(string amazonOrderId, IdentitySalesAccount accountInfo)
//        {
//            AWSOrderItemModel returnModel = null;
//            try
//            {
//                var authInfo = InitAWSAuthInfo(accountInfo);
//                var acccessToken = HelperAmazon.GetCurrentAccessToken(authInfo);
//                var stsToken = HelperAmazon.GetCurrentSTSToken(authInfo, accountInfo);

//                var restClient = new RestClient(AWSSellingPartnerApiEndPoint);

//                var apiPath = string.Format("{0}/{1}/orderItems", AWSApiPathOrders, amazonOrderId);
//                IRestRequest restRequest = new RestRequest(apiPath, Method.GET);

//                restRequest.AddHeader("Accept", "application/json");
//                restRequest.AddHeader("X-Amz-Security-Token", authInfo.SessionToken);
//                restRequest.AddHeader("x-amz-access-token", acccessToken);

//                var request = authInfo.SignRequest(restRequest, restClient, content_form_urlencoded, "us-east-1", "execute-api");

//                var result = await restClient.ExecuteAsync(request);

//                if (result.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    returnModel = JsonConvert.DeserializeObject<AWSOrderItemModel>(result.Content);
//                }
//                else if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
//                {

//                }
//            }
//            catch (Exception ex)
//            {
//                var strError = string.Format("Failed when GetOrderItemAsync because: {0}", ex.ToString());
//                _logger.Error(strError);
//            }

//            return returnModel;
//        }

//        private static void HttpStatusCodeTrace(HttpResponseMessage response)
//        {
//            var statusCode = Utils.ConvertToInt32(response.StatusCode);
//            if (statusCode != (int)HttpStatusCode.OK)
//            {
//                _logger.Error(string.Format("Return code: {0}, message: {1}", response.StatusCode, response.RequestMessage.RequestUri));
//            }
//        }
//    }
//}