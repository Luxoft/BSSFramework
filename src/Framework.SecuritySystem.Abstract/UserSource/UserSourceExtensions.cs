namespace Framework.SecuritySystem.UserSource;

public static class UserSourceExtensions
{
    public static TUser GetByName<TUser>(this IUserSource<TUser> userSource, string name)
    {
        return userSource.TryGetByName(name) ?? throw new UserSourceException($"{typeof(TUser).Name} \"{name}\" not found");
    }
}
