using System;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace Hubery.Lavcode.Uwp.Helpers
{
    public static class ConvertHelper
    {
        private static object ConvertValue(object value, Type type)
        {
            if (value == null || value is DBNull || (value is string emptyStr && emptyStr == string.Empty))
            {
                return default;
            }
            else if (value.GetType() == type)
            {
                return value;
            }
            else if (value is string str && type == typeof(bool))
            {
                if (string.Equals(str, "y", StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(str, "yes", StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(str, "true", StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(str, "1", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return Convert.ChangeType(value, type);
            }
        }

        private static T ConvertValue<T>(object value)
        {
            return (T)ConvertValue(value, typeof(T));
        }

        public static T GetValue<T>(this object obj)
        {
            return ConvertValue<T>(obj);
        }
        public static object GetValue(this object obj, Type type)
        {
            return ConvertValue(obj, type);
        }

        public static T GetValue<T>(this IDataReader dataReader, string fieldName)
        {
            return ConvertValue<T>(dataReader[fieldName]);
        }

        public static T GetValue<T>(this IDataReader dataReader, int index)
        {
            return ConvertValue<T>(dataReader[index]);
        }

        public static T GetValue<T>(this DataRow dataRow, string fieldName)
        {
            return ConvertValue<T>(dataRow[fieldName]);
        }

        public static T GetValue<T>(this DataRow dataRow, int index)
        {
            return ConvertValue<T>(dataRow[index]);
        }

        public static T GetValue<T>(this XAttribute xAttribute)
        {
            return ConvertValue<T>(xAttribute.Value);
        }


        public static string ToX2(this byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
