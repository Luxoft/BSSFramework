using Bss.Platform.Mediation.Abstractions;

using Framework.Application.Repository;
using Framework.Database;

using GenericQueryable;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL._Command.CreateClassA;
using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.WebApiCore.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ClassAAsyncController(
    IRepositoryFactory<ClassA> classARepositoryFactory,
    IMediator mediator)
    : ControllerBase
{
    [DBSessionMode(DBSessionMode.Write)]
    [HttpPost]
    public async Task CreateClassA(int value, bool withSession, CancellationToken cancellationToken)
    {
        if (withSession)
        {
            var repository = classARepositoryFactory.Create();

            var classA = await repository.GetQueryable().Where(x => x.Value == value).GenericSingleOrDefaultAsync(cancellationToken);

            if (classA != null) throw new Exception("Should not exist yet");
        }

        await mediator.Send(new CreateClassAEvent(value), cancellationToken);
    }
}
