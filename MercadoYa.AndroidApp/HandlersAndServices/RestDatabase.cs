using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MercadoYa.Interfaces;
using MercadoYa.Model;
using MercadoYa.Model.Concrete;

namespace MercadoYa.AndroidApp.Handlers_nd_Helpers
{
    public class RestDatabase : IRestDatabase
    {
        public async Task<IEnumerable<IAppUser>> GetNearbyStoresAsync(ILocationRequest Request)
        {
            IEnumerable<IAppUser> Stores = new List<IAppUser>();
            using HttpResponseMessage Res = await Const.GlobalHttpClient.PostAsJsonAsync("Database/nearbystores?NestedTags=Foods", Request);
            if (Res.IsSuccessStatusCode)
            {
                using HttpContent content = Res.Content;
                Stores = await content.ReadAsAsync<IEnumerable<StoreUser>>();
            }
            return Stores;
        }

        public Task<IEnumerable<IAppUser>> GetNearbyStoresAsync(double Longitude, double Latitude) 
            => GetNearbyStoresAsync(new LocationRequest(Longitude, Latitude));
    }
}