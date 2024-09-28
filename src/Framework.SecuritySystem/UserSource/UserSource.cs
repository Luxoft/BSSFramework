using Framework.Core;
using Framework.QueryableSource;

namespace Framework.SecuritySystem.UserSource;

public class UserSource<TUser>(IQueryableSource queryableSource, UserPathInfo<TUser> userPathInfo) : IUserSource<TUser>
{
    public IQueryable<TUser> GetQueryable(string name) => queryableSource.GetQueryable<TUser>()
                                                                         .Where(userPathInfo.Filter)
                                                                         .Where(userPathInfo.NamePath.Select(objName => objName == name));

    public Guid? TryGetId(string name) =>
        this.GetQueryable(name)
            .Select(userPathInfo.IdPath)
            .Select(v => (Guid?)v).SingleOrDefault();

    public Guid GetId(string name) =>

        this.TryGetId(name) ?? throw new UserSourceException($"{typeof(TUser).Name} \"{name}\" not found");
}
