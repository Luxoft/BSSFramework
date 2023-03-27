using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Audit;

public class AuditProperty<TDomainObject, TProperty> : IAuditProperty<TDomainObject, TProperty>
{
    public AuditProperty([NotNull] Expression<Func<TDomainObject, TProperty>> propertyExpr, [NotNull] Func<TProperty> getCurrentValue)
    {
        if (propertyExpr == null) throw new ArgumentNullException(nameof(propertyExpr));
        if (getCurrentValue == null) throw new ArgumentNullException(nameof(getCurrentValue));

        this.PropertyExpr = propertyExpr;
        this.GetCurrentValue = getCurrentValue;
    }


    public Expression<Func<TDomainObject, TProperty>> PropertyExpr { get; private set; }

    public Func<TProperty> GetCurrentValue { get; private set; }


    LambdaExpression IAuditProperty.PropertyExpr
    {
        get { return this.PropertyExpr; }
    }

    Delegate IAuditProperty.GetCurrentValue
    {
        get { return this.GetCurrentValue; }
    }
}
