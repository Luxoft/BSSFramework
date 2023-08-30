using System.Linq.Expressions;

namespace Framework.DomainDriven;

public interface IDomainObjectFilterModel<TDomainObject>
{
    Expression<Func<TDomainObject, bool>> ToFilterExpression();
}
