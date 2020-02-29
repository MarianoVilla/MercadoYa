using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
//using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MercadoYa.Interfaces;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public class TaskCompletionListener : IOnSuccessListener, IOnFailureListener
    {
        public event EventHandler Success;
        public event EventHandler Failure;

        public void OnFailure(Exception e)
        {
            Failure?.Invoke(this, new EventArgs());
        }

        public void OnSuccess(object result)
        {
            Success?.Invoke(this, new EventArgs());
        }
    }
}