using Framework.DomainDriven.Repository;
using Framework.Notification;
using Framework.SecuritySystem;

namespace Framework.Configuration.BLL;

public class EmployeeSource<TEmployee> : IEmployeeSource
    where TEmployee : class, IEmployee
{
    private readonly IRepository<TEmployee> employeeRepository;

    public EmployeeSource([DisabledSecurity] IRepository<TEmployee> employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }

    public IQueryable<IEmployee> GetQueryable() => this.employeeRepository.GetQueryable();
}
