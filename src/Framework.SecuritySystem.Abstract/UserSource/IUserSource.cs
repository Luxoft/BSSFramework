namespace Framework.SecuritySystem.UserSource;

public interface IUserSource<out TUser>
{
    IQueryable<TUser> GetQueryable(string name);

    TUser? TryGetByName(string name) => this.GetQueryable(name).SingleOrDefault();

    TUser GetByName(string name)
    {
        return this.TryGetByName(name) ?? throw new UserSourceException($"{typeof(TUser).Name} \"{name}\" not found");
    }
}
