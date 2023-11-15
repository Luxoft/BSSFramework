using Framework.Authorization.Domain;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public record SecurityRole(Guid Id, string Name, params SecurityOperation[] Operations)
{
    public string? Description { get; init; }

    public static SecurityRole CreateAdministrator(Guid id, IEnumerable<Type> securityOperationTypes, string? description = null)
    {
        return new SecurityRole(
               id,
               BusinessRole.AdminRoleName,
               securityOperationTypes.SelectMany(SecurityOperationHelper.GetSecurityOperations)
                                     .Distinct()
                                     .Where(operation => operation is not DisabledSecurityOperation && operation.AdminHasAccess)
                                     .ToArray()) { Description = description };
    }
}
