using Framework.DomainDriven;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

using NHibernate.Linq;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TestAsyncController : ControllerBase
{
    private readonly ILocationBLLFactory buFactory;


    private readonly ISampleSystemDTOMappingService mappingService;

    public TestAsyncController(ILocationBLLFactory buFactory, ISampleSystemDTOMappingService mappingService)
    {
        this.buFactory = buFactory;
        this.mappingService = mappingService;
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost(nameof(AsyncGetLocations))]
    public async Task<List<LocationSimpleDTO>> AsyncGetLocations(CancellationToken cancellationToken = default)
    {
        var bll = this.buFactory.Create(BLLSecurityMode.View);

        var list = await bll.GetSecureQueryable().ToListAsync(cancellationToken);

        return list.ToSimpleDTOList(this.mappingService);
    }

    [HttpPost(nameof(AsyncSaveLocation))]
    public async Task<LocationIdentityDTO> AsyncSaveLocation(LocationStrictDTO businessUnitStrictDTO, CancellationToken cancellationToken = default)
    {
        var bll = this.buFactory.Create(BLLSecurityMode.Edit);

        var bu = businessUnitStrictDTO.ToDomainObject(this.mappingService, true);

        bll.Save(bu);

        return bu.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost(nameof(AsyncSaveLocationWithWriteException))]
    public async Task<LocationIdentityDTO> AsyncSaveLocationWithWriteException(LocationStrictDTO businessUnitStrictDTO, CancellationToken cancellationToken = default)
    {
        return await this.AsyncSaveLocation(businessUnitStrictDTO, cancellationToken);
    }

    [HttpGet(nameof(TestDelay))]
    public async Task<int> TestDelay(CancellationToken cancellationToken = default)
    {
        await Task.Delay(new TimeSpan(0, 1, 0), cancellationToken);

        return 123;
    }
}
