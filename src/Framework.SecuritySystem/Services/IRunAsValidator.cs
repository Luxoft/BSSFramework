using Framework.SecuritySystem.Credential;

namespace Framework.SecuritySystem.Services;

public interface IRunAsValidator
{
    void Validate(UserCredential userCredential);
}
