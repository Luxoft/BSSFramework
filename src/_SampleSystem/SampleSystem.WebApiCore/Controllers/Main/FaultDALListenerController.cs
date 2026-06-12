using System.Runtime.CompilerServices;

using Anch.SecuritySystem.Attributes;

using Framework.Application.Repository;
using Framework.Database;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.ServiceEnvironment;

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
    public async Task<int> TestFault(bool raiseError, CancellationToken ct)
    {
        await repository.SaveAsync(new NoSecurityObject(), ct);

        listenerSettings.Raise = raiseError;

        return 123;
    }

    [HttpPost]
    [DBSessionMode(DBSessionMode.Write)]
    public async IAsyncEnumerable<int> TestFault2(bool raiseError, [EnumeratorCancellation] CancellationToken ct)
    {
        await repository.SaveAsync(new NoSecurityObject(), ct);

        yield return 123;

        await Task.Delay(5000, ct);

        if (raiseError)
        {
            throw new(nameof(this.TestFault2));
        }

        yield return 234;
    }
}

