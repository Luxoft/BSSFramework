using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using NHibernate.Linq;

using SampleSystem.BLL._Command.CreateClassA;
using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.WebApiCore.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        [HttpPost(nameof(CreateClassA))]
        public async Task CreateClassA(int value, bool withSession, CancellationToken cancellationToken)
        {
            if (withSession)
            {
                var repository = this.classARepositoryFactory.Create(BLLSecurityMode.Disabled);

                var classA = await repository.GetQueryable().Where(x => x.Value == value).SingleOrDefaultAsync(cancellationToken);

                if (classA != null)
                {
                    throw new Exception("Should not exist yet");
                }
            }

            await this.mediator.Send(new CreateClassAEvent(value), cancellationToken);
        }
    }
}
