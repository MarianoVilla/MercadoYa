using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    /// <summary>
    /// Describes the interface for a failure listener.
    /// </summary>
    public interface IOnFailureListener
    {
        void OnFailure(Exception e);
    }
}
