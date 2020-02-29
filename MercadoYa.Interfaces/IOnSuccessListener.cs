using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    /// <summary>
    /// Describes the interface for a success listener.
    /// </summary>
    public interface IOnSuccessListener
    {
        void OnSuccess(object Result);
    }
}
