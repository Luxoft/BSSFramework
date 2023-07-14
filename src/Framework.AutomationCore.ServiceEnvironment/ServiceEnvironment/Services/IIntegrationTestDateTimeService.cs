using Framework.DomainDriven;

namespace Automation.ServiceEnvironment.Services;

public interface IIntegrationTestDateTimeService : IDateTimeService
{
    public void SetCurrentDateTime(DateTime dateTime);

    public void Reset();
}
