using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public abstract record SecurityRule
{
    /// <summary>
    /// Правило доступа для просмотра объекта
    /// </summary>
    public static ModeSecurityRule View { get; } = new(nameof(View));

    /// <summary>
    /// Правило доступа для редактирования объекта
    /// </summary>
    public static ModeSecurityRule Edit { get; } = new(nameof(Edit));

    /// <summary>
    /// Правило доступа для отключения безопасности
    /// </summary>
    public static ProviderSecurityRule Disabled { get; } = new(typeof(ISecurityProvider<>), nameof(Disabled));

    /// <summary>
    /// Правило доступа для доменных объектов привязанных к текущему пользователю
    /// </summary>
    public static ProviderSecurityRule CurrentUser { get; } = new(typeof(ISecurityProvider<>), nameof(CurrentUser));


    public static implicit operator SecurityRule(SecurityOperation securityOperation) => securityOperation.ToSecurityRule();

    public static implicit operator SecurityRule(SecurityRole securityRole) => securityRole.ToSecurityRule();

    public static implicit operator SecurityRule(SecurityRole[] securityRoles) => securityRoles.ToSecurityRule();

    public record ModeSecurityRule(string Name) : SecurityRule
    {
        public override string ToString() => this.Name;
    }

    public abstract record DomainSecurityRule : SecurityRule
    {
        public static implicit operator DomainSecurityRule(SecurityOperation securityOperation) => securityOperation.ToSecurityRule();

        public static implicit operator DomainSecurityRule(SecurityRole securityRole) => securityRole.ToSecurityRule();

        public static implicit operator DomainSecurityRule(SecurityRole[] securityRoles) => securityRoles.ToSecurityRule();
    }

    public record ProviderSecurityRule(Type GenericSecurityProviderType, string? Key = null) : DomainSecurityRule
    {
        public override string ToString() => this.Key ?? base.ToString();
    }

    public record ProviderFactorySecurityRule(Type GenericSecurityProviderFactoryType, string? Key = null) : DomainSecurityRule
    {
        public override string ToString() => this.Key ?? base.ToString();
    }

    public record ConditionSecurityRule(Type GenericConditionFactoryType) : DomainSecurityRule;

    public abstract record RoleBaseSecurityRule : DomainSecurityRule
    {
        public static implicit operator RoleBaseSecurityRule(SecurityOperation securityOperation) => securityOperation.ToSecurityRule();

        public static implicit operator RoleBaseSecurityRule(SecurityRole securityRole) => securityRole.ToSecurityRule();

        public static implicit operator RoleBaseSecurityRule(SecurityRole[] securityRoles) => securityRoles.ToSecurityRule();

        /// <summary>
        /// Тип разворачивания деревьев (как правило для просмотра самого дерева выбирается HierarchicalExpandType.All)
        /// </summary>
        public HierarchicalExpandType? CustomExpandType { get; init; } = null;

        public HierarchicalExpandType SafeExpandType => this.CustomExpandType ?? HierarchicalExpandType.Children;
    }

    public record DynamicRoleSecurityRule(Type DynamicRoleFactoryType) : RoleBaseSecurityRule;

    public record OperationSecurityRule(SecurityOperation SecurityOperation) : RoleBaseSecurityRule
    {
        public static implicit operator OperationSecurityRule(SecurityOperation securityOperation) => securityOperation.ToSecurityRule();

        public override string ToString() => this.SecurityOperation.Name;
    }

    /// <summary>
    /// Список ролей ДО разворачиния дерева ролей вверх
    /// </summary>
    /// <param name="SecurityRoles">Список неразвёрнутых ролей</param>
    public record NonExpandedRolesSecurityRule(DeepEqualsCollection<SecurityRole> SecurityRoles) : RoleBaseSecurityRule
    {
        public static implicit operator NonExpandedRolesSecurityRule(SecurityRole securityRole) => securityRole.ToSecurityRule();

        public static implicit operator NonExpandedRolesSecurityRule(SecurityRole[] securityRoles) => securityRoles.ToSecurityRule();

        public override string ToString() => this.SecurityRoles.Count == 1
                                                 ? this.SecurityRoles.Single().Name
                                                 : $"[{this.SecurityRoles.Join(", ", sr => sr.Name)}]";
        public static NonExpandedRolesSecurityRule operator +(NonExpandedRolesSecurityRule rule1, NonExpandedRolesSecurityRule rule2)
        {
            if (rule1.CustomExpandType != rule2.CustomExpandType)
            {
                throw new InvalidOperationException($"Diff {nameof(CustomExpandType)}");
            }
            else
            {
                return new NonExpandedRolesSecurityRule(DeepEqualsCollection.Create(rule1.SecurityRoles.Union(rule2.SecurityRoles)))
                       {
                           CustomExpandType = rule1.CustomExpandType
                       };
            }
        }
    }

    /// <summary>
    /// Список ролей ПОСЛЕ разворачиния дерева ролей вверх
    /// </summary>
    /// <param name="SecurityRoles">Список развёрнутых ролей</param>
    public record ExpandedRolesSecurityRule(DeepEqualsCollection<SecurityRole> SecurityRoles) : RoleBaseSecurityRule
    {
        public override string ToString() => this.SecurityRoles.Count == 1
                                                 ? this.SecurityRoles.Single().Name
                                                 : $"[{this.SecurityRoles.Join(", ", sr => sr.Name)}]";

        public static ExpandedRolesSecurityRule operator +(ExpandedRolesSecurityRule rule1, ExpandedRolesSecurityRule rule2)
        {
            if (rule1.CustomExpandType != rule2.CustomExpandType)
            {
                throw new InvalidOperationException($"Diff {nameof(CustomExpandType)}");
            }
            else
            {
                return new ExpandedRolesSecurityRule(DeepEqualsCollection.Create(rule1.SecurityRoles.Union(rule2.SecurityRoles)))
                       {
                           CustomExpandType = rule1.CustomExpandType
                       };
            }
        }
    }

    public record AndSecurityRule(DomainSecurityRule Left, DomainSecurityRule Right) : DomainSecurityRule;

    public record OrSecurityRule(DomainSecurityRule Left, DomainSecurityRule Right) : DomainSecurityRule;

    public record NegateSecurityRule(DomainSecurityRule InnerRule) : DomainSecurityRule;
}
