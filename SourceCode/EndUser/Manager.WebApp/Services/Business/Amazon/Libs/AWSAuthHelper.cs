using System;
using RestSharp;

namespace Manager.WebApp.Services.Amazon
{
    public class AWSAuthHelper
    {
        #region Prop
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }
        public Uri Endpoint { get; set; }
        public string AccessKeyId { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public string Resource { get; set; }
        public string SessionToken { get; set; }
        #endregion
        public AWSAuthHelper(string clientId,string clientSecret,string refreshToken,Uri endPoint,string accessKeyId,string secretKey,string region)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RefreshToken = refreshToken;
            Endpoint = endPoint;
            AccessKeyId = accessKeyId;
            SecretKey = secretKey;
            Region = region;
        }

        public IRestRequest SignRequest(IRestRequest restRequest, RestClient restClient, string contentType, string region, string serviceName)
        {
            // Seller APIs
            LWAAuthorizationCredentials lwaAuthorizationCredentials = new LWAAuthorizationCredentials
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                RefreshToken = RefreshToken,
                Endpoint = Endpoint
            };
            AWSAuthenticationCredentials awsAuthenticationCredentials = new AWSAuthenticationCredentials
            {
                AccessKeyId = AccessKeyId,
                SecretKey = SecretKey,
                Region = region,
                
            };

            restRequest = new AWSSigV4Signer(awsAuthenticationCredentials)
                .Sign(restRequest, restClient.BaseUrl.Host, serviceName);

            restRequest.AddHeader("Content-type", contentType);

            restRequest.AddHeader("user-agent", "My YouTube App 1.0 (Language=csharp;Platform=Windows/10)");

            return restRequest;
        }

        public string GetToken()
        {
            LWAAuthorizationCredentials lwaAuthorizationCredentials = new LWAAuthorizationCredentials
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                RefreshToken = RefreshToken,
                Endpoint = Endpoint
            };

            var requestTokenClient = new LWAClient(lwaAuthorizationCredentials);

            return requestTokenClient.GetAccessToken();

        }
    }
}
