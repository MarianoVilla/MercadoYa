using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IAppUser
    {
        string UserName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string Phone { get; set; }
        string Direction { get; set; }
        string City { get; set; }
        double Longitude { get; set; }
        double Latitude { get; set; }
        string ProfilePic { get; set; }
        string Uid { get; set; }
    }
}
