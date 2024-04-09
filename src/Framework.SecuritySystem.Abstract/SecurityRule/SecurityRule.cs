using Framework.Core;

using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public abstract record SecurityRule
{
    public static SecurityRule View { get; } = new SpecialSecurityRule(nameof(View));

    public static SecurityRule Edit { get; } = new SpecialSecurityRule(nameof(Edit));

    /// <summary>
    /// Специальная правило доступа для отключения безопасности
    /// </summary>
    public static SecurityRule Disabled { get; } = new SpecialSecurityRule(nameof(Disabled));

    /// <summary>
    /// Тип разворачивания деревьев (как правило для операции просмотра самого дерева выбирается HierarchicalExpandType.All)
    /// </summary>
    public HierarchicalExpandType ExpandType { get; init; } = HierarchicalExpandType.Children;


    public static implicit operator SecurityRule(SecurityOperation securityOperation)
    {
        return new OperationSecurityRule(securityOperation) { ExpandType = securityOperation.ExpandType };
    }

    public static implicit operator SecurityRule(SecurityRole securityRole)
    {
        return new[] { securityRole };
    }

    public static implicit operator SecurityRule(SecurityRole[] securityRoles)
    {
        return new RolesSecurityRule(new DeepEqualsCollection<SecurityRole>(securityRoles));
    }

    public record SpecialSecurityRule(string Name) : SecurityRule
    {
        public override string ToString() => this.Name;
    }

    public abstract record DomainObjectSecurityRule : SecurityRule;

    public record OperationSecurityRule(SecurityOperation SecurityOperation) : DomainObjectSecurityRule
    {
        public override string ToString() => this.SecurityOperation.Name;
    }

    public record RolesSecurityRule(DeepEqualsCollection<SecurityRole> SecurityRoles) : DomainObjectSecurityRule
    {
        public override string ToString() => this.SecurityRoles.Count == 1
                                                 ? this.SecurityRoles.Single().Name
                                                 : $"SecurityRoles: {this.SecurityRoles.Join(", ", sr => sr.Name)}";
    }
}
