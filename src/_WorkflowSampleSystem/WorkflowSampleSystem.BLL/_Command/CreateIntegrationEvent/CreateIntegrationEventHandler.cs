using System.Threading;
using System.Threading.Tasks;

using Framework.Cap.Abstractions;

using MediatR;

using WorkflowSampleSystem.BLL.Core.IntegrationEvens;
using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.BLL._Command.CreateIntegrationEvent;

public class CreateIntegrationEventHandler : IRequestHandler<CreateIntegrationEventCommand>
{
    private readonly IIntegrationEventBus integrationEventBus;

    private readonly ICountryBLLFactory countryBllFactory;

    public CreateIntegrationEventHandler(
            IIntegrationEventBus integrationEventBus,
            ICountryBLLFactory countryBllFactory)
    {
        this.integrationEventBus = integrationEventBus;
        this.countryBllFactory = countryBllFactory;
    }

    public Task<Unit> Handle(CreateIntegrationEventCommand request, CancellationToken cancellationToken)
    {
        var country = new Country { Code = "test11", Name = "test11", Culture = "test11", NameNative = "test11" };
        this.countryBllFactory.Create()
            .Save(country);

        this.integrationEventBus.Publish(new TestIntegrationEvent(country.Id));

        return Task.FromResult(Unit.Value);
    }
}
