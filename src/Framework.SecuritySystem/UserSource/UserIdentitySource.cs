namespace Framework.SecuritySystem.UserSource;

public class UserIdentitySource<TUser>(
    IUserSource<TUser> userSource,
    UserPathInfo<TUser> userPathInfo)
    : IUserIdentitySource
{
    public Guid? TryGetId(string name) =>
        userSource.GetQueryable(name)
                  .Select(userPathInfo.IdPath)
                  .Select(v => (Guid?)v).SingleOrDefault();
}
