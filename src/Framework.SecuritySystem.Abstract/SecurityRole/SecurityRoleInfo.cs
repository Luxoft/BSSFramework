#nullable enable

using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public record SecurityRoleInfo(Guid Id)
{
    public HierarchicalExpandType ExpandType { get; init; } = HierarchicalExpandType.Children;

    public SecurityPathRestriction Restriction { get; init; } = SecurityPathRestriction.Empty;

    public IReadOnlyList<SecurityOperation> Operations { get; } = [];

    public IReadOnlyList<SecurityRole> Children { get; init; } = [];

    public string? Description { get; init; }

    public string? CustomName { get; init; }
}
