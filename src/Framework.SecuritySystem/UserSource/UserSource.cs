using Framework.Core;
using Framework.QueryableSource;

namespace Framework.SecuritySystem.UserSource;

public class UserSource<TUser>(IQueryableSource queryableSource, UserPathInfo<TUser> userPathInfo) : IUserSource<TUser>
{
    public TUser? TryGetByName(string name) => this.GetQueryable(name).SingleOrDefault();

    public TUser GetByName(string name) => this.TryGetByName(name) ?? throw this.GetNotFoundException(name);

    public Guid? TryGetId(string name) => this.GetQueryable(name).Select(userPathInfo.IdPath).Select(v => (Guid?)v).SingleOrDefault();

    public Guid GetId(string name) =>

        this.TryGetId(name) ?? throw this.GetNotFoundException(name);

    private IQueryable<TUser> GetQueryable(string name) =>
        queryableSource
            .GetQueryable<TUser>()
            .Where(userPathInfo.Filter)
            .Where(userPathInfo.NamePath.Select(objName => objName == name));

    private Exception GetNotFoundException(string name) => new UserSourceException($"{typeof(TUser).Name} \"{name}\" not found");
}
