using System;
using System.Linq.Expressions;

using Framework.DomainDriven;

namespace Framework.Workflow.Domain
{
    public abstract class DomainObjectFilterModel<TDomainObject> : DomainObjectBase, IDomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
    {
        public virtual Expression<Func<TDomainObject, bool>> ToFilterExpression()
        {
            return _ => true;
        }
    }
}