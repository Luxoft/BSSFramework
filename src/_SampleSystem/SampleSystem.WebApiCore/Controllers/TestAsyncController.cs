using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

using NHibernate.Linq;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestAsyncController(
    [ViewSecurity] IRepository<Location> locationViewRepository,
    [EditSecurity] IRepository<Location> locationEditRepository,
    ISampleSystemDTOMappingService mappingService)
    : ControllerBase
{
    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost]
    public async Task<List<LocationSimpleDTO>> AsyncGetLocations(CancellationToken cancellationToken = default)
    {
        var list = await locationViewRepository.GetQueryable().ToListAsync(cancellationToken);

        return list.ToSimpleDTOList(mappingService);
    }

    [HttpPost]
    public async Task<LocationIdentityDTO> AsyncSaveLocation(LocationStrictDTO locationStrictDTO, CancellationToken cancellationToken = default)
    {
        var location = locationStrictDTO.ToDomainObject(mappingService, true);

        await locationEditRepository.SaveAsync(location, cancellationToken);

        return location.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost]
    public async Task<LocationIdentityDTO> AsyncSaveLocationWithWriteException(LocationStrictDTO locationStrictDTO, CancellationToken cancellationToken = default)
    {
        return await this.AsyncSaveLocation(locationStrictDTO, cancellationToken);
    }

    [HttpGet]
    public async Task<int> TestDelay(CancellationToken cancellationToken = default)
    {
        await Task.Delay(new TimeSpan(0, 1, 0), cancellationToken);

        return 123;
    }
}
