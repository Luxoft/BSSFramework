using System.Linq.Expressions;

namespace Framework.Database.AuditProperty;

public interface IAuditProperty
{
    LambdaExpression PropertyExpr { get; }

    Delegate GetCurrentValue { get; }
}

public interface IAuditProperty<TDomainObject, TProperty> : IAuditProperty
{
    new Expression<Func<TDomainObject, TProperty>> PropertyExpr { get; }

    new Func<TProperty> GetCurrentValue { get; }
}
