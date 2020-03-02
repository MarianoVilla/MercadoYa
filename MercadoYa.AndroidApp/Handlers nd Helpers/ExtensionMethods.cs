using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Xamarin.Essentials;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public static class ExtensionMethods
    {
        public static DatabaseReference GetStoreUser(this FirebaseDatabase Database, string Uid)
        {
            return Database.GetReference($"users/storeusers/{Uid}");
        }
        //public static DatabaseReference GetNearStoreUsers(this FirebaseDatabase Database, Location Loc)
        //{
        //    var StoreUsers = Database.GetReference($"users/storeusers");
        //    var OrderedByLocation = StoreUsers.OrderByChild("location");
        //    var OrderedByLatitude = OrderedByLocation.OrderByChild("latitude");
        //    var StartAt = OrderedByLatitude.StartAt(Loc.Latitude - .1);
        //    var LimitTo = StartAt.LimitToFirst(1).Ref;
        //    return Database.GetReference($"users/storeusers").OrderByChild("location").OrderByChild("latitude").StartAt(Loc.Latitude - .1).LimitToFirst(1).Ref;
        //}
        public static DatabaseReference GetClientUser(this FirebaseDatabase Database, string Uid)
        {
            return Database.GetReference($"users/clientusers/{Uid}");
        }
        public static DatabaseReference GetFoods(this FirebaseDatabase Database, string Name = null)
        {
            return string.IsNullOrWhiteSpace(Name) ? Database.GetReference($"foods") : Database.GetReference($"foods/{Name.ToLower()}");
        }
        //public async static Task<T> ReadAsObjectAsync<T>(this HttpContent TheContent)
        //{
        //    return await TheContent.Rea
        //}

    }
}