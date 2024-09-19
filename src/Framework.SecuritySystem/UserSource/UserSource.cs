using Framework.Core;
using Framework.QueryableSource;

namespace Framework.SecuritySystem.UserSource;

public class UserSource<TUser>(IQueryableSource queryableSource,
                                           UserPathInfo<TUser> userPathInfo) : IUserSource<TUser>
{
    public TUser? TryGetByName(string name)
    {
        return queryableSource.GetQueryable<TUser>()
                              .Where(userPathInfo.Filter)
                              .Where(userPathInfo.NamePath.Select(objName => objName == name))
                              .SingleOrDefault();
    }
}
