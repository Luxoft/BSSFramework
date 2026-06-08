using Anch.SecuritySystem.AvailableSecurity;

using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Authorization.WebApi.Controllers;

[ApiController]
[Route("authApi/[controller]/[action]")]
public class OperationController(IAvailableSecurityOperationSource availableSecurityOperationSource) : ControllerBase
{
    [HttpPost]
    public async Task<List<string>> GetSecurityOperations(CancellationToken ct) =>
        await availableSecurityOperationSource
              .GetAvailableSecurityOperations()
              .Select(op => op.Name)
              .ToListAsync(ct);
}

