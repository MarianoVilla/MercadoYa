using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MercadoYa.Model.Concrete;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public class MapsHandler
    {
        public string MapKey { get; set; }
        public MapsHandler(string MapKey)
        {
            this.MapKey = MapKey;
        }
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
    }
}