using System.Linq.Expressions;

using Framework.BLL.Domain.Models;

namespace Framework.Configuration.Domain;

public abstract class DomainObjectFilterModel<TDomainObject> : DomainObjectBase, IDomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    public virtual Expression<Func<TDomainObject, bool>> ToFilterExpression() => _ => true;
}
