using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

using NHibernate.Linq;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class NoSecurityController : ControllerBase
{
    private readonly IDefaultRepositoryFactory<NoSecurityObject> repositoryFactory;

    public NoSecurityController(IDefaultRepositoryFactory<NoSecurityObject> repositoryFactory)
    {
        this.repositoryFactory = repositoryFactory;
    }


    [DBSessionMode(DBSessionMode.Write)]
    [HttpPost(nameof(TestFaultSave))]
    public async Task<NoSecurityObjectIdentityDTO> TestFaultSave(CancellationToken cancellationToken = default)
    {
        var repository = this.repositoryFactory.Create(BLLSecurityMode.Edit);

        var obj = new NoSecurityObject();

        await repository.SaveAsync(obj, cancellationToken);

        return obj.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Write)]
    [HttpPost(nameof(TestSave))]
    public async Task<NoSecurityObjectIdentityDTO> TestSave(CancellationToken cancellationToken = default)
    {
        var repository = this.repositoryFactory.Create(BLLSecurityMode.Disabled);

        var obj = new NoSecurityObject();

        await repository.SaveAsync(obj, cancellationToken);

        return obj.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost(nameof(GetFullList))]
    public async Task<List<NoSecurityObjectIdentityDTO>> GetFullList(CancellationToken cancellationToken = default)
    {
        var repository = this.repositoryFactory.Create(BLLSecurityMode.Disabled);

        var result = await repository.GetQueryable().ToListAsync(cancellationToken);

        return result.ToIdentityDTOList();
    }
}
