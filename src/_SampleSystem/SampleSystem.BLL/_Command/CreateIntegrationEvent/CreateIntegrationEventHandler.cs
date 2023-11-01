using Framework.Cap.Abstractions;

using MediatR;

using SampleSystem.BLL.Core.IntegrationEvens;
using SampleSystem.Domain;

namespace SampleSystem.BLL._Command.CreateIntegrationEvent;

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

    public async Task Handle(CreateIntegrationEventCommand request, CancellationToken cancellationToken)
    {
        var country = new Country { Code = "test11", Name = "test11", Culture = "test11", NameNative = "test11" };
        this.countryBllFactory.Create()
            .Save(country);

        await this.integrationEventBus.PublishAsync(new TestIntegrationEvent(country.Id), cancellationToken);
    }
}
