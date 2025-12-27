using Framework.DomainDriven.Repository;

using GenericQueryable;

using SecuritySystem;
using SecuritySystem.UserSource;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestSecurityController(
    ICurrentUserSource<Employee> currentUserSource,
    IRepositoryFactory<Employee> employeeRepositoryFactory,
    IRepositoryFactory<BusinessUnit> buRepositoryFactory) : ControllerBase
{
    [HttpGet]
    public User GetCurrentUser()
    {
        return currentUserSource.ToSimple().CurrentUser;
    }

    [HttpGet]
    public EmployeeDto GetCurrentEmployee()
    {
        return new(currentUserSource.CurrentUser);
    }

    [HttpGet]
    public async Task<List<EmployeeDto>> GetSecurityEmployees(CancellationToken cancellationToken)
    {
        var allData = await employeeRepositoryFactory.Create(SecurityRule.Disabled).GetQueryable().GenericToListAsync(cancellationToken);

        var secData = await employeeRepositoryFactory.Create(SecurityRule.View).GetQueryable().GenericToListAsync(cancellationToken);

        return secData.Select(e => new EmployeeDto(e)).ToList();
    }


    [HttpGet]
    public async Task<List<BusinessUnitDto>> GetSecurityBusinessUnits(CancellationToken cancellationToken)
    {
        var allData = await buRepositoryFactory.Create(SecurityRule.Disabled).GetQueryable().GenericToListAsync(cancellationToken);

        var secData = await buRepositoryFactory.Create(SecurityRule.View).GetQueryable().GenericToListAsync(cancellationToken);

        return secData.Select(e => new BusinessUnitDto(e)).ToList();
    }
}

public record EmployeeDto(Guid Id, string Login, bool Active)
{
    public EmployeeDto(Employee employee)
        : this(employee.Id, employee.Login, employee.Active)
    {
    }
}

public record BusinessUnitDto(Guid Id, string Name)
{
    public BusinessUnitDto(BusinessUnit businessUnit)
        : this(businessUnit.Id, businessUnit.Name)
    {
    }
}
