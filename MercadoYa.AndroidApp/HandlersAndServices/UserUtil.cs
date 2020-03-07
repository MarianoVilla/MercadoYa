using Android.App;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Views;
using MercadoYa.AndroidApp.Activities;
using MercadoYa.AndroidApp.HandlersAndServices;
using MercadoYa.Interfaces;
using MercadoYa.Lib.Util;
using MercadoYa.Model.Concrete;
using System.Collections.Generic;
using NetDebug = System.Diagnostics.Debug;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public class UserUtil
    {
        static readonly IValidator Validator = App.DiContainer.Resolve<IValidator>();
        static string UserInfoName = "userinfo";

        public static ISharedPreferences GetUserInfo(FileCreationMode CreationMode = FileCreationMode.Private) 
            => LocalDatabase.GetPreferences(UserInfoName, CreationMode);
        public static ISharedPreferencesEditor GetUserInfoEditor(FileCreationMode CreationMode = FileCreationMode.Private) 
            => LocalDatabase.GetPreferencesEditor(UserInfoName, CreationMode);
        public static string GetKey() => GetUserInfo().GetString("Key", "");
        public static void SaveKey(string Key)
        {
            ISharedPreferencesEditor Editor = GetUserInfoEditor();
            Editor.PutString(nameof(Key), Key);
            Editor.Apply();
            NetDebug.Assert(GetKey() == Key);
        }
        public static void SaveIfValid(string Email, string Password = null, string Phone = null, string Name = null)
        {
            EncryptAndSaveIfValid(nameof(Email), Email);
            EncryptAndSaveIfValid(nameof(Password), Password);
            EncryptAndSaveIfValid(nameof(Phone), Phone);
            EncryptAndSaveIfValid(nameof(Name), Name);
        }
        static void EncryptAndSaveIfValid(string Name, string Value)
        {
            if (string.IsNullOrWhiteSpace(Value))
                return;
            string EncryptedValue = CryptoUtil.EncryptString(GetKey(), Value);
            ISharedPreferencesEditor Editor = GetUserInfoEditor();
            Editor.PutString(Name, EncryptedValue);
            Editor.Apply();
        }
        public static void ClearUserPreferences()
        {
            ISharedPreferencesEditor Editor = GetUserInfoEditor();
            Editor.Clear();
            Editor.Apply();
        }
        public static FullAppUser GetUserFromPreferences(string Key)
        {
            ISharedPreferences Editor = GetUserInfo();

            string Email = Editor.GetString(nameof(FullAppUser.Email), null);
            string Password = Editor.GetString(nameof(FullAppUser.Password), null);
            string Phone = Editor.GetString(nameof(FullAppUser.Phone), null);
            string Name = Editor.GetString(nameof(FullAppUser.Username), null);

            List<string> DecryptedUser = CryptoUtil.TryDecryptStrings(Key: Key, DefaultTo: "", Email, Password, Phone, Name);

            return new FullAppUser()
            {
                Email = DecryptedUser[0],
                Password = DecryptedUser[1],
                Phone = DecryptedUser[2],
                Username = DecryptedUser[3]
            };
        }
        public static bool IsValidUser(string Email, string Password, string Phone = null, string Username = null)
        {
            return Validator.IsValidEmail(Email) && Validator.IsValidPhone(Phone)
                && Validator.IsValidPassword(Password) && Validator.IsValidName(Username);
        }
        public static bool IsValidUser(IFullAppUser User) => IsValidUser(User.Email, User.Password, User.Phone, User.Username);
        public static string ValidateUser(string Email, string Password, string Phone = null, string Username = null)
        {
            string ValidationMessage = string.Empty;
            ValidationMessage += Validator.ValidateEmail(Email);
            ValidationMessage += Validator.ValidatePassword(Password);
            ValidationMessage += Phone == null ? string.Empty : Validator.ValidatePhone(Phone);
            ValidationMessage += Username == null ? string.Empty : Validator.ValidateName(Username);
            return ValidationMessage;
        }
        public static string ValidateUser(IFullAppUser User) => ValidateUser(User.Email, User.Password, User.Phone, User.Username);

        public static bool PromptIfInvalid(View RootView, string Email, string Password, string Phone = null, string Username = null)
        {
            string ValidationError = ValidateUser(Email, Password);
            if (ValidationError != string.Empty)
            {
                Snackbar.Make(RootView, ValidationError, Snackbar.LengthShort).Show();
                return true;
            }
            return false;
        }
        public static bool PromptIfInvalid(View RootView, IFullAppUser User)
            => PromptIfInvalid(RootView, User.Email, User.Password, User.Phone, User.Username);

        public static Java.Util.HashMap HashUser(string Email, string Phone, string Username)
        {
            var UserMap = new Java.Util.HashMap();
            UserMap.Put(nameof(Email), Email);
            UserMap.Put(nameof(Phone), Phone);
            UserMap.Put(nameof(Username), Username);
            return UserMap;
        }
    }
}