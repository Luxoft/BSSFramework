using System;

namespace Framework.DomainDriven.WebApiNetCore
{
    public interface IApiControllerBase
    {
        IServiceProvider ServiceProvider { get; set; }
    }
}
