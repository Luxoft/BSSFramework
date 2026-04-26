using Anch.SecuritySystem.AvailableSecurity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Framework.Authorization.WebApi;

public partial class AuthMainController
{
    [HttpPost]
    public virtual async Task<List<string>> GetSecurityOperations(CancellationToken cancellationToken = default)
    {
        var availableSecurityOperationSource = this.HttpContext.RequestServices.GetRequiredService<IAvailableSecurityOperationSource>();

        return await availableSecurityOperationSource
                     .GetAvailableSecurityOperations()
                     .Select(op => op.Name)
                     .ToListAsync(cancellationToken);
    }
}
