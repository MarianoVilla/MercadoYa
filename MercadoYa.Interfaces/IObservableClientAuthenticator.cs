using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MercadoYa.Interfaces
{
    /// <summary>
    /// Describes the interface for a client-side observable authentication service.
    /// Could've used .NET's <see cref="IObservable{T}"/> and <see cref="IObserver{T}"/>,
    /// but wanted to implement my own just for fun.
    /// Gotta keep those design patterns fresh!
    /// </summary>
    public interface IObservableClientAuthenticator
    {
        List<IOnSuccessListener> OnSuccessListeners { get; }
        List<IOnFailureListener> OnFailureListeners { get; }
        IAuthResult AuthResult { get; }
        /// <summary>
        /// The user resulting from the last successful authentication (if any).
        /// The property was solely created to mimic FirebaseAuth's interface and make the dependency transition seamless.
        /// </summary>
        IAppUser CurrentUser { get; }

        void AddOnSuccessListener(IOnSuccessListener Listener);
        void AddOnFailureListener(IOnFailureListener Listener);

        Task SignInWithEmailAndPasswordAsync(string Email, string Password);
        Task RegisterUserAsync(IFullAppUser User);
        Task CreateStoreAsync(IFullAppUser User);

    }
}
