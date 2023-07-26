using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Manager.SharedLibs.Extensions;
using System.ComponentModel;

namespace Manager.SharedLibs
{
    public class ObjectDictionaryIgnore : Attribute
    {

    }

    public class MappingObjectAttribute : Attribute
    {
        public int Code;
        public string Name;
        public string Description;

        public MappingObjectAttribute(int code = 0, string name = "", string description = "")
        {
            this.Code = code;
            this.Name = name;
            this.Description = description;
        }
    }

    public class MappingFieldAttribute : Attribute
    {
        public string Code;
        public string Name;
        public string Description;
        public string Type;

        public MappingFieldAttribute(string code = "", string name = "", string type = "text", string description = "")
        {
            this.Code = code;
            this.Name = name;
            this.Description = description;
            this.Type = type;
        }
    }

    public static class ObjectExt
    {
        public static MappingObjectAttribute GetMappingObject(this System.Type typeInfo)
        {
            MappingObjectAttribute[] attributes =
                (MappingObjectAttribute[])typeInfo.GetCustomAttributes(
                typeof(MappingObjectAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0];
            else
                return null;
        }

        public static MappingFieldAttribute GetMappingField(this System.Reflection.PropertyInfo prop)
        {
            MappingFieldAttribute[] attributes =
                (MappingFieldAttribute[])prop.GetCustomAttributes(
                typeof(MappingFieldAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0];
            else
                return null;
        }

        public static string GetObjectPropertyDescription(this System.Reflection.PropertyInfo prop)
        {
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])prop.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return prop.ToString();
        }

        public static bool IsMappingField(this System.Reflection.PropertyInfo prop)
        {
            if (Attribute.IsDefined(prop, typeof(MappingFieldAttribute)))
                return true;

            return false;
        }

        public static T MappingObject<T>(this object obj)
        {
            try
            {
                if (obj != null)
                {
                    var str = JsonConvert.SerializeObject(obj);
                    var targetObj = JsonConvert.DeserializeObject<T>(str);

                    return targetObj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return default(T);
        }

        public static T MappingListObjectFromDataTable<T>(this DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    var str = JsonConvert.SerializeObject(dt);
                    var obj = JsonConvert.DeserializeObject<T>(str);

                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return default(T);
        }

        public static T MappingSingleObjectFromDataTable<T>(this DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    var str = JsonConvert.SerializeObject(dt);
                    var listData = JsonConvert.DeserializeObject<List<T>>(str);
                    if (listData.HasData())
                    {
                        return listData[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return default(T);
        }

        public static T MappingListObjectFromDataReader<T>(this IDataReader reader)
        {
            try
            {
                if (!reader.IsClosed)
                {
                    DataTable dt = new DataTable();

                    // DataTable.Load automatically advances the reader to the next result set
                    dt.Load(reader);
                    dt.EndLoadData();

                    if (dt != null)
                    {
                        var str = JsonConvert.SerializeObject(dt);
                        var obj = JsonConvert.DeserializeObject<T>(str);

                        return obj;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return default(T);
        }

        public static T MappingSingleObjectFromDataReader<T>(this IDataReader reader)
        {
            try
            {
                if (!reader.IsClosed)
                {
                    DataTable dt = new DataTable();
                    // DataTable.Load automatically advances the reader to the next result set
                    dt.Load(reader);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var str = JsonConvert.SerializeObject(dt);
                        var listData = JsonConvert.DeserializeObject<List<T>>(str);
                        if (listData.HasData())
                        {
                            return listData[0];
                        }
                    }
                }

                //var element = Activator.CreateInstance<T>();
                //var properties = typeof(T).GetProperties();

                //foreach (var f in properties)
                //{
                //    var o = reader[f.Name];
                //    if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);                    
                //}

                //return element;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return default(T);
        }

        public static object MappingObjectFromDataReader(object obj, IDataReader reader)
        {
            try
            {
                foreach (PropertyInfo pInfo in obj.GetType().GetProperties())
                {
                    if (!reader.HasColumn(pInfo.Name))
                        continue;

                    if (reader[pInfo.Name] == null)
                        continue;

                    // do stuff here
                    if (pInfo.PropertyType.Name == "string" || pInfo.PropertyType.Name == "String")
                    {
                        pInfo.SetValue(obj, reader[pInfo.Name].ToString());
                    }
                    else if (pInfo.PropertyType.Name == "Int" || pInfo.PropertyType.Name == "int"
                     || pInfo.PropertyType.Name == "Int32" || pInfo.PropertyType.Name == "int32"
                    )
                    {
                        pInfo.SetValue(obj, Utils.ConvertToInt32(reader[pInfo.Name]));
                    }
                    else if (pInfo.PropertyType.Name == "Bool" || pInfo.PropertyType.Name == "bool")
                    {
                        pInfo.SetValue(obj, Utils.ConvertToBoolean(reader[pInfo.Name]));
                    }
                    else if (pInfo.PropertyType.FullName == "DateTime" || pInfo.PropertyType.Name == "DateTime?")
                    {
                        if (pInfo.PropertyType.Name == "DateTime?")
                        {
                            if (reader[pInfo.Name] != DBNull.Value)
                            {
                                pInfo.SetValue(obj, (DateTime?)reader[pInfo.Name]);
                            }
                        }
                        else
                        {
                            pInfo.SetValue(obj, (DateTime)reader[pInfo.Name]);
                        }
                    }
                    else if (pInfo.PropertyType.Name == "Decimal" || pInfo.PropertyType.Name == "decimal")
                    {
                        pInfo.SetValue(obj, Utils.ConvertToDecimal(reader[pInfo.Name]));
                    }
                    else if (pInfo.PropertyType.Name == "Float" || pInfo.PropertyType.Name == "float")
                    {
                        pInfo.SetValue(obj, Utils.ConvertToDecimal(reader[pInfo.Name]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }

        public static Dictionary<string, object> MappingObjectToDictionary1(object obj)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                foreach (PropertyInfo pInfo in obj.GetType().GetProperties())
                {
                    if (Attribute.IsDefined(pInfo, typeof(ObjectDictionaryIgnore)))
                        continue;

                    dic.Add(pInfo.Name, pInfo.GetValue(obj, null));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dic;
        }

        public static string ToStringNormally(this string myString)
        {
            var output = string.Empty;
            if (!string.IsNullOrEmpty(myString))
            {
                output = myString.Normalize(NormalizationForm.FormKC);
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);

                output = regex.Replace(output, " ");

                if (!string.IsNullOrEmpty(output))
                    output = output.Trim();
            }

            return output;
        }

        //public static T DeepCopy<T>(this T objectToCopy)
        //{
        //    MemoryStream memoryStream = new MemoryStream();
        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    binaryFormatter.Serialize(memoryStream, objectToCopy);

        //    memoryStream.Position = 0;
        //    T returnValue = (T)binaryFormatter.Deserialize(memoryStream);


        //    memoryStream.Close();
        //    memoryStream.Dispose();


        //    return returnValue;
        //}

        public static T DeepCopy<T>(this T self)
        {
            var serialized = JsonConvert.SerializeObject(self);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public static bool TryParseJson<T>(this string obj, out T result)
        {
            try
            {
                // Validate missing fields of object
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.MissingMemberHandling = MissingMemberHandling.Error;

                result = JsonConvert.DeserializeObject<T>(obj, settings);
                return true;
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }
    }

    public static class ObjectExtensions
    {
        public static string ToUTF8(this string rawStr)
        {
            var myStr = rawStr;
            try
            {
                byte[] bytes = Encoding.Default.GetBytes(rawStr);
                myStr = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return rawStr;
            }

            return myStr;
        }

        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

        public static bool HasData<T>(this List<T> myList)
        {
            if (myList.IsAny())
                return true;
            return false;
        }

        public static bool HasData<T>(this T[] myArr)
        {
            if (myArr != null && myArr.Length > 0)
                return true;
            return false;
        }

        public static string DateTimeNullableToString(this DateTime? dateTime, string format = "HH:mm dd/MM/yyyy", bool isUtc = false)
        {
            if (dateTime != null)
            {
                if (isUtc)
                    dateTime = dateTime.Value.ToLocalTime();

                return dateTime.Value.ToString(format);
            }

            return string.Empty;
        }

        public static string DateTimeNullableToLocalString(this DateTime? dateTime, string currentLang, string format = "dd/MM/yyyy")
        {
            if (dateTime != null)
            {

                if (currentLang == "ja-JP")
                {
                    var jaCulture = new CultureInfo(currentLang);
                    return dateTime.Value.ToLocalTime().ToString(jaCulture.DateTimeFormat.LongDatePattern, jaCulture);
                }
                else
                {
                    return dateTime.Value.ToLocalTime().ToString(format);
                }
            }


            return string.Empty;
        }

        public static string DateTimeNullableToStringNow(this DateTime? dateTime, string format = "HH:mm dd/MM/yyyy", bool isUtc = false)
        {
            if (dateTime != null)
            {
                if (isUtc)
                    dateTime = dateTime.Value.ToLocalTime();

                if (dateTime.Value.Date == DateTime.Now.Date)
                    return dateTime.Value.ToString("HH:mm");
                else
                    return dateTime.Value.ToString(format);
            }

            return string.Empty;
        }

        public static string DateTimeNullableToLocalStringNow(this DateTime? dateTime, string format = "HH:mm dd/MM/yyyy")
        {
            if (dateTime != null)
            {
                if (dateTime.Value.Date == DateTime.Now.Date)
                    return dateTime.Value.ToLocalTime().ToString("HH:mm");
                else
                    return dateTime.Value.ToLocalTime().ToString(format);
            }

            return string.Empty;
        }

        public static string TimeSpanQuestToString(this TimeSpan? timeSpan, string format = @"hh\:mm")
        {
            if (timeSpan != null)
                return timeSpan.Value.ToString(format);

            return string.Empty;
        }

        public static string FormatWithComma(this int value)
        {
            var returnStr = "0";
            if (value > 0)
            {
                returnStr = String.Format("{0:n0}", value);
            }

            return returnStr;
        }

        public static decimal Normalize(this decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }

        public static decimal? Normalize(this decimal? value)
        {
            if (value.HasValue)
                return value.Value / 1.000000000000000000000000000000000m;

            return 0;
        }

        public static string FormatWithComma(this decimal value, int partNum = 2)
        {
            var returnStr = "0";
            if (value > 0)
            {
                value = value.Normalize();

                returnStr = String.Format("{0:n" + partNum + "}", Utils.ConvertToDecimal(value.ToString("G29")));
                returnStr = returnStr.Contains(".") ? returnStr.TrimEnd('0').TrimEnd('.') : returnStr;
            }

            return returnStr;
        }


        public static string FormatWithComma(this decimal? num, int partNum = 2)
        {
            var returnStr = "0";
            if (num.HasValue)
            {
                num = num.Normalize();

                returnStr = String.Format("{0:n" + partNum + "}", Utils.ConvertToDecimal(num.Value.ToString("G29")));
                returnStr = returnStr.Contains(".") ? returnStr.TrimEnd('0').TrimEnd('.') : returnStr;
            }

            return returnStr;
        }

        public static IEnumerable<Dictionary<string, object>> Serialize(this IDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }

        private static Dictionary<string, object> SerializeRow(IEnumerable<string> cols, IDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }

        public static string GetTextBetween(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }

        public static IEnumerable<DateTime> GetDaysBetween(DateTime from, DateTime end)
        {
            for (var day = from.Date; day.Date <= end.Date; day = day.AddDays(1))
                yield return day;
        }

        public static IEnumerable<DateTime> GetHoursBetween(DateTime start, DateTime end)
        {
            DateTime first = start.Date.AddHours(start.Hour);
            for (DateTime dateTime = first; dateTime <= end; dateTime = dateTime.AddHours(1))
            {
                yield return dateTime;
            }
        }

        public static IEnumerable<Tuple<DateTime, DateTime>> SplitByHour(DateTime start, DateTime end)
        {
            DateTime chunkEnd;
            var startTime = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
            if (start < end)
            {
                while ((chunkEnd = startTime) < end)
                {
                    yield return Tuple.Create(startTime, chunkEnd.AddHours(1));
                    startTime = chunkEnd.AddHours(1);
                }
            }

            //yield return Tuple.Create(start, start.AddHours(1));
        }

        public static IEnumerable<Tuple<DateTime, DateTime>> SplitByWeek(DateTime start, DateTime end, int chunk)
        {
            DateTime chunkEnd;
            while ((chunkEnd = start.AddDays(chunk)) < end)
            {
                yield return Tuple.Create(start, chunkEnd);
                start = chunkEnd.AddDays(1);
            }
            yield return Tuple.Create(start, start.AddDays(chunk));
        }

        public static IEnumerable<Tuple<DateTime, DateTime>> GetDateRangesByYear(DateTime start, DateTime end)
        {
            List<Tuple<DateTime, DateTime>> ranges = new List<Tuple<DateTime, DateTime>>();

            for (int year = start.Year; year <= end.Year; ++year)
            {
                DateTime yearBegin = year == start.Year ? start : new DateTime(year, 1, 1);
                DateTime yearEnd = year == end.Year ? end : new DateTime(year, 12, 31);

                ranges.Add(Tuple.Create(yearBegin, yearEnd));
            }

            return ranges;
        }
    }

    public class MyObjectExtensions
    {
        public static bool PropertyExists(dynamic obj, string name)
        {
            if (obj == null) return false;
            if (obj is IDictionary<string, object> dict)
            {
                return dict.ContainsKey(name);
            }
            return obj.GetType().GetProperty(name) != null;
        }
    }
}