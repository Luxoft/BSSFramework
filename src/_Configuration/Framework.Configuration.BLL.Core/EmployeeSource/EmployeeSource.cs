using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configuration.BLL;

public class EmployeeSource<TEmployee> : IEmployeeSource
    where TEmployee : class, IEmployee
{
    private readonly IRepository<TEmployee> employeeRepository;

    public EmployeeSource([FromKeyedServices(BLLSecurityMode.Disabled)] IRepository<TEmployee> employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }

    public IQueryable<IEmployee> GetQueryable() => this.employeeRepository.GetQueryable();
}
