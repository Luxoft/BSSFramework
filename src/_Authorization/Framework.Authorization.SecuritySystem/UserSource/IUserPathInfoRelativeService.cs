using System.Linq.Expressions;

namespace Framework.Authorization.SecuritySystem.UserSource;

public interface IUserPathInfoRelativeService
{
    Expression<Func<TDomainObject, Guid>> GetId<TDomainObject>();

    Expression<Func<TDomainObject, string>> GetName<TDomainObject>();
}
