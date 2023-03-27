using SampleSystem.Domain;

namespace SampleSystem.BLL;

public interface IExampleServiceForRepository
{
    Task<(List<Employee> Employees, List<BusinessUnit> BusinessUnits)> LoadPair(CancellationToken cancellationToken = default);
}
