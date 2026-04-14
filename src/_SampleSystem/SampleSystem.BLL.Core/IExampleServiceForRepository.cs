using SampleSystem.Domain;
using SampleSystem.Domain.BU;
using SampleSystem.Domain.Employee;

namespace SampleSystem.BLL;

public interface IExampleServiceForRepository
{
    Task<(List<Employee> Employees, List<BusinessUnit> BusinessUnits)> LoadPair(CancellationToken cancellationToken = default);
}
