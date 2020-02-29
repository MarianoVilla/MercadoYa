using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IDiContainer
    {
        IRegisterOptions Register<RegisterType, RegisterImplementation>()                       where RegisterType : class
            where RegisterImplementation : class, RegisterType;

        ResolveType Resolve<ResolveType>() 
            where ResolveType : class;
    }
    public interface IRegisterOptions
    {
        IRegisterOptions AsSingleton();
        IRegisterOptions AsMultiInstance();
        IRegisterOptions WithWeakReference();
        IRegisterOptions WithStrongReference();
    }
}
