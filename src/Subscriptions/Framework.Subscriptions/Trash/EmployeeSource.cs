using Framework.Subscriptions.Domain;

using SecuritySystem.Attributes;

namespace Framework.Subscriptions;

public class EmployeeSource<TEmployee>([DisabledSecurity] IRepository<TEmployee> employeeRepository) : IEmployeeSource
    where TEmployee : class, IEmployee
{
    public IQueryable<IEmployee> GetQueryable() => employeeRepository.GetQueryable();
}
