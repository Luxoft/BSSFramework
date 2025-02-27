using System.Linq.Expressions;

using Framework.Core;
using Framework.QueryableSource;
using Framework.SecuritySystem.Credential;

namespace Framework.SecuritySystem.UserSource;

public class UserSource<TUser>(IQueryableSource queryableSource, UserPathInfo<TUser> userPathInfo) : IUserSource<TUser>
    where TUser : class
{
    public TUser? TryGetUser(UserCredential userCredential) => this.GetQueryable(userCredential).SingleOrDefault();

    public TUser GetUser(UserCredential userCredential) =>
        this.TryGetUser(userCredential) ?? throw this.GetNotFoundException(userCredential);

    User? IUserSource.TryGetUser(UserCredential userCredential) =>
        this.GetQueryable(userCredential).Select(userPathInfo.ToDefaultUserExpr).SingleOrDefault();

    User IUserSource.GetUser(UserCredential userCredential) =>
        ((IUserSource)this).TryGetUser(userCredential) ?? throw this.GetNotFoundException(userCredential);

    private IQueryable<TUser> GetQueryable(UserCredential userCredential) =>
        queryableSource
            .GetQueryable<TUser>()
            .Where(userPathInfo.Filter)
            .Where(this.GetCredentialFilter(userCredential));

    private Expression<Func<TUser, bool>> GetCredentialFilter(UserCredential userCredential)
    {
        switch (userCredential)
        {
            case UserCredential.NamedUserCredential { Name: var name }:
                return userPathInfo.NamePath.Select(objName => objName == name);

            case UserCredential.IdentUserCredential { Id: var id }:
                return userPathInfo.IdPath.Select(objId => objId == id);

            default:
                throw new ArgumentOutOfRangeException(nameof(userCredential));
        }
    }

    private Exception GetNotFoundException(UserCredential userCredential) =>
        new UserSourceException($"{typeof(TUser).Name} \"{userCredential}\" not found");
}
