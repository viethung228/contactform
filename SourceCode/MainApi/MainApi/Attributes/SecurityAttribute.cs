using MainApi.Helpers;
using MainApi.Resources;
using MainApi.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MainApi
{
    public static class ObjectToDictionaryHelper
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }
    }    

    //This is a static helper class which creates the URL Hash
    public static class TokenUtility
    {
        private static readonly char[] padding = { '=' };

       
        public static RouteValueDictionary QueryStringAsRouteValueDictionary(ActionExecutingContext context)
        {
            var query = context.HttpContext.Request.Query;

            NameValueCollection fixedQueryString = new NameValueCollection();
            foreach (string key in query.Keys)
            {
                string value = query[key];
                if (value.Contains(","))
                {
                    value = value.Split(',')[0];
                }
                fixedQueryString.Add(key, value);
            }
            
            return fixedQueryString.AllKeys.Aggregate(new RouteValueDictionary(context.RouteData.Values),
                (rvd, k) =>
                {
                    rvd.Add(k, fixedQueryString[k]);
                    return rvd;
                });
        }

        public static bool IsValidToken(RouteValueDictionary requestUrlParts, string password, ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            var controllerName = string.Empty;
            try
            {
                controllerName = Convert.ToString(requestUrlParts["controller"]);
            }
            catch
            {
                controllerName = "";
            }

            string actionName = Convert.ToString(requestUrlParts["action"]);

            string token = Convert.ToString(requestUrlParts["tk"]);

            if (string.IsNullOrEmpty(token))
            {
                if (request.Method == "POST")
                {
                    token = request.Query["tk"];
                }
            }

            //Compute a new hash
            var myParams = requestUrlParts;
            myParams.Remove("controller");
            myParams.Remove("action");
            myParams.Remove("tk");
            myParams.Remove("_");

            string computedToken = SecurityHelper.GenerateUrlToken(controllerName, actionName, myParams);
            if (!string.IsNullOrEmpty(computedToken))
            {
                computedToken = computedToken.TrimEnd(padding).Replace('+', '-').Replace('/', '_');
            }

            if (computedToken != token)
            {
                return false;
            }
            else { return true; }
        }
    }

    public class RequestParamsValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var paramDic = TokenUtility.QueryStringAsRouteValueDictionary(context);

            var isValidToken = TokenUtility.IsValidToken(paramDic, SystemSettings.JwtSecretKey, context);
            if (!isValidToken)
            {
                if (request.IsAjaxRequest())
                {
                    var isJsonResponse = true;
                    if (request.Headers != null)
                    {
                        if (!string.IsNullOrEmpty(request.Headers["Accept"]))
                        {
                            if (request.Headers["Accept"].Contains("html"))
                            {
                                isJsonResponse = false;
                            }
                        }
                    }

                    if (!isJsonResponse)
                    {
                        context.Result = new ContentResult
                        {
                            Content = @"<div style='color:#f4516c !important; padding:30px;'><i class='fa fa-warning'></i> " + ManagerResource.COMMON_ERROR_DATA_INVALID + "</div>",
                            StatusCode = 200,
                            ContentType = "text/html"
                        };
                    }
                    else
                    {
                        context.Result = new JsonResult(
                        new
                        {
                            success = false,
                            message = ManagerResource.COMMON_ERROR_DATA_INVALID
                        });
                    }
                }
                else
                {
                    context.Result = new RedirectToRouteResult(
                        "Default",
                        new RouteValueDictionary
                        {
                        { "controller", "Error" },
                        { "action", "RobotDetected" },
                        { "ReturnUrl", request.QueryString }
                        }
                    );
                }
            }
        }

        //public override void OnResultExecuting(ResultExecutingContext context)
        //{
        //    var request = context.HttpContext.Request;
        //    var paramDic = TokenUtility.QueryStringAsRouteValueDictionary(context);

        //    var isValidToken = TokenUtility.IsValidToken(paramDic, SystemSettings.JwtSecretKey, context);
        //    if (!isValidToken)
        //    {
        //        if (request.IsAjaxRequest())
        //        {
        //            var isJsonResponse = true;
        //            if (request.Headers != null)
        //            {
        //                if (!string.IsNullOrEmpty(request.Headers["Accept"]))
        //                {
        //                    if (request.Headers["Accept"].Contains("html"))
        //                    {
        //                        isJsonResponse = false;
        //                    }
        //                }
        //            }

        //            if (!isJsonResponse)
        //            {
        //                context.Result = new ContentResult
        //                {
        //                    Content = @"<div style='color:#f4516c !important; padding:30px;'><i class='fa fa-warning'></i> " + ManagerResource.COMMON_ERROR_DATA_INVALID + "</div>",
        //                    StatusCode = 200,
        //                    ContentType = "text/html"
        //                };
        //            }
        //            else
        //            {
        //                context.Result = new JsonResult(
        //                new
        //                {
        //                    success = false,
        //                    message = ManagerResource.COMMON_ERROR_DATA_INVALID
        //                });
        //            }
        //        }
        //        else
        //        {
        //            context.Result = new RedirectToRouteResult(
        //                "Default",
        //                new RouteValueDictionary
        //                {
        //                { "controller", "Error" },
        //                { "action", "RobotDetected" },
        //                { "ReturnUrl", request.QueryString }
        //                }
        //            );
        //        }
        //    }
        //}        
    }
}