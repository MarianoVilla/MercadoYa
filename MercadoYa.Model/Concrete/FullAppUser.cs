using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    /// <summary>
    /// By now we are only using this concrete for the MobileUser. Later on, we'll need to add derivatives for the client and store users. 
    /// Note: YAGNI does not apply; I'm rather sure I'll need to separate these.
    /// </summary>
    public class FullAppUser : AppUser, IFullAppUser
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
