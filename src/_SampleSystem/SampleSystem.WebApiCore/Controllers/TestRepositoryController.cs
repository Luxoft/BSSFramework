using Framework.DomainDriven;
using Framework.DomainDriven.Repository;

using Microsoft.AspNetCore.Mvc;

using NHibernate;
using NHibernate.Proxy;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
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
    public async Task<(List<EmployeeIdentityDTO> Employees, List<BusinessUnitIdentityDTO> BusinessUnits)> LoadPair(
        CancellationToken cancellationToken = default)
    {
        var pair = await this.exampleService.LoadPair(cancellationToken);

        return (pair.Employees.ToIdentityDTOList(), pair.BusinessUnits.ToIdentityDTOList());
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost(nameof(TestStaticAbstract))]
    public void TestStaticAbstract(
        [FromServices] IRepositoryFactory<BusinessUnit, Guid, SampleSystemSecurityOperationCode> repositoryFactory,
        CancellationToken cancellationToken = default)
    {
        var list = repositoryFactory.Create()
                                    .GetQueryable()
                                    .ToList();

        foreach (var businessUnit in list)
        {
            Console.WriteLine(businessUnit.BusinessUnitType.Id);
        }
    }
}
