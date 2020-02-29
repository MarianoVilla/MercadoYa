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
using MercadoYa.Interfaces;
using MercadoYa.Lib.Util;
using MercadoYa.Model.Concrete;
using NetDebug = System.Diagnostics.Debug;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public class UserUtil
    {
        static readonly IValidator Validator = new Validator();
        public static ISharedPreferences GetUserInfo(FileCreationMode CreationMode = FileCreationMode.Private)
        {
            return Application.Context.GetSharedPreferences("userinfo", CreationMode);
        }
        public static ISharedPreferencesEditor GetUserInfoEditor()
        {
            ISharedPreferences Editor = GetUserInfo();
            return Editor.Edit();
        }
        public static string GetKey()
        {
            return GetUserInfo().GetString("Key", "");
        }
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

            var Email = Editor.GetString(nameof(FullAppUser.Email), null);
            var Password = Editor.GetString(nameof(FullAppUser.Password), null);
            var Phone = Editor.GetString(nameof(FullAppUser.Phone), null);
            var Name = Editor.GetString(nameof(FullAppUser.Username), null);

            List<string> DecryptedUser = CryptoUtil.TryDecryptStrings(Key: Key, DefaultTo: "", Email, Password, Phone, Name);

            return new FullAppUser()
            {
                Email = DecryptedUser[0],
                Password = DecryptedUser[1],
                Phone = DecryptedUser[2],
                Username = DecryptedUser[3]
            };
        }
        public static bool IsValidUser(string Email, string Password, string Phone = null, string Name = null)
        {
            return Validator.IsValidEmail(Email) && Validator.IsValidPhone(Phone)
                && Validator.IsValidPassword(Password) && Validator.IsValidName(Name);
        }
        public static string ValidateUser(string Email, string Password, string Phone = null, string Name = null)
        {
            string ValidationMessage = string.Empty;
            ValidationMessage += Validator.ValidateEmail(Email);
            ValidationMessage += Validator.ValidatePassword(Password);
            ValidationMessage += Phone == null ? string.Empty : Validator.ValidatePhone(Phone);
            ValidationMessage += Name == null ? string.Empty : Validator.ValidateName(Name);
            return ValidationMessage;
        }
        public static bool IsValidUser(IFullAppUser User)
        {
            return IsValidUser(User.Email, User.Password, User.Phone, User.Username);
        }
        public static Java.Util.HashMap HashUser(string Email, string Phone, string Name)
        {
            var UserMap = new Java.Util.HashMap();
            UserMap.Put(nameof(Email), Email);
            UserMap.Put(nameof(Phone), Phone);
            UserMap.Put(nameof(Name), Name);
            return UserMap;
        }
    }
}