#nullable enable
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public record SecurityOperationInfo
{
    public HierarchicalExpandType? CustomExpandType { get; init; } = null;

    public string? Description { get; init; } = null;
}
