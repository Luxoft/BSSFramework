using Framework.Authorization.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Authorization.WebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("authApi/v{version:apiVersion}/[controller]")]
public class OperationController : ControllerBase
{
    private readonly IAvailableSecurityRoleSource availableSecurityRoleSource;

    public OperationController(IAvailableSecurityRoleSource availableSecurityRoleSource)
    {
        this.availableSecurityRoleSource = availableSecurityRoleSource;
    }

    [HttpPost(nameof(GetSecurityOperations))]
    public async Task<IEnumerable<string>> GetSecurityOperations(CancellationToken cancellationToken)
    {
        var roles = await this.availableSecurityRoleSource.GetAvailableSecurityRole(cancellationToken);

        return roles.SelectMany(sr => sr.Information.Operations).Distinct().Select(op => op.Name);
    }
}
