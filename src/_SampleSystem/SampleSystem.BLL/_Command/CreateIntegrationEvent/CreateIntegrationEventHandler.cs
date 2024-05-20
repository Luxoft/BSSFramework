using Bss.Platform.Events.Abstractions;

using MediatR;

using SampleSystem.BLL._Command.ProcessIntegrationEvent;
using SampleSystem.Domain;

namespace SampleSystem.BLL._Command.CreateIntegrationEvent;

public class CreateIntegrationEventHandler(IIntegrationEventPublisher eventPublisher, ICountryBLLFactory countryBllFactory)
    : IRequestHandler<CreateIntegrationEventCommand>
{
    public async Task Handle(CreateIntegrationEventCommand request, CancellationToken cancellationToken)
    {
        var country = new Country { Code = "test11", Name = "test11", Culture = "test11", NameNative = "test11" };
        countryBllFactory.Create().Save(country);

        await eventPublisher.PublishAsync(new TestIntegrationEvent(country.Id), cancellationToken);
    }
}
