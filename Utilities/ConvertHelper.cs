using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Utilities
{
    public static class ConvertHelper
    {
        #region 基础类型转换
        public static int ToInt(this string that, int defult = 0)
        {
            that = that.Trim();
            if (!Regex.IsMatch(that, @"^-?\d+$"))
            {
                Log($"\"{that}\" is not int");
                return defult;
            }
            try
            {
                return Convert.ToInt32(that);
            }
            catch (Exception e)
            {
                Log($"\"{that}\" to int happen exception", e);
                return defult;
            }
        }

        public static int ToInt(this object that, int defult = 0)
        {
            try
            {
                return Convert.ToInt32(that);
            }
            catch (Exception e)
            {
                Log($"\"{that}\" to int happen exception", e);
                return defult;
            }
        }

        public static bool ToBool(this string that, bool defult = false)
        {
            if (that.ToLower() != "false" && that.ToLower() != "true")
            {
                if (that == "0")
                {
                    return false;
                }
                if (that == "1")
                {
                    return true;
                }
                Log($"\"{that}\" is not bool");
                return defult;
            }
            try
            {
                return Convert.ToBoolean(that);
            }
            catch (Exception e)
            {
                Log($"\"{that}\" to bool happen exception", e);
                return defult;
            }
        }

        public static bool ToBool(this object that, bool defult = false)
        {
            if (that.ToString() == "0")
            {
                return false;
            }
            if (that.ToString() == "1")
            {
                return true;
            }
            try
            {
                return Convert.ToBoolean(that);
            }
            catch (Exception e)
            {
                Log($"\"{that}\" to bool happen exception", e);
                return defult;
            }
        }

        public static double ToDouble(this string that, double defult = 0)
        {
            if (!Regex.IsMatch(that, @"^(-?\d+)(\.\d+)?$"))
            {
                Log($"\"{that}\" is not double");
                return defult;
            }
            try
            {
                return Convert.ToDouble(that);
            }
            catch (Exception e)
            {
                Log($"\"{that}\" to double happen exception", e);
                return defult;
            }
        }

        public static double ToDouble(this object that, double defult = 0)
        {
            try
            {
                return Convert.ToDouble(that);
            }
            catch (Exception e)
            {
                Log($"\"{that}\" to double happen exception", e);
                return defult;
            }
        }

        public static decimal ToDecimal(this string that, decimal defult = 0)
        {
            if (!Regex.IsMatch("", ""))
            {
                Log($"\"{that}\" is not decimal");
                return defult;
            }
            try
            {
                return Convert.ToDecimal(that);
            }
            catch (Exception e)
            {
                Log($"\"{that}\" to decimal happen exception", e);
                return defult;
            }
        }

        public static decimal ToDecimal(this object o, decimal defult = 0)
        {
            try
            {
                return Convert.ToDecimal(o);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return defult;
            }
        }

        public static DateTime ToDate(this string that)
        {
            if (!Regex.IsMatch(that,
                @"^(?:(?!0000)[0-9]{4}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])-(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)$")
            )
            {
                Log($"\"{that}\" is not datetime");
                return Convert.ToDateTime("1900-1-1");
            }

            try
            {
                return Convert.ToDateTime(that);
            }
            catch (Exception e)
            {
                Log($"\"{that}\" to datetime happen exception", e);
                return Convert.ToDateTime("1900-1-1");
            }
        }

        public static DateTime ToDate(this object that)
        {
            try
            {
                return Convert.ToDateTime(that);
            }
            catch (Exception e)
            {
                Log($"\"{that}\" to datetime happen exception", e);
                return Convert.ToDateTime("1900-1-1");
            }
        }

        public static string ToDescription(this Enum that)
        {
            var str = that.ToString();
            var field = that.GetType().GetField(str);
            var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs.Length == 0) return str;
            var da = (DescriptionAttribute)objs[0];
            return da.Description;
        }

        #endregion

        #region JSON 类型转换
        public static string ToJson(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(obj);
        }

        public static T JsonToObject<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Log($"/{json}/ to {nameof(T) } happen exception", e);
                return Activator.CreateInstance<T>();
            }
        }
        #endregion

        #region XML 类型转换
        public static T XmlToObject<T>(this string xml)
        {
            try
            {
                using (var rdr = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(rdr);
                }
            }
            catch (Exception e)
            {
                Log($"/{xml}/ to {nameof(T) } happen exception", e);
                return Activator.CreateInstance<T>();
            }
        }

        public static string ToXml<T>(this object obj)
        {
            var xsSubmit = new XmlSerializer(typeof(T));
            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, obj);
                    return XElement.Parse(sww.ToString()).ToString();
                }
            }
        }

        #endregion

        #region Table 转换
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            if (list == null || list.Count < 1)
            {
                return new DataTable();
            }
            var entityType = list[0].GetType();
            var entityProperties = entityType.GetProperties();

            var dt = new DataTable("dt");
            foreach (var t in entityProperties)
            {
                dt.Columns.Add(t.Name);
            }

            foreach (object entity in list)
            {
                var entityValues = new object[entityProperties.Length];
                for (var i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        public static IList<T> ToList<T>(this DataTable table) where T : new()
        {
            var properties = typeof(T).GetProperties().ToList();
            return (from object row in table.Rows select CreateItemFromRow<T>((DataRow)row, properties)).ToList();
        }
        #endregion

        public static string ToMd5(this string that)
        {
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(that));
                var sBuilder = new StringBuilder();
                foreach (var b in data)
                {
                    sBuilder.Append(b.ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        #region 私方法
        private static T CreateItemFromRow<T>(DataRow row, IEnumerable<PropertyInfo> properties) where T : new()
        {
            var item = new T();
            foreach (var property in properties)
            {
                try
                {
                    property.SetValue(item, row[property.Name] ?? "", null);
                }
                catch (Exception e)
                {
                    Log("dateTable to list happen exception", e);
                }
            }
            return item;
        }

        private static void Log(string msg, Exception ex = null)
        {
            if (ex == null)
            {
                Console.WriteLine(msg);
            }
            else
            {
                Console.WriteLine(ex);
                Console.WriteLine(msg);
            }
        }
        #endregion
    }
}
