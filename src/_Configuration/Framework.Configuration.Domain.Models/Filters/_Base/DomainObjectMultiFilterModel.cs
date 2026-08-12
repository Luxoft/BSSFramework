using System.Linq.Expressions;

using Anch.Core;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

public abstract class DomainObjectMultiFilterModel<TDomainObject> : DomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    public override Expression<Func<TDomainObject, bool>> ToFilterExpression() => this.ToFilterExpressionItems().BuildAnd();

    protected abstract IEnumerable<Expression<Func<TDomainObject, bool>>> ToFilterExpressionItems();
}
