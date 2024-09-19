namespace Framework.SecuritySystem.UserSource;

public interface ICurrentUserSource<out TUser> : ICurrentUserSource
{
    TUser CurrentUser { get; }
}

public interface ICurrentUserSource
{
    Guid CurrentUserId { get; }
}
