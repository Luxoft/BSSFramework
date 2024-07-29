using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public record SecurityRoleInfo(Guid Id)
{
    public HierarchicalExpandType? CustomExpandType { get; init; } = null;

    public SecurityPathRestriction Restriction { get; init; } = SecurityPathRestriction.Empty;

    public IReadOnlyList<SecurityOperation> Operations { get; init; } = [];

    public IReadOnlyList<SecurityRole> Children { get; init; } = [];

    public string? Description { get; init; }

    public string? CustomName { get; init; }
}
