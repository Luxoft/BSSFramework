using Framework.Database;

using Microsoft.AspNetCore.Mvc;
using SampleSystem.AuditDomain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class BusinessUnitAuditController(IAsyncDal<BusinessUnitAudit, Guid> dal) : ControllerBase
{
    [HttpGet]
    public (string Author, long Revision, BusinessUnitIdentityDTO BuIdent) LoadFromCustomAuditMapping(BusinessUnitIdentityDTO bu, long revNumber)
    {
        var auditBu = dal.GetQueryable().Single(aBu => aBu.Identifier.Id == bu.Id && aBu.Identifier.RevNumber == revNumber);

        return (auditBu.Revision.Author, auditBu.Revision.Id, new BusinessUnitIdentityDTO(auditBu.Id));
    }
}
