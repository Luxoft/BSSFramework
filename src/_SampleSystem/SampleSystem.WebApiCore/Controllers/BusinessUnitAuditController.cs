using Framework.DomainDriven;
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
    private readonly IAsyncDal<BusinessUnitAudit, Guid> dal;

    public BusinessUnitAuditController(IAsyncDal<BusinessUnitAudit, Guid> dal)
    {
        this.dal = dal;
    }

    [HttpGet(nameof(LoadFromCustomAuditMapping))]
    public (string Author, long Revision, BusinessUnitIdentityDTO BuIdent) LoadFromCustomAuditMapping(BusinessUnitIdentityDTO bu, long revNumber)
    {
        var auditBu = this.dal.GetQueryable().Single(aBu => aBu.Identifier.Id == bu.Id && aBu.Identifier.RevNumber == revNumber);

        return (auditBu.Revision.Author, auditBu.Revision.Id, new BusinessUnitIdentityDTO(auditBu.Id));
    }
}
