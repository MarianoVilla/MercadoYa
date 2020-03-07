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
using Firebase;
using Firebase.Auth;
using Firebase.Database;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    /// <summary>
    /// Contains general Firebase boilerplate code.
    /// </summary>
    public class FirebaseHandler
    {
        public static FirebaseApp InitializeApp()
        {
            var App = FirebaseApp.InitializeApp(Application.Context);
            if (App == null)
            {
                var Options = new FirebaseOptions.Builder()
                    .SetApplicationId("mercadoya-22303")
                    .SetApiKey("AIzaSyBIDWdnKX9etAxnBehwhh3pdq7S1LqFioQ")
                    .SetDatabaseUrl("https://mercadoya-22303.firebaseio.com")
                    .SetStorageBucket("mercadoya-22303.appspot.com")
                    .Build();
                App = FirebaseApp.InitializeApp(Application.Context, Options);
            }
            return App;
        }
        public static FirebaseDatabase GetDatabase()
        {
            var App = InitializeApp();
            return FirebaseDatabase.GetInstance(App);
        }
        public static FirebaseAuth GetFirebaseAuth()
        {
            var App = InitializeApp();
            return FirebaseAuth.GetInstance(App);
        }
        public static FirebaseUser GetCurrentUser()
        {
            var Auth = GetFirebaseAuth();
            return Auth.CurrentUser;
        }
    }
}