using Framework.Core;
using Framework.QueryableSource;

namespace Framework.SecuritySystem.UserSource;

public class UserSource<TUserDomainObject>(IQueryableSource queryableSource,
                                           UserPathInfo<TUserDomainObject> userPathInfo) : IUserSource<TUserDomainObject>
{
    public TUserDomainObject? TryGetByName(string name)
    {
        return queryableSource.GetQueryable<TUserDomainObject>()
                              .Where(userPathInfo.Filter)
                              .Where(userPathInfo.NamePath.Select(objName => objName == name))
                              .SingleOrDefault();
    }
}
