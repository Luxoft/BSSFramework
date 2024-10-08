using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public abstract record DomainSecurityRule : SecurityRule
{
    /// <summary>
    /// Правило доступа для доменных объектов привязанных к текущему пользователю
    /// </summary>
    public static CurrentUserSecurityRule CurrentUser { get; } = new();

    /// <summary>
    /// Правило доступа для блокирования доступа
    /// </summary>
    public static ProviderSecurityRule AccessDenied { get; } = new(typeof(ISecurityProvider<>), nameof(AccessDenied));

    /// <summary>
    /// Любая роль
    /// </summary>
    public static AnyRoleSecurityRule AnyRole { get; } = new();


    public static implicit operator DomainSecurityRule(SecurityOperation securityOperation) => securityOperation.ToSecurityRule();

    public static implicit operator DomainSecurityRule(SecurityRole securityRole) => securityRole.ToSecurityRule();

    public static implicit operator DomainSecurityRule(SecurityRole[] securityRoles) => securityRoles.ToSecurityRule();

    public record CurrentUserSecurityRule(string? RelativePathKey = null) : DomainSecurityRule
    {
        public override string ToString() => this.RelativePathKey ?? nameof(CurrentUser);
    }

    public record ProviderSecurityRule(Type GenericSecurityProviderType, string? Key = null) : DomainSecurityRule
    {
        public override string ToString() => this.Key ?? base.ToString();
    }

    public record ProviderFactorySecurityRule(Type GenericSecurityProviderFactoryType, string? Key = null) : DomainSecurityRule
    {
        public override string ToString() => this.Key ?? base.ToString();
    }

    public record ConditionFactorySecurityRule(Type GenericConditionFactoryType) : DomainSecurityRule;

    public record RelativeConditionSecurityRule(RelativeConditionInfo RelativeConditionInfo) : DomainSecurityRule;

    public record FactorySecurityRule(Type RuleFactoryType) : DomainSecurityRule;

    public record SecurityRuleHeader(string Name) : DomainSecurityRule
    {
        public override string ToString() => this.Name;
    }

    public record OverrideAccessDeniedMessageSecurityRule(DomainSecurityRule BaseSecurityRule, string CustomMessage) : DomainSecurityRule;

    public abstract record RoleBaseSecurityRule : DomainSecurityRule
    {
        public static implicit operator RoleBaseSecurityRule(SecurityOperation securityOperation) => securityOperation.ToSecurityRule();

        public static implicit operator RoleBaseSecurityRule(SecurityRole securityRole) => securityRole.ToSecurityRule();

        public static implicit operator RoleBaseSecurityRule(SecurityRole[] securityRoles) => securityRoles.ToSecurityRule();

        public SecurityPathRestriction? CustomRestriction { get; init; } = null;

        public SecurityRuleCredential? CustomCredential { get; init; } = null;

        /// <summary>
        /// Тип разворачивания деревьев (как правило для просмотра самого дерева выбирается HierarchicalExpandType.All)
        /// </summary>
        public HierarchicalExpandType? CustomExpandType { get; init; } = null;

        public HierarchicalExpandType SafeExpandType => this.CustomExpandType ?? HierarchicalExpandType.Children;

        public bool EqualsCustoms(RoleBaseSecurityRule other)
        {
            return this.CustomExpandType == other.CustomExpandType
                   && this.CustomCredential == other.CustomCredential
                   && this.CustomRestriction == other.CustomRestriction;
        }
    }

    public record AnyRoleSecurityRule : RoleBaseSecurityRule;

    public record RoleFactorySecurityRule(Type RoleFactoryType) : RoleBaseSecurityRule;

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
            if (!rule1.EqualsCustoms(rule2))
            {
                throw new InvalidOperationException("Diff customs");
            }
            else
            {
                return new NonExpandedRolesSecurityRule(DeepEqualsCollection.Create(rule1.SecurityRoles.Union(rule2.SecurityRoles)))
                       {
                           CustomExpandType = rule1.CustomExpandType,
                           CustomCredential = rule1.CustomCredential,
                           CustomRestriction = rule1.CustomRestriction
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

        public static ExpandedRolesSecurityRule Create(IEnumerable<SecurityRole> securityRoles)
        {
            return new ExpandedRolesSecurityRule(DeepEqualsCollection.Create(securityRoles));
        }

        public static ExpandedRolesSecurityRule operator +(ExpandedRolesSecurityRule rule1, ExpandedRolesSecurityRule rule2)
        {
            if (!rule1.EqualsCustoms(rule2))
            {
                throw new InvalidOperationException("Diff customs");
            }
            else
            {
                return new ExpandedRolesSecurityRule(DeepEqualsCollection.Create(rule1.SecurityRoles.Union(rule2.SecurityRoles)))
                       {
                           CustomExpandType = rule1.CustomExpandType,
                           CustomCredential = rule1.CustomCredential,
                           CustomRestriction = rule1.CustomRestriction
                       };
            }
        }
    }

    public record AndSecurityRule(DomainSecurityRule Left, DomainSecurityRule Right) : DomainSecurityRule;

    public record OrSecurityRule(DomainSecurityRule Left, DomainSecurityRule Right) : DomainSecurityRule;

    public record NegateSecurityRule(DomainSecurityRule InnerRule) : DomainSecurityRule;
}
