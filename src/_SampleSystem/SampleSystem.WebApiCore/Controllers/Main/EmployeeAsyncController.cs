using Anch.Core.Auth;

using Framework.Application.Repository;
using Framework.Database;

using Anch.GenericQueryable;

using Microsoft.AspNetCore.Mvc;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class EmployeeAsyncController(
    IRepositoryFactory<Employee> employeeRepositoryFactory,
    ICurrentUser currentUser,
    ISampleSystemDTOMappingService mappingService)
    : ControllerBase
{
    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost]
    public async Task<EmployeeSimpleDTO> GetCurrentEmployee(CancellationToken cancellationToken)
    {
        var userName = currentUser.Name;

        var repository = employeeRepositoryFactory.Create();

        var employees = await repository
                              .GetQueryable()
                              .Where(employee => employee.Login == userName)
                              .GenericToListAsync(cancellationToken);

        return employees.Single().ToSimpleDTO(mappingService);
    }
}
