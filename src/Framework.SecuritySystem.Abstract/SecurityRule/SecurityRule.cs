#nullable enable

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public abstract record SecurityRule
{
    public static SpecialSecurityRule View { get; } = new (nameof(View));

    public static SpecialSecurityRule Edit { get; } = new (nameof(Edit));

    /// <summary>
    /// Специальная правило доступа для отключения безопасности
    /// </summary>
    public static SpecialSecurityRule Disabled { get; } = new (nameof(Disabled));


    public static implicit operator SecurityRule(SecurityOperation securityOperation)
    {
        return securityOperation.ToSecurityRule();
    }

    public static implicit operator SecurityRule(SecurityRole securityRole)
    {
        return securityRole.ToSecurityRule();
    }

    public static implicit operator SecurityRule(SecurityRole[] securityRoles)
    {
        return securityRoles.ToSecurityRule();
    }

    public record SpecialSecurityRule(string Name) : SecurityRule
    {
        public override string ToString() => this.Name;
    }

    public abstract record DomainObjectSecurityRule : SecurityRule
    {
        /// <summary>
        /// Тип разворачивания деревьев (как правило для просмотра самого дерева выбирается HierarchicalExpandType.All)
        /// </summary>
        public HierarchicalExpandType? CustomExpandType { get; init; } = null;


        public static implicit operator DomainObjectSecurityRule(SecurityOperation securityOperation)
        {
            return securityOperation.ToSecurityRule();
        }

        public static implicit operator DomainObjectSecurityRule(SecurityRole securityRole)
        {
            return securityRole.ToSecurityRule();
        }
    }

    public record OperationSecurityRule(SecurityOperation SecurityOperation) : DomainObjectSecurityRule
    {
        public override string ToString() => this.SecurityOperation.Name;
    }

    /// <summary>
    /// Список ролей ДО разворачиния дерева ролей вверх
    /// </summary>
    /// <param name="SecurityRoles">Список неразвёрнутых ролей</param>
    public record NonExpandedRolesSecurityRule(DeepEqualsCollection<SecurityRole> SecurityRoles) : DomainObjectSecurityRule
    {
        public override string ToString() => this.SecurityRoles.Count == 1
                                                 ? this.SecurityRoles.Single().Name
                                                 : $"{nameof(this.SecurityRoles)}: {this.SecurityRoles.Join(", ", sr => sr.Name)}";
    }

    /// <summary>
    /// Список ролей ПОСЛЕ разворачиния дерева ролей вверх
    /// </summary>
    /// <param name="SecurityRoles">Список развёрнутых ролей</param>
    public record ExpandedRolesSecurityRule(DeepEqualsCollection<SecurityRole> SecurityRoles) : DomainObjectSecurityRule
    {
        public override string ToString() => this.SecurityRoles.Count == 1
                                                 ? this.SecurityRoles.Single().Name
                                                 : $"{nameof(this.SecurityRoles)}: {this.SecurityRoles.Join(", ", sr => sr.Name)}";
    }

    /// <summary>
    /// Композитное правило
    /// </summary>
    /// <param name="Children"></param>
    public record CompositeSecurityRule(DeepEqualsCollection<DomainObjectSecurityRule> Children) : DomainObjectSecurityRule
    {
        public override string ToString() => this.Children.Count == 1
                                                 ? this.Children.Single().ToString()
                                                 : $"{nameof(this.Children)}: {this.Children.Join(", ")}";
    }
}
