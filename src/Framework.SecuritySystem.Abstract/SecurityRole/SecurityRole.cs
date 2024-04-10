using Framework.Core;

namespace Framework.SecuritySystem;

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

    public override string ToString() => this.Name;

    public static SecurityRole CreateAdministrator(
        Guid id,
        IEnumerable<Type> securityRoleTypes,
        IEnumerable<Type> securityOperationTypes = null,
        string? description = null)
    {
        const string administratorRoleName = "Administrator";

        var children = securityRoleTypes
                       .SelectMany(
                           securityRoleType => securityRoleType.GetStaticPropertyValueList<SecurityRole>(srName => srName != administratorRoleName))
                       .Distinct()
                       .ToList();

        var operations = securityOperationTypes
                         .EmptyIfNull()
                         .SelectMany(securityOperationType => securityOperationType.GetStaticPropertyValueList<SecurityOperation>())
                         .Distinct()
                         .ToArray();

        return new SecurityRole(id, administratorRoleName, operations)
               {
                   Children = children,
                   Description = description
               };
    }
}
