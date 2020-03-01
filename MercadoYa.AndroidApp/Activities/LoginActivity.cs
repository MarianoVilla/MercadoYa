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
using Firebase.Auth;
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.Interfaces;

namespace MercadoYa.AndroidApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MercadoYa.Theme", MainLauncher = false)]
    public class LoginActivity : AppCompatActivity
    {

        TextView txtGoToRegister;
        TextInputLayout txtEmail;
        TextInputLayout txtPassword;
        Button btnLogin;
        CoordinatorLayout RootView;
        IObservableClientAuthenticator Authenticator;

        string Email, Password;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.login);
            InitControls();
            ResolveDependencies();
        }

        private void ResolveDependencies()
        {
            Authenticator = App.DiContainer.Resolve<IObservableClientAuthenticator>();
        }

        private void InitControls()
        {
            RootView = (CoordinatorLayout)FindViewById(Resource.Id.rootView);
            txtGoToRegister = (TextView)FindViewById(Resource.Id.txtGoToRegister);
            txtEmail = (TextInputLayout)FindViewById(Resource.Id.txtEmail);
            txtPassword = (TextInputLayout)FindViewById(Resource.Id.txtPassword);
            btnLogin = (Button)FindViewById(Resource.Id.btnLogin);

            btnLogin.Click += BtnLogin_Click;
            txtGoToRegister.Click += TxtGoToRegister_Click;

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {

            Email = txtEmail.EditText.Text;
            Password = txtPassword.EditText.Text;

            if (InvalidInput())
                return;

            var TaskCompletionListener = new TaskCompletionListener(TaskCompletionListener_Success, TaskCompletionListener_Failure);

            Authenticator.SignInWithEmailAndPasswordAsync(Email, Password);
            Authenticator.AddOnFailureListener(TaskCompletionListener);
            Authenticator.AddOnSuccessListener(TaskCompletionListener);
        }

        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(RootView, "Algo salió mal", Snackbar.LengthShort).Show();
        }

        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            UserUtil.SaveIfValid(Email, Password);
            StartActivity(typeof(MainActivity));
            Finish();
        }

        bool InvalidInput()
        {
            string ValidationError = UserUtil.ValidateUser(Email, Password);
            if (ValidationError != string.Empty)
            {
                Snackbar.Make(RootView, ValidationError, Snackbar.LengthShort).Show();
                return true;
            }
            return false;
        }

        private void TxtGoToRegister_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
            Finish();
        }
    }
}