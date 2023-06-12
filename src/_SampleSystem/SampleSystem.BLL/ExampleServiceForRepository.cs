using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using NHibernate.Linq;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public class ExampleServiceForRepository : IExampleServiceForRepository
{
    private readonly IRepository<Employee> employeeRepository;

    private readonly IRepository<BusinessUnit> businessUnitRepository;

    public ExampleServiceForRepository(
            IDefaultRepositoryFactory<Employee> employeeRepositoryFactory,
            IDefaultRepositoryFactory<BusinessUnit, SampleSystemSecurityOperationCode> businessUnitRepository)
    {
        this.employeeRepository = employeeRepositoryFactory.Create(BLLSecurityMode.Disabled);

        this.businessUnitRepository = businessUnitRepository.Create(SampleSystemSecurityOperationCode.BusinessUnitView);
    }

    public async Task<(List<Employee> Employees, List<BusinessUnit> BusinessUnits)> LoadPair(CancellationToken cancellationToken = default)
    {
        var employeesFuture = this.employeeRepository.GetQueryable().ToFuture();

        var businessUnitsFuture = this.businessUnitRepository.GetQueryable().ToFuture();

        var employees = await employeesFuture.GetEnumerableAsync(cancellationToken);

        var businessUnits = await businessUnitsFuture.GetEnumerableAsync(cancellationToken);

        return (employees.ToList(), businessUnits.ToList());
    }
}
