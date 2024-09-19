namespace Framework.SecuritySystem.UserSource;

public interface IUserSource<out TUser>
{
    TUser? TryGetByName(string name);
}
