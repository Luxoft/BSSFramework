using Framework.SecuritySystem.AvailableSecurity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [HttpPost]
    public virtual async Task<IEnumerable<string>> GetSecurityOperations(CancellationToken cancellationToken = default)
    {
        var availableSecurityOperationSource = this.HttpContext.RequestServices.GetRequiredService<IAvailableSecurityOperationSource>();

        var operations = await availableSecurityOperationSource.GetAvailableSecurityOperations(cancellationToken);

        return operations.Select(op => op.Name);
    }
}
