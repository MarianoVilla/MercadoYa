using System.Collections.Generic;

namespace MercadoYa.Interfaces
{
    /// <summary>
    /// Describes the interface for a standar authentication result.
    /// </summary>
    public interface IAuthResult
    {
        /// <summary>
        /// An object that implements IAppUser.
        /// </summary>
        IAppUser User { get; }
        /// <summary>
        /// A general purpose container for any additional data one might want to add to the AuthResult.
        /// </summary>
        IDictionary<string, object> AdditionalAuthInfo { get; }
    }
}