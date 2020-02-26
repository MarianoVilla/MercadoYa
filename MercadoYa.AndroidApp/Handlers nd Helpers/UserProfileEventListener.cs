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

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public class UserProfileEventListener : Java.Lang.Object, IValueEventListener
    {
        public void OnCancelled(DatabaseError error)
        {
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Value == null)
                return;
            string  Name, Email, Phone;
            Name = snapshot.Child(nameof(Name)).Value.ToString() ?? "";
            Email = snapshot.Child(nameof(Email)).Value.ToString() ?? "";
            Phone = snapshot.Child(nameof(Phone)).Value.ToString() ?? "";
            UserUtil.SaveIfValid(Email: Email, Phone: Phone, Name: Name);
        }
        public void Create()
        {
            var Database = FirebaseHandler.GetDatabase();
            string UserId = FirebaseHandler.GetCurrentUser().Uid;
            //TODO: make "users/" a constant.
            DatabaseReference ProfileReference = Database.GetReference($"users/{UserId}");
            ProfileReference.AddValueEventListener(this);
        }
    }
}