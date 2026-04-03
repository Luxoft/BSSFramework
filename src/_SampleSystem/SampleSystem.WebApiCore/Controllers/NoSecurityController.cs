using Framework.Application.Repository;
using Framework.Database;

using GenericQueryable;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

using SecuritySystem;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class NoSecurityController(IRepositoryFactory<NoSecurityObject> repositoryFactory) : ControllerBase
{
    [DBSessionMode(DBSessionMode.Write)]
    [HttpPost]
    public async Task<NoSecurityObjectIdentityDTO> TestFaultSave(CancellationToken cancellationToken = default)
    {
        var repository = repositoryFactory.Create(SecurityRule.Edit);

        var obj = new NoSecurityObject();

        await repository.SaveAsync(obj, cancellationToken);

        return obj.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Write)]
    [HttpPost]
    public async Task<NoSecurityObjectIdentityDTO> TestSave(CancellationToken cancellationToken = default)
    {
        var repository = repositoryFactory.Create();

        var obj = new NoSecurityObject();

        await repository.SaveAsync(obj, cancellationToken);

        return obj.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost]
    public async Task<List<NoSecurityObjectIdentityDTO>> GetFullList(CancellationToken cancellationToken = default)
    {
        var repository = repositoryFactory.Create();

        var result = await repository.GetQueryable().GenericToListAsync(cancellationToken);

        return result.ToIdentityDTOList();
    }
}
