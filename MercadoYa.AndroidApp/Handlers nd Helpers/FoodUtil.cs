using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using MercadoYa.Model.Concrete;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public static class FoodUtil
    {
        public static Food GetFood(string Name)
        {
            DatabaseReference FoodWithThatName = FirebaseHandler.GetDatabase().GetFoods(Name);
            var Limited = FoodWithThatName.LimitToFirst(1);
            return new Food();
        }
    }
}