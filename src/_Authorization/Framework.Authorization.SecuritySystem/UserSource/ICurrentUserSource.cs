namespace Framework.Authorization.SecuritySystem.UserSource;

public interface ICurrentUserSource<out TUserDomainObject>
{
    TUserDomainObject CurrentUser { get; }
}
