using Framework.SecuritySystem.Credential;
using Framework.SecuritySystem.UserSource;

namespace Framework.SecuritySystem.Services;

public interface IRunAsManager
{
    User? RunAsUser { get; }

    Task StartRunAsUserAsync(UserCredential userCredential, CancellationToken cancellationToken = default);

    Task FinishRunAsUserAsync(CancellationToken cancellationToken = default);
}
