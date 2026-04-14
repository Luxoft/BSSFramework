using System.Linq.Expressions;

using Framework.BLL.Domain.Models;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

public abstract class DomainObjectFilterModel<TDomainObject> : DomainObjectBase, IDomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    public virtual Expression<Func<TDomainObject, bool>> ToFilterExpression() => _ => true;
}
