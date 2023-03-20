using System;
using System.Linq.Expressions;

namespace SampleSystem.Domain;

public abstract class DomainObjectRootFilterModel<TDomainObject> : DomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    public override Expression<Func<TDomainObject, bool>> ToFilterExpression()
    {
        return _ => true;
    }
}
