using Microsoft.AspNetCore.Mvc;

using SecuritySystem.AvailableSecurity;

// ReSharper disable once CheckNamespace
namespace Authorization.WebApi.Controllers;

[ApiController]
[Route("authApi/[controller]/[action]")]
public class OperationController(IAvailableSecurityOperationSource availableSecurityOperationSource) : ControllerBase
{
    [HttpPost]
    public async Task<List<string>> GetSecurityOperations(CancellationToken cancellationToken) =>
        await availableSecurityOperationSource
              .GetAvailableSecurityOperations()
              .Select(op => op.Name)
              .ToListAsync(cancellationToken);
}
