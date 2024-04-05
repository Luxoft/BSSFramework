using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public record SecurityRule<T>(T Value) : SecurityRule
{
    public sealed override string ToString() => this.Value.ToString();
}

public abstract record SecurityRule
{
    public static SecurityRule View { get; } = new SecurityRule<string>(nameof(View));

    public static SecurityRule Edit { get; } = new SecurityRule<string>(nameof(Edit));

    /// <summary>
    /// Специальная правило доступа для отключения безопасности
    /// </summary>
    public static SecurityRule Disabled { get; } = new SecurityRule<string>(nameof(Disabled));

    /// <summary>
    /// Тип разворачивания деревьев (как правило для операции просмотра самого дерева выбирается HierarchicalExpandType.All)
    /// </summary>
    public HierarchicalExpandType ExpandType { get; init; } = HierarchicalExpandType.Children;


    public static implicit operator SecurityRule(SecurityOperation securityOperation)
    {
        return new SecurityRule<SecurityOperation>(securityOperation) { ExpandType = securityOperation.ExpandType };
    }

    public static implicit operator SecurityRule(SecurityRole securityRole)
    {
        return new[] { securityRole };
    }

    public static implicit operator SecurityRule(SecurityRole[] securityRoles)
    {
        return new SecurityRule<SecurityRole[]>(securityRoles);
    }
}
