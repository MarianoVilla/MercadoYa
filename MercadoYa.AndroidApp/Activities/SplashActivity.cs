using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.Lib.Util;
using MercadoYa.Model.Concrete;

namespace MercadoYa.AndroidApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MercadoYa.Splash", MainLauncher = true, NoHistory = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        MobileAppUser User;
        string Key;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        protected override void OnResume()
        {
            base.OnResume();
            CheckKey();
            if (HasPreviousLogin())
            {
                var TaskCompletionListener = new TaskCompletionListener();
                TaskCompletionListener.Success += TaskCompletionListener_Success;
                TaskCompletionListener.Failure += TaskCompletionListener_Failure;
                FirebaseHandler.GetFirebaseAuth().SignInWithEmailAndPassword(User.Email, User.Password)
                    .AddOnSuccessListener(this, TaskCompletionListener)
                    .AddOnFailureListener(this, TaskCompletionListener);
            }
            else
            {
                StartActivity(typeof(LoginActivity));
            }
        }
        void CheckKey()
        {
            if (HasKey())
                return;
            Key = CryptoUtil.RandomString(32);
            UserUtil.SaveKey(Key);
        }
        bool HasKey()
        {
            Key = UserUtil.GetKey();
            return Key != "";
        }
        bool HasPreviousLogin()
        {
            User = UserUtil.GetUserFromPreferences(Key);
            return StringUtil.NoneNullOrWhiteSpace(User.Email, User.Password);
        }
        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }

        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}