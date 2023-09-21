using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.DiTests;

public static class ExampleSecurityOperation
{
    public static DisabledSecurityOperation Disabled { get; } = SecurityOperation.Disabled;

    public static ContextSecurityOperation<Guid> EmployeeView { get; } = new ContextSecurityOperation<Guid>(nameof(EmployeeView), HierarchicalExpandType.Children, Guid.NewGuid());

    public static ContextSecurityOperation<Guid> EmployeeEdit { get; } = new ContextSecurityOperation<Guid>(nameof(EmployeeEdit), HierarchicalExpandType.Children, Guid.NewGuid());
}
