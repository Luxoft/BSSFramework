using Framework.Application.Repository;
using Framework.Notification;
using Framework.Notification.Domain;

using SecuritySystem.Attributes;

namespace Framework.Configuration.BLL;

public class EmployeeSource<TEmployee>([DisabledSecurity] IRepository<TEmployee> employeeRepository) : IEmployeeSource
    where TEmployee : class, IEmployee
{
    public IQueryable<IEmployee> GetQueryable() => employeeRepository.GetQueryable();
}
