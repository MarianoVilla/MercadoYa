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
//using Firebase.Auth;
using Firebase.Database;
using Java.Util;
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.Interfaces;
using MercadoYa.Model.Concrete;

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
        IObservableClientAuthenticator Authenticator;
        CoordinatorLayout RootView;
        TextView txtClickToLogin;
        IFullAppUser Customer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.register);
            InitControls();
            ResolveDependencies();
        }

        void ResolveDependencies()
        {
            Authenticator = App.DiContainer.Resolve<IObservableClientAuthenticator>();
            Customer = new FullCustomerUser();
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

        void BtnRegister_Click(object sender, EventArgs e)
        {
            Customer.Username = txtName.EditText.Text;
            Customer.Phone = txtPhone.EditText.Text;
            Customer.Email = txtEmail.EditText.Text;
            Customer.Password = txtPassword.EditText.Text;

            if (UserUtil.PromptIfInvalid(RootView, Customer))
                return;

            RegisterUser();
        }
        void RegisterUser()
        {
            var TaskCompletionListener = new AuthCompletionListener(TaskCompletionListener_Success, TaskCompletionListener_Failure);
            Authenticator.CreateCustomerAsync(Customer);
            Authenticator.AddOnFailureListener(TaskCompletionListener);
            Authenticator.AddOnSuccessListener(TaskCompletionListener);
        }

        void TaskCompletionListener_Failure(object sender, Exception e)
        {
            Snackbar.Make(RootView, "Ocurrió un error al registrarse", Snackbar.LengthShort).Show();
        }

        void TaskCompletionListener_Success(object sender, IAuthResult Result)
        {
            Snackbar.Make(RootView, "¡Registro exitoso!", Snackbar.LengthShort).Show();
            var User = Result.User as FullCustomerUser;
            UserUtil.SaveIfValid(User.Email, User.Password);
            StartActivity(typeof(MainActivity));
            Finish();
        }

    }
}