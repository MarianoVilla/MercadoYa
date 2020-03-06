using MercadoYa.Interfaces;

namespace MercadoYa.Model.Concrete
{
    public class FullCustomerUser : CustomerUser, IFullAppUser
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
