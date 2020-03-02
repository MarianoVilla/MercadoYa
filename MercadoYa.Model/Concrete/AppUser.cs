using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    public class AppUser : IAppUser
    {
        public string Uid { get; set; }
        public string Direction { get; set; }
        public string City { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string ProfilePic { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public string DisplayableName { get; set; }
    }
}
