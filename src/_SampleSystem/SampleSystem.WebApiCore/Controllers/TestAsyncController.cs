using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TestAsyncController : ControllerBase
{
    private readonly ILocationBLLFactory buFactory;

    private readonly IDBSession session;

    private readonly ISampleSystemDTOMappingService mappingService;

    public TestAsyncController(ILocationBLLFactory buFactory, IDBSession session, ISampleSystemDTOMappingService mappingService)
    {
        this.buFactory = buFactory;
        this.session = session;
        this.mappingService = mappingService;
    }

    [HttpPost(nameof(AsyncGetLocations))]
    public async Task<List<LocationSimpleDTO>> AsyncGetLocations(CancellationToken cancellationToken = default)
    {
        this.session.AsReadOnly();

        var bll = this.buFactory.Create(BLLSecurityMode.View);

        return bll.GetFullList().ToSimpleDTOList(this.mappingService);
    }


    [HttpPost(nameof(AsyncSaveLocation))]
    public async Task<LocationIdentityDTO> AsyncSaveLocation(LocationStrictDTO businessUnitStrictDTO, CancellationToken cancellationToken = default)
    {
        var bll = this.buFactory.Create(BLLSecurityMode.Edit);

        var bu = businessUnitStrictDTO.ToDomainObject(this.mappingService, true);

        bll.Save(bu);

        return bu.ToIdentityDTO();
    }
}
