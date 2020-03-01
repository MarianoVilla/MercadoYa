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

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public class AuthCompletionListener : IOnSuccessListener, IOnFailureListener
    {
        public event EventHandler<IAuthResult> Success;
        public event EventHandler<Exception> Failure;

        public AuthCompletionListener(EventHandler<IAuthResult> SuccessMethod, EventHandler<Exception> FailureMethod)
        {
            this.Success = SuccessMethod;
            this.Failure = FailureMethod;
        }

        public void OnFailure(Exception e)
        {
            Failure?.Invoke(this, e);
        }

        public void OnSuccess(object result)
        {
            Success?.Invoke(this, (IAuthResult)result);
        }
    }
}