using System.Linq.Expressions;

using Framework.Core;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.UserSource;

public class UserPathInfoRelativeService<TUserDomainObject>(IServiceProvider serviceProvider, UserPathInfo<TUserDomainObject> userPathInfo)
    : IUserPathInfoRelativeService
{
    public Expression<Func<TDomainObject, Guid>> GetId<TDomainObject>()
    {
        return this.GetRelativeDomainPathInfo<TDomainObject>().Path.Select(userPathInfo.IdPath);
    }

    public Expression<Func<TDomainObject, string>> GetName<TDomainObject>()
    {
        return this.GetRelativeDomainPathInfo<TDomainObject>().Path.Select(userPathInfo.NamePath);
    }

    private IRelativeDomainPathInfo<TDomainObject, TUserDomainObject> GetRelativeDomainPathInfo<TDomainObject>() =>
        serviceProvider.GetRequiredService<IRelativeDomainPathInfo<TDomainObject, TUserDomainObject>>();
}
