using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Lib.Comparers
{
    public class UserEqualityComparer : IEqualityComparer<IAppUser>
    {
        public bool Equals(IAppUser x, IAppUser y) => x?.Uid == y?.Uid;

        public int GetHashCode(IAppUser obj) => obj is null ? 0 : obj.Uid.GetHashCode();
    }
}
