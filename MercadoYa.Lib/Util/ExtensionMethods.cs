using MercadoYa.Interfaces;
using MercadoYa.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MercadoYa.Lib.Util
{
    public static class ExtensionMethods
    {
        public static string RemoveNonNumeric(this string Value)
        {
            return Regex.Replace(Value, @"[^0-9]+", "");
        }
        public static bool HasNumber(this string Value)
        {
            return new Regex(@"[0-9]+").IsMatch(Value);
        }
        public static bool HasUpperChar(this string Value)
        {
            return new Regex(@"[A-Z]+").IsMatch(Value);
        }
        public static bool HasAtLeastNChars(this string Value, int N)
        {
            return Value.Length >= N;
        }
        public static void LoadTags(this IEnumerable<IAppUser> Users, IDatabase Database)
        {
            foreach (var u in Users)
            {
                u.Tags = Database.GetTagsForUser(u);
            }
        }
        public static  void LoadFoods(this IEnumerable<StoreUser> Stores, IDatabase Database)
        {
            foreach (var s in Stores)
            {
                s.Foods = (IEnumerable<Food>)Database.GetFoodTagsForUser(s);
            }
        }
    }

}
