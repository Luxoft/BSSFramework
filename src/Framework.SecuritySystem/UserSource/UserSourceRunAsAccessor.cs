using Framework.Core;

namespace Framework.SecuritySystem.UserSource;

public class UserSourceRunAsAccessor<TUser>(UserSourceRunAsAccessorData<TUser> data) : IUserSourceRunAsAccessor<TUser>
{
    private readonly Func<TUser, TUser?> getRunAsFunc = data.Path.Compile(LambdaCompileCache.Default);

    private readonly Action<TUser, TUser?> setRunAsAction = data.Path.GetProperty().GetSetValueAction<TUser, TUser?>();

    public TUser? GetRunAs(TUser user) => this.getRunAsFunc(user);

    public void SetRunAs(TUser user, TUser? targetUser) => this.setRunAsAction(user, targetUser);
}
