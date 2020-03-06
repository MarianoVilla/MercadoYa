using MercadoYa.Interfaces;

namespace MercadoYa.Model.Concrete
{
    public class FullStoreUser : StoreUser, IFullAppUser
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
