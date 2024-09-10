using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Authorization.WebApi.Controllers;

[ApiController]
[Route("authApi/[controller]/[action]")]
public class OperationController(IAvailableSecurityOperationSource availableSecurityOperationSource) : ControllerBase
{
    [HttpPost(nameof(GetSecurityOperations))]
    public async Task<IEnumerable<string>> GetSecurityOperations(CancellationToken cancellationToken)
    {
        var operations = await availableSecurityOperationSource.GetAvailableSecurityOperations(cancellationToken);

        return operations.Select(op => op.Name);
    }
}
