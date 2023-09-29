using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.DomainServices;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.BLL;

public partial class OperationBLL
{
    public override void Remove(Operation operation)
    {
        this.Context.ServiceProvider.GetRequiredService<IOperationDomainService>().RemoveAsync(operation).GetAwaiter().GetResult();
    }
}
