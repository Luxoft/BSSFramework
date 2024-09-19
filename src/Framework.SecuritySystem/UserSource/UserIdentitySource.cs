
using Framework.Core;

namespace Framework.SecuritySystem.UserSource;

public class UserIdentitySource<TUser>(
    IUserSource<TUser> userSource,
    UserPathInfo<TUser> userPathInfo)
    : IUserIdentitySource
{
    public Guid? TryGetId(string name)
    {
        var user = userSource.TryGetByName(name);

        return user == null ? null : userPathInfo.IdPath.Eval(user);
    }
}
