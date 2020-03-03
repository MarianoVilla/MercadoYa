using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.Interfaces;
//using MercadoYa.Model.Concrete;
using Xamarin.Essentials;
using MyLocationRequest = MercadoYa.Model.Concrete.LocationRequest;
using EssentialsLocation = Xamarin.Essentials.Location;
using Android.Gms.Common;
using MercadoYa.AndroidApp.Fragments;

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

        UserProfileEventListener ProfileEventListener;

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
            ProfileEventListener = new UserProfileEventListener();
            var MapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            MapFragment.GetMapAsync(this);

            CheckLocationPermission();
            CreateLocationRequest();
            StartLocationUpdate();
            //ProfileEventListener.Create();
            ResolveDependencies();

            //Task.Run(() => SearchNearbyPlaces());

        }
        private void ResolveDependencies()
        {
            Database = App.DiContainer.Resolve<IRestDatabase>();
        }
        string[] FoodSuggestions;
        private void InitControls()
        {
            this.txtSearch = FindViewById<AutoCompleteTextView>(Resource.Id.txtSearch);

            txtSearch.Click += TxtSearch_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            //txtSearch.Adapter = new ArrayAdapter<String> (this, Resource.Layout.list_item, FoodSuggestions);

        }

        private void TxtSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            GetSuggestions();
        }

        //TODO: save recent searches.
        private void TxtSearch_Click(object sender, EventArgs e)
        {
            GetSuggestions();
        }

        private void GetSuggestions()
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
            }
        }
        async Task SearchNearbyPlaces()
        {
            EssentialsLocation CurrentLocation = await GetCurrentLocation();
            var NearbyStores = await Database.GetNearbyStoresAsync(new MyLocationRequest(CurrentLocation.Longitude, CurrentLocation.Latitude));
            foreach(var store in NearbyStores)
            {
                var Options = new MarkerOptions();
                Options.SetPosition(new LatLng(store.Latitude, store.Longitude));
                Options.SetTitle(store.DisplayableName);

                var bmDescriptor = BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_store_mall_directory_128_28856);
                Options.SetIcon(bmDescriptor);

                MainMap.AddMarker(Options);
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
            string MapKey = Resources.GetString(Resource.String.maps_key);
            MapsHandler = new MapsHandler(MapKey);
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
        void LocationCallback_MyLocation(object senter, LocationCallbackHelper.OnLocationCapturedEventArgs e)
        {
            LastLocation = e.Location;
            var Position = new LatLng(LastLocation.Latitude, LastLocation.Longitude);
            MainMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(Position, 17));

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
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
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
            var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                var errorDialog = GoogleApiAvailability.Instance.GetErrorDialog(this, queryResult, RC_INSTALL_GOOGLE_PLAY_SERVICES);
                var dialogFrag = new MyErrorDialogFragment(errorDialog);

                dialogFrag.Show(FragmentManager, "GooglePlayServicesDialog");
            }

            return false;
        }
    }
}

