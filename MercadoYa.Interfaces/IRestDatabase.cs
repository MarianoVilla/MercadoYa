using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MercadoYa.Interfaces
{
    public interface IRestDatabase
    {
        Task<IEnumerable<IAppUser>> GetNearbyStoresAsync(ILocationRequest Request);
        Task<IEnumerable<IAppUser>> GetNearbyStoresAsync(double Longitude, double Latitude);
    }
}
