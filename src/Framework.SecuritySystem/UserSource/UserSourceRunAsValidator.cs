using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem.UserSource;

public class UserSourceRunAsValidator<TUser>(IUserSource<TUser> userSource) : IRunAsValidator
{
    public void Validate(string name) => _ = userSource.GetByName(name);
}
