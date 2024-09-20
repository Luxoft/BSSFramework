using System.Linq.Expressions;

namespace Framework.DomainDriven.Audit;

public class AuditProperty<TDomainObject, TProperty>(
    Expression<Func<TDomainObject, TProperty>> propertyExpr,
    Func<TProperty> getCurrentValue)
    : IAuditProperty<TDomainObject, TProperty>
{
    public Expression<Func<TDomainObject, TProperty>> PropertyExpr { get; } = propertyExpr;

    public Func<TProperty> GetCurrentValue { get; } = getCurrentValue;

    LambdaExpression IAuditProperty.PropertyExpr => this.PropertyExpr;

    Delegate IAuditProperty.GetCurrentValue => this.GetCurrentValue;
}
