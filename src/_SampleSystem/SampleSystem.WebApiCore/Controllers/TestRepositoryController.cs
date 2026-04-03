using Framework.Database;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestRepositoryController(IExampleServiceForRepository exampleService) : ControllerBase
{
    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost]
    public async Task<(List<EmployeeIdentityDTO> Employees, List<BusinessUnitIdentityDTO> BusinessUnits)> LoadPair(CancellationToken cancellationToken = default)
    {
        var pair = await exampleService.LoadPair(cancellationToken);

        return (pair.Employees.ToIdentityDTOList(), pair.BusinessUnits.ToIdentityDTOList());
    }
}
