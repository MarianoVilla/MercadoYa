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
        TextView txtAdditionalInfo;
        TextView txtIsOpened;
        TextView txtOpensAt;
        //@ToDo: implement these bad boys!
        Button btnMessageStore;
        Button btnShareStore;
        Button btnUberToStore;
        View RootView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        private void InitControls()
        {
            txtStoreName = RootView.FindViewById<TextView>(Resource.Id.txtStoreName);
            storeRatingBar = RootView.FindViewById<RatingBar>(Resource.Id.storeRatingBar);
            txtAdditionalInfo = RootView.FindViewById<TextView>(Resource.Id.txtAdditionalInfo);
            txtIsOpened = RootView.FindViewById<TextView>(Resource.Id.txtIsOpened);
            txtOpensAt = RootView.FindViewById<TextView>(Resource.Id.txtOpensAt);
            btnMessageStore = RootView.FindViewById<Button>(Resource.Id.btnMessageStore);
            btnShareStore = RootView.FindViewById<Button>(Resource.Id.btnShareStore);
            btnUberToStore = RootView.FindViewById<Button>(Resource.Id.btnUberToStore);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            RootView = inflater.Inflate(Resource.Layout.store_details_fragment, container, false);
            InitControls();
            RootView.Visibility = ViewStates.Visible;
            return RootView;
        }
        public void Update(StoreUser Store)
        {
            txtStoreName.Text = Store.DisplayableName;
            storeRatingBar.Rating = Store.RatingScore;
            txtAdditionalInfo.Text = Store.Description;
            var OpenIntervals = Store.Schedule.DailyIntervals.FirstOrDefault(x => x.Day == DateTime.Now.DayOfWeek)?.OpenIntervals;
            bool IsOpened = OpenIntervals is null ? false : OpenIntervals.ContainsValue(DateTime.Now);
            txtIsOpened.Text = IsOpened ? "Abierto" : "Cerrado";
            txtOpensAt.Text = OpenIntervals is null || IsOpened ? "" : OpenIntervals.Minimum.Hour.ToString();
        }
    }
}