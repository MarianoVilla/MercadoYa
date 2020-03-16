using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MercadoYa.AndroidApp.Fragments;
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.AndroidApp.HandlersAndServices;
using MercadoYa.AndroidApp.Model;
using MercadoYa.Interfaces;
using MercadoYa.Lib.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static MercadoYa.AndroidApp.Handlers_nd_Helpers.LocationCallbacker;
using EssentialsLocation = Xamarin.Essentials.Location;
using MyLocationRequest = MercadoYa.Model.Concrete.LocationRequest;
using StoreUser = MercadoYa.Model.Concrete.StoreUser;

namespace MercadoYa.AndroidApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MercadoYa.Theme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback, NavigationView.IOnNavigationItemSelectedListener
    {
        GoogleMap MainMap;
        readonly string[] PermissionGroupLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        const int LocationRequestId = 0;
        AutoCompleteTextView txtSearch;
        MapHandler MapsHandler;
        IRestDatabase Database;
        SupportMapFragment MapFragment;
        FloatingActionButton fabCenter;
        Button btnSearchHere;
        FusedLocationProviderClient LocationProviderClient;
        StoreDetailsFragment StoreDetails;
        CardView StoreDetailsCard;


        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            InitControls();
            MapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            MapFragment.GetMapAsync(this);

            CheckLocationPermission();
            ResolveDependencies();
            await SetupLocationProvider();
        }


        async Task SetupLocationProvider()
        {
            var LocRequest = new LocationRequest();
            LocRequest.SetInterval(1000);
            LocRequest.SetFastestInterval(10 * 1000);
            LocRequest.SetSmallestDisplacement(100);
            LocRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            LocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            var locationCallback = new LocationCallbacker();
            locationCallback.MyLocationHandler = LocationCallback_LocationResult;
            await LocationProviderClient.RequestLocationUpdatesAsync(LocRequest, locationCallback);
        }

        async void LocationCallback_LocationResult(object sender, OnLocationCapturedEventArgs e)
        {
            MapsHandler.UpdateCachedLocation(e.Location);
            await SearchNearbyStores();
        }

        private void ResolveDependencies()
        {
            Database = App.DiContainer.Resolve<IRestDatabase>();
        }

        ICollection<string> FoodSuggestions;
        ArrayAdapter<string> FoodAdapter;
        private void InitControls()
        {
            this.fabCenter = FindViewById<FloatingActionButton>(Resource.Id.fabCenter);
            this.btnSearchHere = FindViewById<Button>(Resource.Id.btnSearchHere);
            this.txtSearch = FindViewById<AutoCompleteTextView>(Resource.Id.txtSearch);
            this.StoreDetails = (StoreDetailsFragment)SupportFragmentManager.FindFragmentById(Resource.Id.storeDetailsFragment);
            this.StoreDetailsCard = FindViewById<CardView>(Resource.Id.storeDetailsCardView);

            //InitNavigationView();
            InitTxtSearch();

            fabCenter.Click += FabCenter_Click;
            btnSearchHere.Click += BtnSearchHere_Click;

        }
        //void InitNavigationView()
        //{
        //    NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
        //    navigationView.SetNavigationItemSelectedListener(this);
        //}
        private async void BtnSearchHere_Click(object sender, EventArgs e)
        {
            await SearchNearbyStores(MainMap.Projection.VisibleRegion.LatLngBounds.Center);
            ApplySearchFilter(NearbyStores);
        }

        #region InitTxtSearch.
        void InitTxtSearch()
        {
            txtSearch.Click += TxtSearch_Click;
            txtSearch.EditorAction += TxtSearch_EditorAction;
            txtSearch.Touch += TxtSearch_Touch;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            FetchFoodSuggestions();
            txtSearch.Adapter = FoodAdapter = new ArrayAdapter<string>(this, Resource.Layout.list_item, FoodSuggestions.ToArray());
        }
        private void TxtSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (txtSearch.Text.Length > 0)
            {
                txtSearch.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.ic_cross_81577_32, 0);
            }
            else
            {
                txtSearch.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
            }
            txtSearch.ShowDropDown();

        }
        private void TxtSearch_Touch(object sender, View.TouchEventArgs e)
        {
            var rightDrawable = txtSearch.GetCompoundDrawables()[2];
            if (rightDrawable == null || e.Event.Action != MotionEventActions.Up)
            {
                e.Handled = false;
                return;
            }
            if (e.Event.GetX() >= txtSearch.Width - txtSearch.TotalPaddingRight)
            {
                txtSearch.Text = string.Empty;
                e.Handled = true;
            }
            (sender as AutoCompleteTextView)?.OnTouchEvent(e.Event);
        }
        private void TxtSearch_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            UpdateFoodSuggestionsStorage();
            //@ToDo: replace this minimalistic extension with an actual search/suggestions engine.
            ApplySearchFilter(NearbyStores);
            HideKeyboar(this);
            ActiveSearch = true;
        }
        void ApplySearchFilter(IEnumerable<StoreUser> StoresToFilter)
        {
            IEnumerable<StoreUser> FilteredByFood = StoresToFilter.FilteredByFood(txtSearch.Text);
            MapsHandler.Draw(FilteredByFood.Select(x => new LatLng(x.Latitude, x.Longitude)), BitmapDescriptorFactory.HueBlue);
        }
        void HideKeyboar(Activity YourThis)
        {
            var InputManager = (InputMethodManager)YourThis.GetSystemService(InputMethodService);
            _ = InputManager.HideSoftInputFromWindow(YourThis.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
        }

        private void TxtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.ShowDropDown();
        }
        #endregion





        void FabCenter_Click(object sender, EventArgs e)
        {
            CenterOnCurrentLocation();
        }
        bool ActiveSearch { get; set; }


        #region FoodSuggestions storage.
        void FetchFoodSuggestions()
        {
            FoodSuggestions = LocalDatabase.GetFoodSearchHistory();
        }
        void UpdateFoodSuggestionsStorage()
        {
            if (FoodSuggestions.Contains(txtSearch.Text))
                return;
            FoodAdapter.Add(txtSearch.Text);
            FoodSuggestions.Add(txtSearch.Text);
            LocalDatabase.SaveFoodSearchHistory(FoodSuggestions);
        }
        #endregion

        #region NearbyStores.
        HashSet<StoreUser> NearbyStores = new HashSet<StoreUser>(new UserEqualityComparer());
        //@ToDo move this to the MapHandler. NearbyStores could be part of the MapHandler's state.
        async Task SearchNearbyStores(LatLng Location = null)
        {
            if (MainMap.CameraPosition.Zoom <= 8)
                return;
            EssentialsLocation TargetLocation = Location is null ? await GetCurrentLocation() : new EssentialsLocation(Location.Latitude, Location.Longitude);
            //This is painfully suboptimal: we're fetching the same results over and over again, and then comparing them to avoid redrawing.
            var FetchResult = (IEnumerable<StoreUser>)await Database.GetNearbyStoresAsync(new MyLocationRequest(TargetLocation.Longitude, TargetLocation.Latitude));
            NearbyStores.UnionWith(FetchResult);

            foreach (StoreUser Store in NearbyStores)
            {
                MapsHandler.Draw(new LatLng(Store.Latitude, Store.Longitude), ResolveStoreIcon(), Store);
            }
        }
        private BitmapDescriptor ResolveStoreIcon()
        {
            float Zoom = MainMap.CameraPosition.Zoom;
            BitmapDescriptor Output;
            switch (Zoom)
            {
                case var _ when Zoom < 8: Output = BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_store_mall_directory_128_64); break;
                case var _ when Zoom < 12: Output = BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_store_mall_directory_128_48); break;
                case var _ when Zoom < 15: Output = BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_store_mall_directory_128_32); break;
                case var _ when Zoom < 18: Output = BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_store_mall_directory_128_24); break;
                default: Output = null; break;
            }
            return Output;
        }
        #endregion
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //MenuInflater.Inflate(Resource.Menu.nav_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //int id = item.ItemId;
            //if (id == Resource.Id.action_settings)
            //{
            //    return true;
            //}

            return base.OnOptionsItemSelected(item);
        }
        //@ToDo: move these events to the MapHandler, providing an assignment interface for the caller.
        public void OnMapReady(GoogleMap googleMap)
        {
            MainMap = googleMap;
            MainMap.MyLocationEnabled = true;
            MainMap.UiSettings.MyLocationButtonEnabled = false;
            MainMap.CameraIdle += MainMap_CameraIdle;
            MainMap.MarkerClick += Map_MarkerClick;
            string MapKey = Resources.GetString(Resource.String.maps_key);
            MapsHandler = new MapHandler(MapKey, MainMap);
            CenterOnCurrentLocation(false);
        }
        void Map_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            //@ToDo: extract this expression into a method call that makes it more evident. I find it rather confusing.
            var Store = (e.Marker.Tag as JavaObjectWrapper<object>).Obj as StoreUser;
            if (Store is null)
                return;
            //ToDo: update StoreDetails fragment.
            StoreDetailsCard.Visibility = ViewStates.Visible;
            StoreDetails.Update(Store);
        }
        async void MainMap_CameraIdle(object sender, EventArgs e)
        {
            if (ActiveSearch)
            {
                btnSearchHere.Visibility = ViewStates.Visible;
            }
            //await SearchNearbyPlaces();
        }
        async void CenterOnCurrentLocation(bool Animate = true)
        {
            if (!CheckLocationPermission())
                return;
            await MapsHandler.CenterOnCurrentLocation(Animate);
        }

        #region Persmissions.
        bool CheckLocationPermission()
        {
            if ((int)Build.VERSION.SdkInt < 23)
                return true;
            bool PermissionGranted;
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted &&
                Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
            {
                PermissionGranted = false;
                RequestPermissions(PermissionGroupLocation, LocationRequestId);
            }
            else
            {
                PermissionGranted = false;
            }
            return PermissionGranted;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        #endregion

        async Task<EssentialsLocation> GetCurrentLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(60));
            return await Geolocation.GetLocationAsync(request);
        }


        public static readonly int RC_INSTALL_GOOGLE_PLAY_SERVICES = 1000;
        [Obsolete("Not being currently used.")]
        bool TestIfGooglePlayServicesIsInstalled()
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                //string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Dialog errorDialog = GoogleApiAvailability.Instance.GetErrorDialog(this, queryResult, RC_INSTALL_GOOGLE_PLAY_SERVICES);
                var dialogFrag = new MyErrorDialogFragment(errorDialog);

                dialogFrag.Show(FragmentManager, "GooglePlayServicesDialog");
            }

            return false;
        }

        //@ToDo Finish sidebar design and events.
        //@Body User settings, logout, about.
        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            throw new NotImplementedException();
        }
    }
}

