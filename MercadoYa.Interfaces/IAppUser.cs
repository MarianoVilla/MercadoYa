using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IAppUser
    {
        string Uid { get; set; }
        string DisplayableName { get; set; }
        string Direction { get; set; }
        string City { get; set; }
        double Longitude { get; set; }
        double Latitude { get; set; }
        string ProfilePic { get; set; }
        string Phone { get; set; }
        string UserType { get; set; }
    }
}
