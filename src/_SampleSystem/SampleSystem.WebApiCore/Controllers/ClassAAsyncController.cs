using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.GenericQueryable;

using GenericQueryable;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL._Command.CreateClassA;
using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.WebApiCore.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ClassAAsyncController : ControllerBase
{
    private readonly IRepositoryFactory<ClassA> classARepositoryFactory;

    private readonly IMediator mediator;

    private readonly IServiceEvaluator<IMediator> mediatorEvaluator;

    public ClassAAsyncController(
        IRepositoryFactory<ClassA> classARepositoryFactory,
        IMediator mediator)
    {
        this.classARepositoryFactory = classARepositoryFactory;
        this.mediator = mediator;
    }

    [DBSessionMode(DBSessionMode.Write)]
    [HttpPost]
    public async Task CreateClassA(int value, bool withSession, CancellationToken cancellationToken)
    {
        if (withSession)
        {
            var repository = this.classARepositoryFactory.Create();

            var classA = await repository.GetQueryable().Where(x => x.Value == value).GenericSingleOrDefaultAsync(cancellationToken);

            if (classA != null) throw new Exception("Should not exist yet");
        }

        await this.mediator.Send(new CreateClassAEvent(value), cancellationToken);
    }
}
