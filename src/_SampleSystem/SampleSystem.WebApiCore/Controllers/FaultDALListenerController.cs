using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.WebApiCore.Controllers.Main;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class FaultDALListenerController : ControllerBase
{
    private readonly FaultDALListener listener;

    private readonly IRepository<NoSecurityObject, Guid> repository;

    public FaultDALListenerController(FaultDALListener listener, IRepositoryFactory<NoSecurityObject, Guid, SampleSystemSecurityOperationCode> repositoryFactory)
    {
        this.listener = listener;
        this.repository = repositoryFactory.Create(BLLSecurityMode.Disabled);
    }

    [HttpPost(nameof(TestFault))]
    [DBSessionMode(DBSessionMode.Write)]
    public async Task TestFault(CancellationToken cancellationToken)
    {
        await this.repository.SaveAsync(new NoSecurityObject(), cancellationToken);

        this.listener.Raise = true;
    }
}
