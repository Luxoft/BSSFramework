using Framework.Authorization.Domain;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityRole
{
    public SecurityRole(Guid id, string name, params SecurityOperation[] operations)
    {
        this.Id = id;
        this.Name = name;
        this.Operations = operations;
    }

    public Guid Id { get; }

    public string Name { get; }

    public IReadOnlyList<SecurityOperation> Operations { get; }

    public string? Description { get; init; }

    public IReadOnlyList<SecurityRole> Children { get; init; } = new List<SecurityRole>();

    public static SecurityRole CreateAdministrator(Guid id, IEnumerable<Type> securityOperationTypes, string? description = null)
    {
        return new SecurityRole(
               id,
               BusinessRole.AdminRoleName,
               securityOperationTypes.SelectMany(SecurityOperationHelper.GetSecurityOperations)
                                     .Distinct()
                                     .Where(operation => operation is not DisabledSecurityOperation && operation.AdminHasAccess)
                                     .ToArray())
               { Description = description };
    }
}
