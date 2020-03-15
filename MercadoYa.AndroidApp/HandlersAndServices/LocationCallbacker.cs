using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public class LocationCallbacker : LocationCallback
    {
        public EventHandler<OnLocationCapturedEventArgs> MyLocationHandler;
        public class OnLocationCapturedEventArgs : EventArgs
        {
            public Android.Locations.Location Location { get; set; }
        }
        public override void OnLocationAvailability(LocationAvailability locationAvailability)
        {
            Log.Debug("MercadoYa", $"IsLocationAvailable: {locationAvailability.IsLocationAvailable}");
            base.OnLocationAvailability(locationAvailability);
        }
        public override void OnLocationResult(LocationResult result)
        {
            if(result.Locations.Count != 0)
            {
                MyLocationHandler?.Invoke(this, new OnLocationCapturedEventArgs { Location = result.Locations[0] });
            }
        }
    }
}