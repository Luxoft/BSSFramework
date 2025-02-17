﻿using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public record SecurityPathRestriction(
    DeepEqualsCollection<SecurityContextRestriction>? SecurityContextRestrictions,
    DeepEqualsCollection<Type> ConditionFactoryTypes,
    DeepEqualsCollection<RelativeConditionInfo> RelativeConditions)
{
    public SecurityPathRestriction(
        IEnumerable<SecurityContextRestriction>? securityContexts,
        IEnumerable<Type> conditionFactoryTypes,
        IEnumerable<RelativeConditionInfo> relativeConditions)
        : this(
            securityContexts == null ? null : DeepEqualsCollection.Create(securityContexts),
            DeepEqualsCollection.Create(conditionFactoryTypes),
            DeepEqualsCollection.Create(relativeConditions))
    {
    }

    public IEnumerable<Type>? SecurityContextTypes => this.SecurityContextRestrictions?.Select(v => v.Type);

    public static SecurityPathRestriction Disabled { get; } = new(null, Array.Empty<Type>(), []);

    public static SecurityPathRestriction Empty { get; } = new([], Array.Empty<Type>(), []);

    public SecurityPathRestriction Add<TSecurityContext>(bool required = false, string? key = null)
        where TSecurityContext : ISecurityContext =>
        new(
            this.SecurityContextRestrictions.EmptyIfNull()
                .Concat(new[] { new SecurityContextRestriction(typeof(TSecurityContext), required, key) }.Distinct()),
            this.ConditionFactoryTypes,
            this.RelativeConditions);

    public SecurityPathRestriction AddRelativeCondition<TDomainObject>(Expression<Func<TDomainObject, bool>> condition) =>

        new(
            this.SecurityContextRestrictions,
            this.ConditionFactoryTypes,
            this.RelativeConditions.Concat([new RelativeConditionInfo<TDomainObject>(condition)]));

    public SecurityPathRestriction AddConditionFactory(Type conditionFactoryType) =>
        new(this.SecurityContextRestrictions, this.ConditionFactoryTypes.Concat([conditionFactoryType]), this.RelativeConditions);

    public static SecurityPathRestriction Create<TSecurityContext>(bool required = false, string? key = null)
        where TSecurityContext : ISecurityContext => Disabled.Add<TSecurityContext>(required, key);

    public static SecurityPathRestriction Create<TDomainObject>(Expression<Func<TDomainObject, bool>> condition) =>
        Disabled.AddRelativeCondition(condition);
}
