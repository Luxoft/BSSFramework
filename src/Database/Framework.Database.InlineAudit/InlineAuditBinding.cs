using System.Reflection;

using Anch.Core;

namespace Framework.Database.InlineAudit;

public record InlineAuditBinding<TDomainObject, TProperty, TAuditValueResolver>(PropertyAccessors<TDomainObject, TProperty?> PropertyAccessors) : InlineAuditBinding
    where TAuditValueResolver : IAuditValueResolver<TProperty>
    where TProperty : notnull
{
    public override Type AuditValueResolverType { get; } = typeof(TAuditValueResolver);

    public override Type DomainObjectType { get; } = typeof(TDomainObject);

    public override Type PropertyType { get; } = typeof(TProperty);

    public override PropertyInfo Property { get; } = PropertyAccessors.Path.GetProperty();
}

public abstract record InlineAuditBinding
{
    public required InlineAuditAction Action { get; init; }

    public abstract Type AuditValueResolverType { get; }

    public abstract Type DomainObjectType { get; }

    public abstract Type PropertyType { get; }

    public abstract PropertyInfo Property { get; }
}
