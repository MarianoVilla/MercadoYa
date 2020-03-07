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

namespace MercadoYa.AndroidApp.HandlersAndServices
{
    public class LocalDatabase
    {
        public static ISharedPreferences GetPreferences(string Name, FileCreationMode CreationMode = FileCreationMode.Private)
        {
            return Application.Context.GetSharedPreferences(Name, CreationMode);
        }
        public static ISharedPreferencesEditor GetPreferencesEditor(string Name, FileCreationMode CreationMode = FileCreationMode.Private)
        {
            ISharedPreferences Editor = GetPreferences(Name, CreationMode);
            return Editor.Edit();
        }
        static string FoodPrefName = "foodpref";
        static string FoodHistoryName = "foodhistory";
        public static ISharedPreferences GetFoodPreferences()
        {
            return GetPreferences(FoodPrefName);
        }
        public static ISharedPreferencesEditor GetFoodPreferencesEditor()
        {
            return GetPreferencesEditor(FoodPrefName);
        }
        public static ICollection<string> GetFoodSearchHistory()
        {
            return GetFoodPreferences().GetStringSet(FoodHistoryName, new List<string>());
        }
        public static void SaveFoodSearchHistory(ICollection<string> UpdatedCollection)
        {
            var Editor = GetFoodPreferencesEditor();
            Editor.PutStringSet(FoodHistoryName, UpdatedCollection);
            Editor.Commit();
        }
    }
}