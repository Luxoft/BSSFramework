using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.GenericQueryable;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class EmployeeAsyncController : ControllerBase
{
    private readonly IRepositoryFactory<Employee> employeeRepositoryFactory;

    private readonly IUserAuthenticationService userAuthenticationService;

    private readonly ISampleSystemDTOMappingService mappingService;

    public EmployeeAsyncController(
            IRepositoryFactory<Employee> employeeRepositoryFactory,
            IUserAuthenticationService userAuthenticationService,
            ISampleSystemDTOMappingService mappingService)
    {
        this.employeeRepositoryFactory = employeeRepositoryFactory;
        this.userAuthenticationService = userAuthenticationService;
        this.mappingService = mappingService;
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost]
    public async Task<EmployeeSimpleDTO> GetCurrentEmployee(CancellationToken cancellationToken)
    {
        var userName = this.userAuthenticationService.GetUserName();

        var repository = this.employeeRepositoryFactory.Create();

        var employees = await repository
                              .GetQueryable()
                              .Where(employee => employee.Login == userName)
                              .GenericToListAsync(cancellationToken);

        return employees.Single().ToSimpleDTO(this.mappingService);
    }
}
