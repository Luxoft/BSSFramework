using System.Linq.Expressions;

using Framework.Core.ExpressionComparers;

namespace Framework.SecuritySystem;

public abstract record SecurityPathRestrictionConditionInfo
{
    public abstract Type RelativeTargetDomainObjectType { get; }
}

public record SecurityPathRestrictionConditionInfo<TRelativeTargetDomainObject>(
    Expression<Func<TRelativeTargetDomainObject, bool>> Condition)
    : SecurityPathRestrictionConditionInfo
{
    public override Type RelativeTargetDomainObjectType { get; } = typeof(TRelativeTargetDomainObject);

    public virtual bool Equals(SecurityPathRestrictionConditionInfo<TRelativeTargetDomainObject>? other) =>
        object.ReferenceEquals(this, other)
        || (other is not null && ExpressionComparer.Value.Equals(this.Condition, other.Condition));

    public override int GetHashCode() => 0;
}
