using Anch.GenericQueryable;
using Anch.SecuritySystem.Attributes;

using Framework.Application.Repository;
using Framework.Database;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain.Directories;
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
    public async Task<List<LocationSimpleDTO>> AsyncGetLocations(CancellationToken ct)
    {
        var list = await locationViewRepository.GetQueryable().GenericToListAsync(ct);

        return list.ToSimpleDTOList(mappingService);
    }

    [HttpPost]
    public async Task<LocationIdentityDTO> AsyncSaveLocation(LocationStrictDTO locationStrictDTO, CancellationToken ct)
    {
        var location = locationStrictDTO.ToDomainObject(mappingService, true);

        await locationEditRepository.SaveAsync(location, ct);

        return location.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost]
    public async Task<LocationIdentityDTO> AsyncSaveLocationWithWriteException(LocationStrictDTO locationStrictDTO, CancellationToken ct) => await this.AsyncSaveLocation(locationStrictDTO, ct);

    [HttpGet]
    public async Task<int> TestDelay(CancellationToken ct)
    {
        await Task.Delay(new TimeSpan(0, 1, 0), ct);

        return 123;
    }
}

