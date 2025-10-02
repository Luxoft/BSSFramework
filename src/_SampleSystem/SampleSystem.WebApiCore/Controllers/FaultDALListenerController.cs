using System.Runtime.CompilerServices;

using Framework.DomainDriven;
using Framework.DomainDriven.Repository;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.ServiceEnvironment;

using SecuritySystem.Attributes;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class FaultDALListenerController(
    ExampleFaultDALListenerSettings listenerSettings,
    [DisabledSecurity] IRepository<NoSecurityObject> repository)
    : ControllerBase
{
    [HttpPost]
    [DBSessionMode(DBSessionMode.Write)]
    public async Task<int> TestFault(bool raiseError, CancellationToken cancellationToken)
    {
        await repository.SaveAsync(new NoSecurityObject(), cancellationToken);

        listenerSettings.Raise = raiseError;

        return 123;
    }

    [HttpPost]
    [DBSessionMode(DBSessionMode.Write)]
    public async IAsyncEnumerable<int> TestFault2(bool raiseError, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await repository.SaveAsync(new NoSecurityObject(), cancellationToken);

        yield return 123;

        await Task.Delay(5000, cancellationToken);

        if (raiseError)
        {
            throw new(nameof(this.TestFault2));
        }

        yield return 234;
    }
}
