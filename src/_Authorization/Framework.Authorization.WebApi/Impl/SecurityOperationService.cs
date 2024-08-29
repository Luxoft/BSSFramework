using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController(IAvailableSecurityOperationSource availableSecurityOperationSource)
{
    [HttpPost(nameof(GetSecurityOperations))]
    public virtual async Task<IEnumerable<string>> GetSecurityOperations(CancellationToken cancellationToken = default)
    {
        var operations = await availableSecurityOperationSource.GetAvailableSecurityOperations(cancellationToken);

        return operations.Select(op => op.Name);
    }
}
