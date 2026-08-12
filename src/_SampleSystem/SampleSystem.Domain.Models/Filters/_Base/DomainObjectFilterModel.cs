using System.Linq.Expressions;

using Framework.BLL.Domain.Models;

namespace SampleSystem.Domain.Models.Filters._Base;

public abstract class DomainObjectFilterModel<TDomainObject> : DomainObjectBase, IDomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    public abstract Expression<Func<TDomainObject, bool>> ToFilterExpression();
}
