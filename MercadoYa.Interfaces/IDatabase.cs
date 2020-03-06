using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IDatabase
    {
        string AddStoreUser(IAppUser User, IUserCredentials Credentials);
        string AddCustomerUser(IAppUser User, IUserCredentials Credentials);
        IAppUser GetStoreUser(string Uid);
        IAppUser GetCustomerUser(string Uid);
        IAppUser GetUser(string Uid);
        IAppUser GetUserByEmail(string Email);
        IUserCredentials GetUserCredentials(string Email);
        IEnumerable<IAppUser> GetNearbyStores(ILocationRequest Request);
        //IEnumerable<IAppUser> GetNearbyStoresIncludingTags(ILocationRequest Request);
        //IEnumerable<IAppUser> GetNearbyStoresIncludingFoods(ILocationRequest Request);
        IEnumerable<ITag> GetTagsForUser(IAppUser User);
        IEnumerable<ITag> GetFoodTagsForUser(IAppUser User);
    }
}
