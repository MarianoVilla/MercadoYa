using GeneralUtil.Model;
using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    public class StoreUser : IAppUser
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
        public int RatingScore { get; set; }
        public string Description { get; set; }
        public string Lore { get; set; }
        public IEnumerable<Interval<DateTime>> OpenIntervals { get; set; }
        public IEnumerable<ITag> Tags { get; set; }
        public IEnumerable<Food> Foods { get; set; }
    }
}
