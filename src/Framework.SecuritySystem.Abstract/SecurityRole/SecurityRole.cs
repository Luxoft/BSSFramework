#nullable enable

using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityRole(Guid id, string name)
{
    public Guid Id { get; } = id;

    public string Name { get; } = name;

    public string? Description { get; init; }

    public SecurityRuleRestriction? Restriction { get; init; } = null;

    public IReadOnlyList<SecurityRole> Children { get; init; } = [];

    public IReadOnlyList<SecurityOperation> Operations { get; init; } = [];

    public override string ToString() => this.Name;

    public static SecurityRole CreateAdministrator(
        Guid id,
        IEnumerable<Type> securityRoleTypes,
        IEnumerable<Type>? securityOperationTypes = null,
        IEnumerable<string>? exceptPropertyNames = null,
        string? description = null)
    {
        const string administratorRoleName = "Administrator";

        var realExceptPropertyNames = (exceptPropertyNames ?? [administratorRoleName, "SystemIntegration"]).ToHashSet();

        var children = securityRoleTypes
                       .SelectMany(
                           securityRoleType =>
                               securityRoleType.GetStaticPropertyValueList<SecurityRole>(
                                   srName => !realExceptPropertyNames.Contains(srName)))
                       .Distinct()
                       .ToList();

        var operations = securityOperationTypes
                         .EmptyIfNull()
                         .SelectMany(securityOperationType => securityOperationType.GetStaticPropertyValueList<SecurityOperation>())
                         .Distinct()
                         .ToArray();

        return new SecurityRole(id, administratorRoleName) { Children = children, Description = description, Operations = operations };
    }
}
