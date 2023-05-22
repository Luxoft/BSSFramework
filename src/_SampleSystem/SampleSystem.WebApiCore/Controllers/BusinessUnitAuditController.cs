using Framework.DomainDriven.Repository;
using Framework.Security;
using Framework.SecuritySystem;

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

    public BusinessUnitAuditController(IRepositoryFactory<BusinessUnitAudit, Guid, SecurityOperationCode> repositoryFactory)
    {
        this.repository = repositoryFactory.Create(BLLSecurityMode.Disabled);
    }

    [HttpGet(nameof(LoadFromCustomAuditMapping))]
    public async Task<(string Author, long Revision, BusinessUnitIdentityDTO BuIdent)> LoadFromCustomAuditMapping(BusinessUnitIdentityDTO bu)
    {
        var auditBu = await this.repository.LoadAsync(bu.Id);

        return (auditBu.Revision.Author, auditBu.Revision.Id, new BusinessUnitIdentityDTO(auditBu.Id));
    }
}
