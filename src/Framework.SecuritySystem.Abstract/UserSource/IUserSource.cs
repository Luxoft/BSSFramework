namespace Framework.SecuritySystem.UserSource;

public interface IUserSource<out TUser>
{
    TUser? TryGetByName(string name);

    TUser GetByName(string name)
    {
        return this.TryGetByName(name) ?? throw new UserSourceException($"{typeof(TUser).Name} \"{name}\" not found");
    }
}
