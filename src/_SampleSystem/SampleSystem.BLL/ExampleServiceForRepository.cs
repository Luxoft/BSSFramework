using Anch.GenericQueryable;
using Anch.SecuritySystem;

using Framework.Application.Repository;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Employee;

namespace SampleSystem.BLL;

public class ExampleServiceForRepository(
    IRepositoryFactory<Employee> employeeRepositoryFactory,
    IRepositoryFactory<BusinessUnit> businessUnitRepository)
    : IExampleServiceForRepository
{
    private readonly IRepository<Employee> employeeRepository = employeeRepositoryFactory.Create();

    private readonly IRepository<BusinessUnit> businessUnitRepository = businessUnitRepository.Create(SecurityRule.View);

    public async Task<(List<Employee> Employees, List<BusinessUnit> BusinessUnits)> LoadPair(CancellationToken ct)
    {
        //var employeesFuture = this.employeeRepository.GetQueryable().ToFuture();

        //var businessUnitsFuture = this.businessUnitRepository.GetQueryable().ToFuture();

        //var employees = await employeesFuture.GetEnumerableAsync(ct);

        //var businessUnits = await businessUnitsFuture.GetEnumerableAsync(ct);

        var employees = await this.employeeRepository.GetQueryable().GenericToHashSetAsync(ct);

        var businessUnits = await this.businessUnitRepository.GetQueryable().GenericToHashSetAsync(ct);

        return (employees.ToList(), businessUnits.ToList());
    }
}

