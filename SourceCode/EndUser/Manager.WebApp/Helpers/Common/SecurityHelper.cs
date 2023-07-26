using Manager.SharedLibs;
using Manager.WebApp.Models;
using Manager.WebApp.Settings;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Manager.WebApp.Helpers
{
    public static class SecurityHelper
    {
        private static readonly char[] padding = { '=' };

        public static HtmlString EncodedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            string queryString = string.Empty;
            string htmlAttributesString = string.Empty;
            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (i > 0)
                    {
                        queryString += "&";
                    }
                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            if (htmlAttributes != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            //What is Entity Framework??
            StringBuilder ancor = new StringBuilder();
            ancor.Append("<a ");
            if (htmlAttributesString != string.Empty)
            {
                ancor.Append(htmlAttributesString);
            }
            ancor.Append(" href='");
            if (controllerName != string.Empty)
            {
                ancor.Append("/" + controllerName);
            }

            if (actionName != "Index")
            {
                ancor.Append("/" + actionName);
            }
            if (queryString != string.Empty)
            {
                var token = GenerateUrlToken(controllerName, actionName, routeValues);
                ancor.Append(string.Format("?{0}&tk={1}", queryString, token));
            }

            ancor.Append("'");
            ancor.Append(">");
            ancor.Append(linkText);
            ancor.Append("</a>");
            return new HtmlString(ancor.ToString());
        }

        public static string GenerateSecureLink(string controllerName, string actionName, object argumentParams)
        {
            StringBuilder builderStr = new StringBuilder();
            string queryString = string.Empty;
            try
            {
                string htmlAttributesString = string.Empty;
                if (argumentParams != null)
                {
                    RouteValueDictionary d = new RouteValueDictionary(argumentParams);
                    for (int i = 0; i < d.Keys.Count; i++)
                    {
                        if (i > 0)
                        {
                            queryString += "&";
                        }
                        queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                    }
                }

                //What is Entity Framework??
                if (controllerName != string.Empty)
                {
                    builderStr.Append("/" + controllerName);
                }

                if (actionName != "Index")
                {
                    builderStr.Append("/" + actionName);
                }
                if (queryString != string.Empty)
                {
                    var token = GenerateUrlToken(controllerName, actionName, argumentParams);
                    builderStr.Append(string.Format("?{0}&tk={1}", queryString, token));
                }
            }
            catch
            {

            }

            return builderStr.ToString();
        }

        public static string GenerateSecureLink(string controllerName, string actionName, List<RequestKeyValueModel> argumentParams)
        {
            StringBuilder builderStr = new StringBuilder();
            string queryString = string.Empty;
            try
            {
                string htmlAttributesString = string.Empty;
                if (argumentParams.HasData())
                {
                    var idx = 0;
                    foreach (var item in argumentParams)
                    {
                        if (idx > 0)
                        {
                            queryString += "&";
                        }
                        queryString += item.Key + "=" + item.Value;

                        idx++;
                    }
                }

                //What is Entity Framework??
                if (controllerName != string.Empty)
                {
                    builderStr.Append("/" + controllerName);
                }

                if (actionName != "Index")
                {
                    builderStr.Append("/" + actionName);
                }
                if (queryString != string.Empty)
                {
                    var token = GenerateUrlToken(controllerName, actionName, argumentParams);
                    builderStr.Append(string.Format("?{0}&tk={1}", queryString, token));
                }
            }
            catch
            {

            }

            return builderStr.ToString();
        }

        public static string GenerateSecureLinkDynamic(string controllerName, string actionName, dynamic argumentParams)
        {
            StringBuilder builderStr = new StringBuilder();
            string queryString = string.Empty;
            try
            {
                string htmlAttributesString = string.Empty;
                if (argumentParams != null)
                {
                    RouteValueDictionary d = new RouteValueDictionary(argumentParams);
                    for (int i = 0; i < d.Keys.Count; i++)
                    {
                        if (i > 0)
                        {
                            queryString += "&";
                        }
                        queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                    }
                }

                //What is Entity Framework??
                if (controllerName != string.Empty)
                {
                    builderStr.Append("/" + controllerName);
                }

                if (actionName != "Index")
                {
                    builderStr.Append("/" + actionName);
                }
                if (queryString != string.Empty)
                {
                    var token = GenerateUrlToken(controllerName, actionName, argumentParams);
                    builderStr.Append(string.Format("?{0}&tk={1}", queryString, token));
                }
            }
            catch
            {

            }

            return builderStr.ToString();
        }

        public static string GenerateUrlToken(string controllerName, string actionName, object argumentParams)
        {
            string token = "";
            //The salt can be defined global
            string salt = "#testsalt";
            //generating the partial url
            if (!string.IsNullOrEmpty(controllerName))
                controllerName = controllerName.ToLower();

            if (!string.IsNullOrEmpty(actionName))
                actionName = actionName.ToLower();

            string stringToToken = controllerName + "/" + actionName + "/";
            if (argumentParams.GetType() == typeof(RouteValueDictionary))
            {
                var myParams = (RouteValueDictionary)argumentParams;
                foreach (var item in myParams)
                {
                    if (item.Value.GetType() == typeof(string))
                    {
                        if (string.IsNullOrEmpty(item.Value.ToString()))
                        {
                            continue;
                        }
                    }

                    if (item.Value != null)
                        stringToToken += "/" + item.Value;
                }
            }
            else
            {
                var paramsDictionary = ObjectToDictionaryHelper.ToDictionary(argumentParams);
                foreach (var item in paramsDictionary)
                {
                    if (item.Value.GetType() == typeof(string))
                    {
                        if (string.IsNullOrEmpty(item.Value.ToString()))
                        {
                            continue;
                        }
                    }

                    if (item.Value != null)
                        stringToToken += "/" + item.Value;
                }
            }

            token = Utility.EncryptText(stringToToken, SystemSettings.JwtSecretKey, salt, "SHA1");

            if (!string.IsNullOrEmpty(token))
            {
                token = token.TrimEnd(padding).Replace('+', '-').Replace('/', '_');
            }

            //token = HttpUtility.UrlEncode(token);
            return token;
        }

        public static T GetFromQueryString<T>() where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var valueAsString = HttpContext.Current.Request.Query[property.Name];
                var value = ParseObject(valueAsString, property.PropertyType);

                if (value == null)
                    continue;

                property.SetValue(obj, value, null);
            }
            return obj;
        }

        public static object ParseObject(string valueToConvert, Type dataType)
        {
            TypeConverter obj = TypeDescriptor.GetConverter(dataType);
            object value = obj.ConvertFromString(null, CultureInfo.InvariantCulture, valueToConvert);
            return value;
        }
    }

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
}
