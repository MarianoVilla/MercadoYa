using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MercadoYa.Model.Concrete;
using Newtonsoft.Json;
using Xamarin.Essentials;
using EssentialsLocation = Xamarin.Essentials.Location;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public class MapHandler
    {
        public string MapKey { get; set; }
        public GoogleMap Map { get; set; }
        public List<Marker> MappedMarkers { get; } = new List<Marker>();

        EssentialsLocation CachedUserLocation;
        public MapHandler(string MapsApiKey, GoogleMap Map)
        {
            this.MapKey = MapsApiKey;
            this.Map = Map;
        }
        public void Draw(LatLng Position, BitmapDescriptor Icon = null)
        {
            var Existing = MappedMarkers.FirstOrDefault(x => x.Position.Longitude == Position.Longitude && x.Position.Latitude == Position.Latitude);
            if(Existing is null)
            {
                DrawNew(Position, Icon);
                return;
            }
            Existing.SetIcon(Icon);
        }
        public void Draw(IEnumerable<LatLng> Positions, BitmapDescriptor Icon = null)
        {
            foreach(var p in Positions)
                Draw(p, Icon);
        }
        public void Draw(IEnumerable<LatLng> Positions, float Hue)
        {
            foreach (var p in Positions)
                Draw(p, BitmapDescriptorFactory.DefaultMarker(Hue));
        }
        void DrawNew(LatLng Position, BitmapDescriptor Icon = null)
        {
            var Options = new MarkerOptions();
            Options.SetPosition(Position);
            Options.SetIcon(Icon ?? BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen));
            var AddedMarker = Map.AddMarker(Options);
            MappedMarkers.Add(AddedMarker);
        }
        public async Task CenterOnCurrentLocation(bool Animate = true, int Zoom = 17)
        {
            //CachedUserLocation = CachedUserLocation ?? await GetCurrentLocation();
            CachedUserLocation = await GetCurrentLocation();
            var MyPosition = new LatLng(CachedUserLocation.Latitude, CachedUserLocation.Longitude);
            if (Animate)
                Map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(MyPosition, Zoom));
            else
                Map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(MyPosition, Zoom));
            //MapsHandler.Draw(MyPosition);
        }
        async Task<EssentialsLocation> GetCurrentLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(60));
            return await Geolocation.GetLocationAsync(request);
        }

        public void UpdateCachedLocation(Android.Locations.Location location)
        {
            if (CachedUserLocation is null)
                return;
            CachedUserLocation.Latitude = location.Latitude;
            CachedUserLocation.Longitude = location.Longitude;
        }

        #region GeoCode API stuff.
        public string GetGeoCodeUrl(double Lat, double Lng)
        {
            return $@"https://maps.googleapis.com/maps/api/geocode/json?latlng={Lat},{Lng}&key={MapKey}";
        }
        public async Task<string> GetGeoJsonAsync(string Url)
        {
            var Handler = new HttpClientHandler();
            var Client = new HttpClient(Handler);
            return await Client.GetStringAsync(Url);
        }
        public async Task<string> FindCoordinateAddress(LatLng Position)
        {
            var Placemarks = await Geocoding.GetPlacemarksAsync(Position.Latitude, Position.Longitude);
            string Url = GetGeoCodeUrl(Position.Latitude, Position.Longitude);
            string Json = string.Empty;
            string PlaceAddress = string.Empty;

            Json = await GetGeoJsonAsync(Url);

            if (!string.IsNullOrWhiteSpace(Json))
            {
                var GeoCodeData = JsonConvert.DeserializeObject<GeocodingResult>(Json);
                if (!GeoCodeData.status.Contains("ZERO"))
                {
                    if (GeoCodeData.results[0] != null)
                    {
                        PlaceAddress = GeoCodeData.results[0].formatted_address;
                    }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
                }
            }
            return PlaceAddress;
        }
        #endregion
    }
}