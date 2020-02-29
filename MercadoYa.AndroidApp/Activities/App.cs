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
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.Interfaces;
using TinyIoC;

namespace MercadoYa.AndroidApp.Activities
{
    [Application]
    public class App : Application
    {
        protected App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        public override void OnCreate()
        {
            Initialize();
            base.OnCreate();
        }

        public static IDiContainer DIContainer { get; private set; }

        private static void Initialize()
        {
            DIContainer = new TinyIoCContainer();
            DIContainer.Register<IObservableClientAuthenticator, RestAuth>();
        }
    }
}