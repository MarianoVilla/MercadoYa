﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MercadoYa.Interfaces;
using MercadoYa.Model;
using MercadoYa.Model.Concrete;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public class RestAuth : IObservableClientAuthenticator
    {
        HttpClient Client;
        public List<IOnSuccessListener> OnSuccessListeners { get; } = new List<IOnSuccessListener>();
        public List<IOnFailureListener> OnFailureListeners { get; } = new List<IOnFailureListener>();
        public IAuthResult AuthResult { get; private set; }

        public RestAuth()
        {
            Client = new HttpClient() { BaseAddress = Const.RestUri  };
            Client.DefaultRequestHeaders.Add("accept", "*/*");
        }

        #region Observer boilerplate.
        public void AddOnSuccessListener(IOnSuccessListener Listener)
        {
            this.OnSuccessListeners.Add(Listener);
        }
        public void AddOnFailureListener(IOnFailureListener Listener)
        {
            this.OnFailureListeners.Add(Listener);
        }
        void NotifyError(Exception ex)
        {
            foreach(var l in OnFailureListeners)
            {
                l.OnFailure(ex);
            }
        }

        void NotifySuccess()
        {
            foreach(var l in OnSuccessListeners)
            {
                l.OnSuccess(this.AuthResult);
            }
        }
        #endregion

        public async Task SignInWithEmailAndPasswordAsync(string UserEmail, string UserPassword)
        {
            try
            {
                using HttpResponseMessage Res = await Client.PostAsJsonAsync("Auth/users/signin", new { Email = UserEmail, Password = UserPassword });
                if (Res.IsSuccessStatusCode)
                {
                    using HttpContent content = Res.Content;
                    string data = await content.ReadAsStringAsync();
                    this.AuthResult = new AuthResult(new FullAppUser() { Uid = data, Email = UserEmail, Password = UserPassword });
                    this.NotifySuccess();
                }
                else
                {
                    throw new InvalidCredentialException();
                }
            }
            catch (Exception ex)
            {
                this.NotifyError(ex);
            }

        }

    }
}