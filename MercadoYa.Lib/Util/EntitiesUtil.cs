using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Lib.Util
{
    public class EntitiesUtil
    {
        public static object ChangeType(object value, Type t)
        {
            if (value == null)
            {
                if (t.IsValueType)
                    throw new InvalidCastException();
                else return null;
            }
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }
                t = Nullable.GetUnderlyingType(t);
            }
            if (value.GetType() == typeof(string) && t == typeof(double))
            {
                value = ((string)value).Replace('.', ',');
            }
            if (t == typeof(DateTime))
            {
                DateTime Date;
                return DateTime.TryParse(value.ToString(), out Date) ? Date : default(DateTime);
            }
            if (t.IsEnum)
                return Enum.Parse(t, value.ToString());
            return Convert.ChangeType(value, t);
        }
    }
}
