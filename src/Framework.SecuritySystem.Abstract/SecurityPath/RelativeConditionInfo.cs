using System.Linq.Expressions;

using Framework.Core.ExpressionComparers;

namespace Framework.SecuritySystem;

public abstract record RelativeConditionInfo
{
    public abstract Type RelativeDomainObjectType { get; }
}

public record RelativeConditionInfo<TRelativeDomainObject>(Expression<Func<TRelativeDomainObject, bool>> Condition)
    : RelativeConditionInfo
{
    public override Type RelativeDomainObjectType { get; } = typeof(TRelativeDomainObject);

    public virtual bool Equals(RelativeConditionInfo<TRelativeDomainObject>? other) =>
        object.ReferenceEquals(this, other)
        || (other is not null && ExpressionComparer.Value.Equals(this.Condition, other.Condition));

    public override int GetHashCode() => 0;
}

public static class RelativeConditionInfoExtensions
{
    public static RelativeConditionInfo<TRelativeDomainObject> ToInfo<TRelativeDomainObject>(this Expression<Func<TRelativeDomainObject, bool>> condition)
    {
        return new RelativeConditionInfo<TRelativeDomainObject>(condition);
    }
}
