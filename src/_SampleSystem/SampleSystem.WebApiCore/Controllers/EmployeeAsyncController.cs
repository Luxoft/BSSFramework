using Framework.DomainDriven;
using Framework.DomainDriven.Repository;

using GenericQueryable;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

using SecuritySystem.Services;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class EmployeeAsyncController(
    IRepositoryFactory<Employee> employeeRepositoryFactory,
    IRawUserAuthenticationService userAuthenticationService,
    ISampleSystemDTOMappingService mappingService)
    : ControllerBase
{
    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost]
    public async Task<EmployeeSimpleDTO> GetCurrentEmployee(CancellationToken cancellationToken)
    {
        var userName = userAuthenticationService.GetUserName();

        var repository = employeeRepositoryFactory.Create();

        var employees = await repository
                              .GetQueryable()
                              .Where(employee => employee.Login == userName)
                              .GenericToListAsync(cancellationToken);

        return employees.Single().ToSimpleDTO(mappingService);
    }
}
