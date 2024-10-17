using Framework.SecuritySystem.Credential;
using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem.UserSource;

public class UserSourceRunAsValidator<TUser>(IUserSource<TUser> userSource) : IRunAsValidator
{
    public void Validate(UserCredential userCredential) => _ = userSource.GetUser(userCredential);
}
