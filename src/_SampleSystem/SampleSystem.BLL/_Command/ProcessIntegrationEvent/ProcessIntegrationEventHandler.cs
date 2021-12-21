using System.Threading;
using System.Threading.Tasks;

using MediatR;

using SampleSystem.BLL.Core.IntegrationEvens;

namespace SampleSystem.BLL._Command.ProcessIntegrationEvent;

public class ProcessIntegrationEventHandler : IRequestHandler<TestIntegrationEvent>
{
    private readonly ICountryBLLFactory countryBllFactory;

    public ProcessIntegrationEventHandler(ICountryBLLFactory countryBllFactory) => this.countryBllFactory = countryBllFactory;

    public Task<Unit> Handle(TestIntegrationEvent request, CancellationToken cancellationToken)
    {
        var countryBll = this.countryBllFactory.Create();
        var c = countryBll.GetById(request.CountryId);
        if (c != null)
        {
            countryBll.Remove(c);
        }

        return Task.FromResult(Unit.Value);
    }
}
