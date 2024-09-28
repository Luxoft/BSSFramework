namespace Framework.SecuritySystem.UserSource;

public interface IUserSource<out TUser> : IUserSource
{
    TUser? TryGetByName(string name);

    TUser GetByName(string name);
}

public interface IUserSource
{
    Guid? TryGetId(string name);

    Guid GetId(string name);
}
