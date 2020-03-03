using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface ILocationRequest
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        double Radius { get; set; }
        int LimitTo { get; set; }
    }
}
