using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using NHibernate.Linq;

namespace SampleSystem.WebApiCore.Controllers.Main;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
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
    [HttpPost(nameof(GetCurrentEmployee))]
    public async Task<EmployeeSimpleDTO> GetCurrentEmployee(CancellationToken cancellationToken)
    {
        var userName = this.userAuthenticationService.GetUserName();

        var repository = this.employeeRepositoryFactory.Create(BLLSecurityMode.Disabled);

        var employees = await repository
                              .GetQueryable()
                              .Where(employee => employee.Login == userName)
                              .ToFuture()
                              .GetEnumerableAsync(cancellationToken);

        return employees.Single().ToSimpleDTO(this.mappingService);
    }
}
