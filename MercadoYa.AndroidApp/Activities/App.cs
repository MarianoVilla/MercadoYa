using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.Interfaces;
using MercadoYa.Lib.Util;
using MercadoYa.Model;
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

        public static IDiContainer DiContainer { get; private set; }

        private static void Initialize()
        {
            InitDi();
            InitHttpClient();
        }
        static void InitDi()
        {
            DiContainer = new TinyIoCContainer();
            DiContainer.Register<IObservableClientAuthenticator, RestAuth>().AsMultiInstance();
            DiContainer.Register<IRestDatabase, RestDatabase>();
            DiContainer.Register<IValidator, Validator>();
        }
        static void InitHttpClient()
        {
            var Cookies = new CookieContainer();
            var Handler = new HttpClientHandler() { CookieContainer = Cookies };

            Const.GlobalHttpClient = new HttpClient(Handler) { BaseAddress = Const.RestUri };
            Const.GlobalHttpClient.DefaultRequestHeaders.Add("accept", "*/*");
        }
    }
}