using MediatR;

using SampleSystem.BLL.Core.IntegrationEvens;

namespace SampleSystem.BLL._Command.ProcessIntegrationEvent;

public class ProcessIntegrationEventHandler : IRequestHandler<TestIntegrationEvent>
{
    private readonly ICountryBLLFactory countryBllFactory;

    public ProcessIntegrationEventHandler(ICountryBLLFactory countryBllFactory) => this.countryBllFactory = countryBllFactory;

    public async Task Handle(TestIntegrationEvent request, CancellationToken cancellationToken)
    {
        var countryBll = this.countryBllFactory.Create();
        var c = countryBll.GetById(request.CountryId);
        if (c != null)
        {
            countryBll.Remove(c);
        }
    }
}
