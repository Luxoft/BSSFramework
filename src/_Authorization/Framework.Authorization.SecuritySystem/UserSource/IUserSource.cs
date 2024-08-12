namespace Framework.Authorization.SecuritySystem.UserSource;

public interface IUserSource<out TUserDomainObject>
{
    TUserDomainObject? TryGetByName(string name);
}
