using System.Linq.Expressions;

namespace SampleSystem.Domain.Models.Filters._Base;

public abstract class DomainObjectRootFilterModel<TDomainObject> : DomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    public override Expression<Func<TDomainObject, bool>> ToFilterExpression() => _ => true;
}
