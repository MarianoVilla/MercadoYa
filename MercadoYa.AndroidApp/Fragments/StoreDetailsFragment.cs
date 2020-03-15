using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MercadoYa.Model.Concrete;

namespace MercadoYa.AndroidApp.Fragments
{
    public class StoreDetailsFragment : Android.Support.V4.App.Fragment
    {
        TextView txtStoreName;
        RatingBar storeRatingBar;
        TextView txtAddicionalInfo;
        TextView txtIsOpened;
        TextView txtOpensAt;
        Button btnMessageStore;
        Button btnShareStore;
        Button btnUberToStore;
        View RootView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitControls();
        }

        private void InitControls()
        {
            txtStoreName = RootView.FindViewById<TextView>(Resource.Id.txtStoreName);
            storeRatingBar = RootView.FindViewById<RatingBar>(Resource.Id.storeRatingBar);
            txtAddicionalInfo = RootView.FindViewById<TextView>(Resource.Id.txtAdditionalInfo);
            txtIsOpened = RootView.FindViewById<TextView>(Resource.Id.txtIsOpened);
            txtOpensAt = RootView.FindViewById<TextView>(Resource.Id.txtOpensAt);
            btnMessageStore = RootView.FindViewById<Button>(Resource.Id.btnMessageStore);
            btnShareStore = RootView.FindViewById<Button>(Resource.Id.btnShareStore);
            btnUberToStore = RootView.FindViewById<Button>(Resource.Id.btnUberToStore);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            RootView = inflater.Inflate(Resource.Layout.store_details_fragment, container, false);
            InitControls();
            RootView.Visibility = ViewStates.Visible;
            return RootView;
        }
        public void Update(StoreUser Store)
        {
            txtStoreName.Text = Store.DisplayableName;
        }

        public static StoreDetailsFragment NewInstance(StoreUser store)
        {
            var bundle = new Bundle();
            return new StoreDetailsFragment { Arguments = bundle };
        }
    }
}