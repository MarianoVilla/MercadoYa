using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    public class AuthResult : IAuthResult
    {
        public IAppUser User { get; }
        public IDictionary<string, object> AdditionalAuthInfo { get; }

        public AuthResult(IAppUser User, IDictionary<string, object> AdditionalInfo)
        {
            this.User = User;
            this.AdditionalAuthInfo = AdditionalInfo;
        }
        public AuthResult(IAppUser User)
        {
            this.User = User;
        }
    }
}
