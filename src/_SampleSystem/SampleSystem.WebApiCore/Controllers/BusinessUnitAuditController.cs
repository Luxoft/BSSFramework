using Framework.DomainDriven.Repository;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.AuditDomain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BusinessUnitAuditController : ControllerBase
{
    private readonly IRepository<BusinessUnitAudit, Guid> repository;

    public BusinessUnitAuditController(IRepository<BusinessUnitAudit, Guid> repository)
    {
        this.repository = repository;
    }

    [HttpGet(nameof(LoadFromCustomAuditMapping))]
    public async Task<BusinessUnitIdentityDTO> LoadFromCustomAuditMapping(BusinessUnitIdentityDTO bu)
    {
        var reloaded = await this.repository.LoadAsync(bu.Id);

        return new BusinessUnitIdentityDTO(reloaded.Id);
    }
}
