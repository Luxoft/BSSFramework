
using Framework.Core;

namespace Framework.SecuritySystem.UserSource;

public class UserIdentitySource<TUserDomainObject>(
    IUserSource<TUserDomainObject> userSource,
    UserPathInfo<TUserDomainObject> userPathInfo)
    : IUserIdentitySource
{
    public Guid? TryGetId(string name)
    {
        var user = userSource.TryGetByName(name);

        return user == null ? null : userPathInfo.IdPath.Eval(user);
    }
}
