using System;

namespace Framework.DomainDriven.WebApiNetCore
{
    public interface IApiControllerBase
    {
        string PrincipalName { get; set; }

        IServiceProvider ServiceProvider { get; set; }
    }
}
