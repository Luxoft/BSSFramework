namespace Framework.SecuritySystem.UserSource;

public interface IUserSourceRunAsAccessor<TUser>
{
    TUser? GetRunAs(TUser user);

    void SetRunAs(TUser user, TUser? targetUser);
}
