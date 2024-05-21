using Framework.Authorization.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Authorization.WebApi.Controllers;

[ApiController]
[Route("authApi/[controller]")]
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
        var roles = await this.availableSecurityRoleSource.GetAvailableSecurityRoles(cancellationToken);

        return roles.SelectMany(sr => sr.Information.Operations).Distinct().Select(op => op.Name);
    }
}
