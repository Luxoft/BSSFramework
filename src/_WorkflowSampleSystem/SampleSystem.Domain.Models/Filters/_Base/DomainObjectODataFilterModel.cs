using System;
using System.Linq.Expressions;

using Framework.DomainDriven;

namespace SampleSystem.Domain
{
    public abstract class DomainObjectODataFilterModel<TDomainObject> : DomainObjectBase, IDomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
    {
        public abstract Expression<Func<TDomainObject, bool>> ToFilterExpression();
    }
}