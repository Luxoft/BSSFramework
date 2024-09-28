using Framework.Core;
using Framework.QueryableSource;

namespace Framework.SecuritySystem.UserSource;

public class UserSource<TUser> : IUserSource<TUser>
{
    private readonly IQueryableSource queryableSource;

    private readonly UserPathInfo<TUser> userPathInfo;

    private readonly Lazy<TUser> lazyCurrentUser;

    public UserSource (ICurrentUser currentUser, IQueryableSource queryableSource, UserPathInfo<TUser> userPathInfo)
    {
        this.queryableSource = queryableSource;
        this.userPathInfo = userPathInfo;

        this.lazyCurrentUser = LazyHelper.Create(() => this.GetByName(currentUser.Name));
    }

    public TUser CurrentUser => this.lazyCurrentUser.Value;

    public TUser? TryGetByName(string name) => this.GetQueryable(name).SingleOrDefault();

    public TUser GetByName(string name) => this.TryGetByName(name) ?? throw this.GetNotFoundException(name);

    public Guid? TryGetId(string name) => this.GetQueryable(name).Select(this.userPathInfo.IdPath).Select(v => (Guid?)v).SingleOrDefault();

    public Guid GetId(string name) =>

        this.TryGetId(name) ?? throw this.GetNotFoundException(name);

    private IQueryable<TUser> GetQueryable(string name) =>
        this.queryableSource
            .GetQueryable<TUser>()
            .Where(this.userPathInfo.Filter)
            .Where(this.userPathInfo.NamePath.Select(objName => objName == name));

    private Exception GetNotFoundException(string name) => new UserSourceException($"{typeof(TUser).Name} \"{name}\" not found");
}
