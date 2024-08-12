using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.UserSource;

public class UserSource<TUserDomainObject>([DisabledSecurity] IRepository<TUserDomainObject> userRepository,
                                           UserPathInfo<TUserDomainObject> userPathInfo) : IUserSource<TUserDomainObject>
{
    public TUserDomainObject? TryGetByName(string name)
    {
        return userRepository.GetQueryable()
                             .Where(userPathInfo.Filter)
                             .Where(userPathInfo.NamePath.Select(objName => objName == name))
                             .SingleOrDefault();
    }
}
