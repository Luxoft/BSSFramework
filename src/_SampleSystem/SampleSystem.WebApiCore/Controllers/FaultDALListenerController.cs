using System.Runtime.CompilerServices;

using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]")]
[ApiController]
public class FaultDALListenerController : ControllerBase
{
    private readonly ExampleFaultDALListener listener;

    private readonly IRepository<NoSecurityObject> repository;

    public FaultDALListenerController(ExampleFaultDALListener listener, IRepositoryFactory<NoSecurityObject> repositoryFactory)
    {
        this.listener = listener;
        this.repository = repositoryFactory.Create();
    }

    [HttpPost(nameof(TestFault))]
    [DBSessionMode(DBSessionMode.Write)]
    public async Task<int> TestFault(bool raiseError, CancellationToken cancellationToken)
    {
        await this.repository.SaveAsync(new NoSecurityObject(), cancellationToken);

        this.listener.Raise = raiseError;

        return 123;
    }

    [HttpPost(nameof(TestFault2))]
    [DBSessionMode(DBSessionMode.Write)]
    public async IAsyncEnumerable<int> TestFault2(bool raiseError, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await this.repository.SaveAsync(new NoSecurityObject(), cancellationToken);

        yield return 123;

        await Task.Delay(5000, cancellationToken);

        if (raiseError)
        {
            throw new(nameof(this.TestFault2));
        }

        yield return 234;
    }
}
