using Framework.SecuritySystem.Credential;

namespace Framework.SecuritySystem.UserSource;

public interface IUserSource<out TUser> : IUserSource
{
    new TUser? TryGetUser(UserCredential userCredential);

    new TUser GetUser(UserCredential userCredential);
}

public interface IUserSource
{
    User? TryGetUser(UserCredential userCredential);

    User GetUser(UserCredential userCredential);
}
