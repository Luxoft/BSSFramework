using Framework.DomainDriven.Repository;
using Framework.GenericQueryable;
using Framework.SecuritySystem;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public class ExampleServiceForRepository(
    IRepositoryFactory<Employee> employeeRepositoryFactory,
    IRepositoryFactory<BusinessUnit> businessUnitRepository)
    : IExampleServiceForRepository
{
    private readonly IRepository<Employee> employeeRepository = employeeRepositoryFactory.Create();

    private readonly IRepository<BusinessUnit> businessUnitRepository = businessUnitRepository.Create(SecurityRule.View);

    public async Task<(List<Employee> Employees, List<BusinessUnit> BusinessUnits)> LoadPair(CancellationToken cancellationToken = default)
    {
        //var employeesFuture = this.employeeRepository.GetQueryable().ToFuture();

        //var businessUnitsFuture = this.businessUnitRepository.GetQueryable().ToFuture();

        //var employees = await employeesFuture.GetEnumerableAsync(cancellationToken);

        //var businessUnits = await businessUnitsFuture.GetEnumerableAsync(cancellationToken);

        var employees = await this.employeeRepository.GetQueryable().ToGenericListAsync(cancellationToken);

        var businessUnits = await this.businessUnitRepository.GetQueryable().ToGenericListAsync(cancellationToken);

        return (employees.ToList(), businessUnits.ToList());
    }
}
