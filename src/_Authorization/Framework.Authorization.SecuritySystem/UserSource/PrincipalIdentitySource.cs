
using Framework.Core;

namespace Framework.Authorization.SecuritySystem.UserSource;

public class PrincipalIdentitySource<TUserDomainObject>(
    IUserSource<TUserDomainObject> userSource,
    UserPathInfo<TUserDomainObject> userPathInfo)
    : IPrincipalIdentitySource
{
    public Guid? TryGetId(string name)
    {
        var user = userSource.TryGetByName(name);

        return user == null ? null : userPathInfo.IdPath.Eval(user);
    }
}
