using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class OperationAccessorFactory : IOperationAccessorFactory
{
    private readonly IServiceProvider serviceProvider;

    public OperationAccessorFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IOperationAccessor Create(bool withRunAs)
    {
        return ActivatorUtilities.CreateInstance<OperationAccessor>(this.serviceProvider, withRunAs);
    }
}
