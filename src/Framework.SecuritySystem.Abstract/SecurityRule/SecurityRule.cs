#nullable enable

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public abstract record SecurityRule
{
    public static SpecialSecurityRule View { get; } = new(nameof(View));

    public static SpecialSecurityRule Edit { get; } = new(nameof(Edit));

    /// <summary>
    /// Специальная правило доступа для отключения безопасности
    /// </summary>
    public static DisabledSecurityRule Disabled { get; } = new();


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
        public static implicit operator DomainObjectSecurityRule(SecurityOperation securityOperation)
        {
            return securityOperation.ToSecurityRule();
        }

        public static implicit operator DomainObjectSecurityRule(SecurityRole securityRole)
        {
            return securityRole.ToSecurityRule();
        }

        public static implicit operator DomainObjectSecurityRule(SecurityRole[] securityRoles)
        {
            return securityRoles.ToSecurityRule();
        }
    }

    public record DisabledSecurityRule : DomainObjectSecurityRule
    {
        public override string ToString() => nameof(Disabled);
    }

    public record CustomProviderSecurityRule(Type GenericSecurityProviderType) : DomainObjectSecurityRule;

    public abstract record ExpandableSecurityRule : DomainObjectSecurityRule
    {
        public static implicit operator ExpandableSecurityRule(SecurityOperation securityOperation)
        {
            return securityOperation.ToSecurityRule();
        }

        public static implicit operator ExpandableSecurityRule(SecurityRole securityRole)
        {
            return securityRole.ToSecurityRule();
        }

        public static implicit operator ExpandableSecurityRule(SecurityRole[] securityRoles)
        {
            return securityRoles.ToSecurityRule();
        }

        /// <summary>
        /// Тип разворачивания деревьев (как правило для просмотра самого дерева выбирается HierarchicalExpandType.All)
        /// </summary>
        public HierarchicalExpandType? CustomExpandType { get; init; } = null;

        public HierarchicalExpandType SafeExpandType => this.CustomExpandType ?? HierarchicalExpandType.Children;
    }

    public record OperationSecurityRule(SecurityOperation SecurityOperation) : ExpandableSecurityRule
    {
        public static implicit operator OperationSecurityRule(SecurityOperation securityOperation)
        {
            return securityOperation.ToSecurityRule();
        }

        public override string ToString() => this.SecurityOperation.Name;
    }

    /// <summary>
    /// Список ролей ДО разворачиния дерева ролей вверх
    /// </summary>
    /// <param name="SecurityRoles">Список неразвёрнутых ролей</param>
    public record NonExpandedRolesSecurityRule(DeepEqualsCollection<SecurityRole> SecurityRoles) : ExpandableSecurityRule
    {
        public override string ToString() => this.SecurityRoles.Count == 1
                                                 ? this.SecurityRoles.Single().Name
                                                 : $"[{this.SecurityRoles.Join(", ", sr => sr.Name)}]";
    }

    /// <summary>
    /// Список ролей ПОСЛЕ разворачиния дерева ролей вверх
    /// </summary>
    /// <param name="SecurityRoles">Список развёрнутых ролей</param>
    public record ExpandedRolesSecurityRule(DeepEqualsCollection<SecurityRole> SecurityRoles) : ExpandableSecurityRule
    {
        public override string ToString() => this.SecurityRoles.Count == 1
                                                 ? this.SecurityRoles.Single().Name
                                                 : $"[{this.SecurityRoles.Join(", ", sr => sr.Name)}]";
    }

    public record AndSecurityRule(DomainObjectSecurityRule Left, DomainObjectSecurityRule Right) : DomainObjectSecurityRule;

    public record OrSecurityRule(DomainObjectSecurityRule Left, DomainObjectSecurityRule Right) : DomainObjectSecurityRule;
}
