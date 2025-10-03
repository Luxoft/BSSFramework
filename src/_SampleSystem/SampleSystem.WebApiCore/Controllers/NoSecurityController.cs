using Framework.DomainDriven;
using Framework.DomainDriven.Repository;

using GenericQueryable;

using SecuritySystem;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class NoSecurityController : ControllerBase
{
    private readonly IRepositoryFactory<NoSecurityObject> repositoryFactory;

    public NoSecurityController(IRepositoryFactory<NoSecurityObject> repositoryFactory)
    {
        this.repositoryFactory = repositoryFactory;
    }


    [DBSessionMode(DBSessionMode.Write)]
    [HttpPost]
    public async Task<NoSecurityObjectIdentityDTO> TestFaultSave(CancellationToken cancellationToken = default)
    {
        var repository = this.repositoryFactory.Create(SecurityRule.Edit);

        var obj = new NoSecurityObject();

        await repository.SaveAsync(obj, cancellationToken);

        return obj.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Write)]
    [HttpPost]
    public async Task<NoSecurityObjectIdentityDTO> TestSave(CancellationToken cancellationToken = default)
    {
        var repository = this.repositoryFactory.Create();

        var obj = new NoSecurityObject();

        await repository.SaveAsync(obj, cancellationToken);

        return obj.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost]
    public async Task<List<NoSecurityObjectIdentityDTO>> GetFullList(CancellationToken cancellationToken = default)
    {
        var repository = this.repositoryFactory.Create();

        var result = await repository.GetQueryable().GenericToListAsync(cancellationToken);

        return result.ToIdentityDTOList();
    }
}
