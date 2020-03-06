using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    public class LocationRequest : ILocationRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }
        public int LimitTo { get; set; }
        public LocationRequest() { }
        public LocationRequest(double Longitude, double Latitude, double Radius = 500, int LimitTo = 100)
        {
            this.Longitude = Math.Round(Longitude, 8);
            this.Latitude = Math.Round(Latitude, 8);
            this.Radius = Radius;
            this.LimitTo = LimitTo;
        }
    }
}
