using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Database;
using Java.Util;
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.Interfaces;

namespace MercadoYa.AndroidApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MercadoYa.Theme", MainLauncher = false)]
    public class RegisterActivity : AppCompatActivity
    {
        TextInputLayout txtName;
        TextInputLayout txtPhone;
        TextInputLayout txtEmail;
        TextInputLayout txtPassword;
        Button btnRegister;
        //FirebaseAuth Auth;
        IObservableClientAuthenticator Authenticator;
        FirebaseDatabase Database;
        CoordinatorLayout RootView;
        TextView txtClickToLogin;


        string Name, Phone, Email, Password;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.register);
            InitControls();
            InitFirebase();
            ResolveDependencies();
        }

        private void ResolveDependencies()
        {
            Authenticator = App.DiContainer.Resolve<IObservableClientAuthenticator>();
        }

        void InitControls()
        {
            RootView = (CoordinatorLayout)FindViewById(Resource.Id.rootView);
            txtName = (TextInputLayout)FindViewById(Resource.Id.txtFullName);
            txtPhone = (TextInputLayout)FindViewById(Resource.Id.txtPhoneNumber);
            txtEmail = (TextInputLayout)FindViewById(Resource.Id.txtEmail);
            txtPassword = (TextInputLayout)FindViewById(Resource.Id.txtPassword);
            btnRegister = (Button)FindViewById(Resource.Id.btnRegister);
            txtClickToLogin = (TextView)FindViewById(Resource.Id.txtGoToLogin);

            btnRegister.Click += BtnRegister_Click;
            txtClickToLogin.Click += TxtClickToLogin_Click;
        }

        private void TxtClickToLogin_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            Name = txtName.EditText.Text;
            Phone = txtPhone.EditText.Text;
            Email = txtEmail.EditText.Text;
            Password = txtPassword.EditText.Text;

            if (InvalidInput())
                return;

            RegisterUser();
        }

        bool InvalidInput()
        {
            return !UserUtil.IsValidUser(Email, Password, Phone, Name);
        }
        void RegisterUser()
        {
            var TaskCompletionListener = new TaskCompletionListener(TaskCompletionListener_Success, TaskCompletionListener_Failure);
            Authenticator.SignInWithEmailAndPasswordAsync(Email, Password);
            Authenticator.AddOnFailureListener(TaskCompletionListener);
            Authenticator.AddOnSuccessListener(TaskCompletionListener);
        }

        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(RootView, "Ocurrió un error al registrarse", Snackbar.LengthShort).Show();
        }

        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            Snackbar.Make(RootView, "¡Registro exitoso!", Snackbar.LengthShort).Show();

            DatabaseReference UserReference = Database.GetClientUser(Authenticator.CurrentUser.Uid);
            UserReference.SetValue(UserUtil.HashUser(Email, Phone, Name));
        }


        void InitFirebase()
        {
            Database = FirebaseHandler.GetDatabase();
        }

    }
}