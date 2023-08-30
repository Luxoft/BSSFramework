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
    public Task<LocationIdentityDTO> AsyncSaveLocation(LocationStrictDTO businessUnitStrictDTO, CancellationToken cancellationToken = default)
    {
        var bll = this.buFactory.Create(BLLSecurityMode.Edit);

        var bu = businessUnitStrictDTO.ToDomainObject(this.mappingService, true);

        bll.Save(bu);

        return Task.FromResult(bu.ToIdentityDTO());
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost(nameof(AsyncSaveLocationWithWriteException))]
    public Task<LocationIdentityDTO> AsyncSaveLocationWithWriteException(LocationStrictDTO businessUnitStrictDTO, CancellationToken cancellationToken = default)
    {
        return this.AsyncSaveLocation(businessUnitStrictDTO, cancellationToken);
    }
}
