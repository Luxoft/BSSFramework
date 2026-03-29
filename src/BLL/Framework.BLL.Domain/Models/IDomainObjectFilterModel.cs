using System.Linq.Expressions;

namespace Framework.BLL.Domain.Models;

public interface IDomainObjectFilterModel<TDomainObject>
{
    Expression<Func<TDomainObject, bool>> ToFilterExpression();
}
