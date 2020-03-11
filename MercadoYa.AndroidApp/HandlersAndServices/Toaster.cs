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

namespace MercadoYa.AndroidApp.HandlersAndServices
{
    public class Toaster
    {
        public static void SomethingWentWrong(Activity Context, ToastLength Length = ToastLength.Short)
        {
            Toast.MakeText(Context, "Algo salió mal", Length).Show();
        }
    }
}