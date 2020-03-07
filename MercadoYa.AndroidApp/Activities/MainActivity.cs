using Android;
using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MercadoYa.AndroidApp.Fragments;
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.AndroidApp.HandlersAndServices;
using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using MercadoYa.Model.Concrete;
using Xamarin.Essentials;
using EssentialsLocation = Xamarin.Essentials.Location;
using MyLocationRequest = MercadoYa.Model.Concrete.LocationRequest;

namespace MercadoYa.AndroidApp.Activities
{
    //TODO: refactor the fuck out of this.
    [Activity(Label = "@string/app_name", Theme = "@style/MercadoYa.Theme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {
        GoogleMap MainMap;
        readonly string[] PermissionGroupLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        const int LocationRequestId = 0;
        LocationRequest LocRequest;
        FusedLocationProviderClient LocationClient;
        Android.Locations.Location LastLocation;
        LocationCallbackHelper LocationCallback;
        AutoCompleteTextView txtSearch;
        MapsHandler MapsHandler;
        IRestDatabase Database;
        List<string> MappedStores = new List<string>();

        //UserProfileEventListener ProfileEventListener;
        SupportMapFragment MapFragment;

        static int UPDATE_INTERVAL = 5;
        static int FASTEST_INTERVAL = 5;
        static int DISPLACEMENT = 3;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            InitControls();
            TestIfGooglePlayServicesIsInstalled();
            //ProfileEventListener = new UserProfileEventListener();
            MapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);




            CheckLocationPermission();
            CreateLocationRequest();
            StartLocationUpdate();
            //ProfileEventListener.Create();
            ResolveDependencies();

        }
        protected override void OnResume()
        {
            base.OnResume();
            MapFragment.GetMapAsync(this);
        }
        private void ResolveDependencies()
        {
            Database = App.DiContainer.Resolve<IRestDatabase>();
        }
        List<string> FoodSuggestions;
        ArrayAdapter<String> FoodAdapter;
        private void InitControls()
        {
            this.txtSearch = FindViewById<AutoCompleteTextView>(Resource.Id.txtSearch);

            txtSearch.Click += TxtSearch_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.EditorAction += TxtSearch_EditorAction;
            UpdateFoodSuggestions();
            txtSearch.Adapter = FoodAdapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, FoodSuggestions);
        }

        private void TxtSearch_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            FoodAdapter.Add(txtSearch.Text);
            FoodSuggestions.Add(txtSearch.Text);
            LocalDatabase.SaveFoodSearchHistory(FoodSuggestions);
        }

        private void TxtSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
        }

        private void TxtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.ShowDropDown();
            GetSuggestions();
        }
        void UpdateFoodSuggestions()
        {
            FoodSuggestions = LocalDatabase.GetFoodSearchHistory().ToList();
        }
        private void GetSuggestions()
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {

            }
        }

        async Task SearchNearbyPlaces()
        {
            if (MainMap.CameraPosition.Zoom <= 8)
                return;
            EssentialsLocation CurrentLocation = await GetCurrentLocation();
            IEnumerable<IAppUser> NearbyStores = await Database.GetNearbyStoresAsync(new MyLocationRequest(CurrentLocation.Longitude, CurrentLocation.Latitude));
            foreach (IAppUser store in NearbyStores)
            {
                var Options = new MarkerOptions();
                Options.SetPosition(new LatLng(store.Latitude, store.Longitude));
                Options.SetTitle(store.DisplayableName);

                BitmapDescriptor bmDescriptor = ResolveStoreIcon();
                Options.SetIcon(bmDescriptor);

                MainMap.AddMarker(Options);
                MappedStores.Add(store.Uid);
            }
        }

        private BitmapDescriptor ResolveStoreIcon()
        {
            float Zoom = MainMap.CameraPosition.Zoom;
            switch (Zoom)
            {
                case var _ when Zoom < 8: return BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_store_mall_directory_128_64);
                case var _ when Zoom < 12: return BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_store_mall_directory_128_48);
                case var _ when Zoom < 15: return BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_store_mall_directory_128_32);
                case var _ when Zoom < 18: return BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_store_mall_directory_128_24);
                default: return null;
            }
        }

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

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            var view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            MainMap = googleMap;
            MainMap.MyLocationEnabled = true;
            UpdateMapLocation();
            MainMap.CameraIdle += MainMap_CameraIdle;
            MainMap.CameraChange += MainMap_CameraChange;
            string MapKey = Resources.GetString(Resource.String.maps_key);
            MapsHandler = new MapsHandler(MapKey);
        }

        void MainMap_CameraChange(object sender, GoogleMap.CameraChangeEventArgs e)
        {
            //var currentZoomLevel = MainMap.CameraPosition.Zoom;
            //if ((int)e.Position.Zoom == (int)currentZoomLevel)
            //    return;

            //currentZoomLevel = (int)e.Position.Zoom;
        }

        async void MainMap_CameraIdle(object sender, EventArgs e)
        {
            //TODO: add a current location/search location text edit.
            //Then we can use the MapsHandler to get the location updated:
            //var CurrentLocation = MainMap.CameraPosition.Target;
            //var CurrentLocationText = MapsHandler.FindCoordinateAddress(CurrentLocation);
            await SearchNearbyPlaces();
        }

        bool CheckLocationPermission()
        {
            if ((int)Build.VERSION.SdkInt < 23)
                return true;
            bool PermissionGranted = false;
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
        void CreateLocationRequest()
        {
            LocRequest = new LocationRequest();
            LocRequest.SetInterval(UPDATE_INTERVAL);
            LocRequest.SetFastestInterval(FASTEST_INTERVAL);
            LocRequest.SetSmallestDisplacement(DISPLACEMENT);
            LocRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            LocationClient = LocationServices.GetFusedLocationProviderClient(this);
            LocationCallback = new LocationCallbackHelper();
            LocationCallback.MyLocation += LocationCallback_MyLocation;
        }
        void LocationCallback_MyLocation(object sender, LocationCallbackHelper.OnLocationCapturedEventArgs e)
        {
            LastLocation = e.Location;
            var Position = new LatLng(LastLocation.Latitude, LastLocation.Longitude);
            MainMap?.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(Position, 17));

        }
        async void UpdateMapLocation()
        {
            if (!CheckLocationPermission())
                return;
            if (LastLocation != null)
            {
                EssentialsLocation UserLocation = await GetCurrentLocation();
                var MyPosition = new LatLng(UserLocation.Latitude, UserLocation.Longitude);
                MainMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(MyPosition, 17));
            }
        }
        async Task<EssentialsLocation> GetCurrentLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(60));
            return await Geolocation.GetLocationAsync(request);
        }
        void StartLocationUpdate()
        {
            if (CheckLocationPermission())
                LocationClient.RequestLocationUpdates(LocRequest, LocationCallback, null);
        }

        public static readonly int RC_INSTALL_GOOGLE_PLAY_SERVICES = 1000;
        bool TestIfGooglePlayServicesIsInstalled()
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Dialog errorDialog = GoogleApiAvailability.Instance.GetErrorDialog(this, queryResult, RC_INSTALL_GOOGLE_PLAY_SERVICES);
                var dialogFrag = new MyErrorDialogFragment(errorDialog);

                dialogFrag.Show(FragmentManager, "GooglePlayServicesDialog");
            }

            return false;
        }
    }
}

