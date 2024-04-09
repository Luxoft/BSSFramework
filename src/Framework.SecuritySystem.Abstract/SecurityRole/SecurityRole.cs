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

    public static SecurityRole CreateAdministrator(
        Guid id,
        IEnumerable<Type> securityRoleTypes,
        string? description = null)
    {
        const string administratorRoleName = "Administrator";

        var children = securityRoleTypes
                       .SelectMany(
                           securityRoleType => SecurityRoleHelper.GetSecurityRoles(
                               securityRoleType,
                               srName => srName != administratorRoleName))
                       .Distinct()
                       .ToList();

        return new SecurityRole(id, administratorRoleName)
               {
                   Children = children,
                   Description = description
               };
    }
}
