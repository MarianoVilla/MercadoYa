using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using MercadoYa.Interfaces;
using Xamarin.Essentials;
using StoreUser = MercadoYa.Model.Concrete.StoreUser;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<StoreUser> FilteredByFood(this IEnumerable<StoreUser> Stores, string FilterText)
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                return Stores;
            return Stores.Where(x => x.Foods.Any(y => y.TagName.Trim().ToLower() == FilterText.Trim().ToLower()));
        }

    }
}