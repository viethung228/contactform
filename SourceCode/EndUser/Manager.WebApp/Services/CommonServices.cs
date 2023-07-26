using Autofac;
using Manager.WebApp.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestSharp;
using RestSharp.Authenticators;
using System.Linq;
using System.Web;
using Ubiety.Dns.Core;

namespace Manager.WebApp.Services
{
    public class CommonServices
    {
        public static void MainApiAuthorization(RestClient client)
        {
            //var myKey = string.Format(EnumFormatInfoCacheKeys.JWTAuthToken, CommonHelpers.GetCurrentStaffId());
            //var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

            ////Get from cache
            //var authToken = cacheProvider.Get<string>(myKey);

            var myKey = ".AspNetCore.JWT";
            var authToken = CommonHelpers.GetCookie(myKey);
            if (!string.IsNullOrEmpty(authToken) )
            {
                client.Authenticator = new JwtAuthenticator(authToken);
            }
            else
            {
                if (HttpContext.Current.Request.Cookies.Count > 0)
                {
                    var siteCookies = HttpContext.Current.Request.Cookies.Where(c => c.Key.Contains(".AspNetCore.") || c.Key.Contains("Microsoft.Authentication"));
                    foreach (var cookie in siteCookies)
                    {
                        HttpContext.Current.Response.Cookies.Delete(cookie.Key);
                    }
                }
            }
        }

        //public static HttpClient AuthorizationHttpClientCore(this HttpClient client)
        //{
        //    //Set Basic Auth
        //    var user = (!string.IsNullOrEmpty(AuthorizationCoreSettings.Username)) ? AuthorizationCoreSettings.Username : "username";
        //    var password = (!string.IsNullOrEmpty(AuthorizationCoreSettings.Password)) ? AuthorizationCoreSettings.Password : "password";
        //    var authorizeHeaderKey = (!string.IsNullOrEmpty(AuthorizationCoreSettings.HeaderKey)) ? AuthorizationCoreSettings.HeaderKey : "key";
        //    var authorizeHeaderValue = (!string.IsNullOrEmpty(AuthorizationCoreSettings.HeaderValue)) ? AuthorizationCoreSettings.HeaderValue : "value";

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);

        //    client.DefaultRequestHeaders.Add(authorizeHeaderKey, authorizeHeaderValue);

        //    return client;
        //}

        //public static HttpClient AuthorizationHttpCustomerCore(this HttpClient client)
        //{
        //    //Set Basic Auth
        //    var user = (!string.IsNullOrEmpty(AuthorizationCustomerCoreSettings.Username)) ? AuthorizationCustomerCoreSettings.Username : "username";
        //    var password = (!string.IsNullOrEmpty(AuthorizationCustomerCoreSettings.Password)) ? AuthorizationCustomerCoreSettings.Password : "password";
        //    var authorizeHeaderKey = (!string.IsNullOrEmpty(AuthorizationCustomerCoreSettings.HeaderKey)) ? AuthorizationCustomerCoreSettings.HeaderKey : "key";
        //    var authorizeHeaderValue = (!string.IsNullOrEmpty(AuthorizationCustomerCoreSettings.HeaderValue)) ? AuthorizationCustomerCoreSettings.HeaderValue : "value";

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);

        //    client.DefaultRequestHeaders.Add(authorizeHeaderKey, authorizeHeaderValue);

        //    return client;
        //}

        //public static HttpClient AuthorizationHttpClientSocial(this HttpClient client)
        //{
        //    //Set Basic Auth
        //    var user = (!string.IsNullOrEmpty(AuthorizationSocialSettings.Username)) ? AuthorizationSocialSettings.Username : "username";
        //    var password = (!string.IsNullOrEmpty(AuthorizationSocialSettings.Password)) ? AuthorizationSocialSettings.Password : "password";
        //    var authorizeHeaderKey = (!string.IsNullOrEmpty(AuthorizationSocialSettings.HeaderKey)) ? AuthorizationSocialSettings.HeaderKey : "key";
        //    var authorizeHeaderValue = (!string.IsNullOrEmpty(AuthorizationSocialSettings.HeaderValue)) ? AuthorizationSocialSettings.HeaderValue : "value";

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);

        //    client.DefaultRequestHeaders.Add(authorizeHeaderKey, authorizeHeaderValue);

        //    return client;
        //}

        //public static RestRequest RakutenAuthorization(this RestRequest request)
        //{

        //    var secretKey = AppConfiguration.GetAppsetting("Rakuten:ApiSecretKey"];
        //    var licenseKey = AppConfiguration.GetAppsetting("Rakuten:ApiLicenseKey"];

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{secretKey}:{licenseKey}"));
        //    request.AddHeader("Authorization", string.Format("ESA {0}", base64String));

        //    return request;
        //}

        //public static HttpClient RakutenAuthorization(this HttpClient request)
        //{

        //    var secretKey = AppConfiguration.GetAppsetting("Rakuten:ApiSecretKey"];
        //    var licenseKey = AppConfiguration.GetAppsetting("Rakuten:ApiLicenseKey"];

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{secretKey}:{licenseKey}"));

        //    request.DefaultRequestHeaders.Add("Authorization", string.Format("ESA {0}", base64String));

        //    return request;
        //}        
    }
}