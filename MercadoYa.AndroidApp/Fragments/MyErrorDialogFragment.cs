using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MercadoYa.AndroidApp.Fragments
{
    public class MyErrorDialogFragment : ErrorDialogFragment
    {
        public MyErrorDialogFragment(Dialog dialog)
        {
            Dialog = dialog;
        }

        public new Dialog Dialog { get; }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return Dialog;
        }
    }
}