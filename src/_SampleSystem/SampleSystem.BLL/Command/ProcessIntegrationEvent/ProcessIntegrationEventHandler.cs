using MediatR;

namespace SampleSystem.BLL.Command.ProcessIntegrationEvent;

public class ProcessIntegrationEventHandler(ICountryBLLFactory countryBllFactory) : INotificationHandler<TestIntegrationEvent>
{
    public Task Handle(TestIntegrationEvent request, CancellationToken ct)
    {
        var countryBll = countryBllFactory.Create();
        var c = countryBll.GetById(request.CountryId);
        if (c is not null) countryBll.Remove(c);

        return Task.CompletedTask;
    }
}

