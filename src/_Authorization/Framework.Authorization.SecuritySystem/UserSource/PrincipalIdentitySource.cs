using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class PrincipalIdentitySource<TUserDomainObject>(
    [DisabledSecurity] IRepository<TUserDomainObject> userRepository,
    UserPathInfo<TUserDomainObject> userPathInfo)
    : IPrincipalIdentitySource
{
    public Guid? TryGetId(string name)
    {
        var user = userRepository.GetQueryable().Where(userPathInfo.Filter).SingleOrDefault(userPathInfo.NamePath.Select(v => v == name));

        return user == null ? null : userPathInfo.IdPath.Eval(user);
    }
}
