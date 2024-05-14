using Framework.DomainDriven;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]")]
[ApiController]
public class TestRepositoryController : ControllerBase
{
    private readonly IExampleServiceForRepository exampleService;

    public TestRepositoryController(IExampleServiceForRepository exampleService)
    {
        this.exampleService = exampleService;
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost(nameof(LoadPair))]
    public async Task<(List<EmployeeIdentityDTO> Employees, List<BusinessUnitIdentityDTO> BusinessUnits)> LoadPair(CancellationToken cancellationToken = default)
    {
        var pair = await this.exampleService.LoadPair(cancellationToken);

        return (pair.Employees.ToIdentityDTOList(), pair.BusinessUnits.ToIdentityDTOList());
    }
}
