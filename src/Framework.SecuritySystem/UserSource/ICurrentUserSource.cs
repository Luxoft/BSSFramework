namespace Framework.SecuritySystem.UserSource;

public interface ICurrentUserSource<out TUserDomainObject> : ICurrentUserSource
{
    TUserDomainObject CurrentUser { get; }
}

public interface ICurrentUserSource
{
    Guid CurrentUserId { get; }
}
