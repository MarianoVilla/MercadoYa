using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    public class FilteredLocationRequest : ILocationRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }
        public int LimitTo { get; set; }

        public FilteredLocationRequest()
        {

        }
        public FilteredLocationRequest(double Longitude, double Latitude, double Radius = 500, int LimitTo = 100)
        {
            this.Longitude = Longitude;
            this.Latitude = Latitude;
            this.Radius = Radius;
            this.LimitTo = LimitTo;
        }
    }
}
