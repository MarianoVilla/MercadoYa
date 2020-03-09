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

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Model.Concrete.StoreUser> FilteredByFood(this IEnumerable<Model.Concrete.StoreUser> Stores, string FilterText)
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                return Stores;
            return Stores.Where(x => x.Foods.Any(y => y.TagName.Trim().ToLower() == FilterText.Trim().ToLower()));
        }
        //public static void DrawInMap(this IAppUser User, GoogleMap Map, BitmapDescriptor Icon)
        //{
        //    var Options = new MarkerOptions();
        //    Options.SetPosition(new LatLng(User.Latitude, User.Longitude));
        //    Options.SetTitle(User.DisplayableName);
        //    Options.SetIcon(Icon);
        //    Map.AddMarker(Options);
        //}
        //public static void DrawInMap(this IEnumerable<IAppUser> Users, GoogleMap Map, BitmapDescriptor Icon)
        //{
        //    foreach(var User in Users)
        //    {
        //        User.DrawInMap(Map, Icon);
        //    }
        //}
        //public static void DrawInMap(this IEnumerable<IAppUser> Users, GoogleMap Map, float Hue = BitmapDescriptorFactory.HueBlue)
        //{
        //    foreach (var User in Users)
        //    {
        //        User.DrawInMap(Map, BitmapDescriptorFactory.DefaultMarker(Hue));
        //    }
        //}
        //public static void DrawMarker(this GoogleMap Map, LatLng Position, BitmapDescriptor Icon = null)
        //{
        //    var Options = new MarkerOptions();
        //    Options.SetPosition(Position);
        //    Options.SetIcon(Icon ?? BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen));
        //    Map.AddMarker(Options);
        //}

    }
}