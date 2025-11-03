using Framework.DomainDriven;
using Framework.DomainDriven.Repository;

using GenericQueryable;

using MediatR;

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
