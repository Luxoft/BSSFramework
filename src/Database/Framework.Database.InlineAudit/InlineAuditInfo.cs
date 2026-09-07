using Anch.Core;

namespace Framework.Database.InlineAudit;

public record InlineAuditInfo<TDomainObject, TProperty>(PropertyAccessors<TDomainObject, TProperty?> PropertyAccessors) : InlineAuditInfo
{
    public override Type DomainObjectType { get; } = typeof(TDomainObject);

    public override Type PropertyType { get; } = typeof(TProperty);
}

public abstract record InlineAuditInfo
{
    public required InlineAuditType Type { get; init; }

    public abstract Type DomainObjectType { get; }

    public abstract Type PropertyType { get; }
}
