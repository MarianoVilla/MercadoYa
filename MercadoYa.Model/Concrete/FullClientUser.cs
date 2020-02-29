using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    public class FullClientUser : IFullAppUser
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Uid { get; set; }
        public string Direction { get; set; }
        public string City { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string ProfilePic { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
    }
}
