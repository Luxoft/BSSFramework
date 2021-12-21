using System;

namespace Framework.DomainDriven.BLL
{
    public interface IServiceProviderContainer
    {
        IServiceProvider ServiceProvider { get; }
    }
}
