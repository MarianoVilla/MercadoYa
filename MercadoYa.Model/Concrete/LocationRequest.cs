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
    }
}
