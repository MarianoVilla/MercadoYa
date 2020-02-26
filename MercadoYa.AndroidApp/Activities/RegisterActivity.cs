﻿using System;
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
        FirebaseAuth Auth;
        FirebaseDatabase Database;
        CoordinatorLayout RootView;
        TaskCompletionListener TaskCompletionListener = new TaskCompletionListener();
        TextView txtClickToLogin;


        string Name, Phone, Email, Password;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.register);
            InitControls();
            InitFirebase();
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
            TaskCompletionListener.Success += TaskCompletionListener_Success;
            TaskCompletionListener.Failure += TaskCompletionListener_Failure;
            Auth.CreateUserWithEmailAndPassword(Email, Password)
                .AddOnSuccessListener(this, TaskCompletionListener)
                .AddOnFailureListener(this, TaskCompletionListener);
        }

        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(RootView, "Ocurrió un error al registrarse", Snackbar.LengthShort).Show();
        }

        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            Snackbar.Make(RootView, "¡Registro exitoso!", Snackbar.LengthShort).Show();

            DatabaseReference UserReference = Database.GetClientUser(Auth.CurrentUser.Uid);
            UserReference.SetValue(UserUtil.HashUser(Email, Phone, Name));
        }


        void InitFirebase()
        {
            Database = FirebaseHandler.GetDatabase();
            Auth = FirebaseHandler.GetFirebaseAuth();
        }

    }
}