using Anch.GenericQueryable;
using Anch.SecuritySystem;

using Framework.Application.Repository;
using Framework.Database;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class NoSecurityController(IRepositoryFactory<NoSecurityObject> repositoryFactory) : ControllerBase
{
    [DBSessionMode(DBSessionMode.Write)]
    [HttpPost]
    public async Task<NoSecurityObjectIdentityDTO> TestFaultSave(CancellationToken ct)
    {
        var repository = repositoryFactory.Create(SecurityRule.Edit);

        var obj = new NoSecurityObject();

        await repository.SaveAsync(obj, ct);

        return obj.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Write)]
    [HttpPost]
    public async Task<NoSecurityObjectIdentityDTO> TestSave(CancellationToken ct)
    {
        var repository = repositoryFactory.Create();

        var obj = new NoSecurityObject();

        await repository.SaveAsync(obj, ct);

        return obj.ToIdentityDTO();
    }

    [DBSessionMode(DBSessionMode.Read)]
    [HttpPost]
    public async Task<List<NoSecurityObjectIdentityDTO>> GetFullList(CancellationToken ct)
    {
        var repository = repositoryFactory.Create();

        var result = await repository.GetQueryable().GenericToListAsync(ct);

        return result.ToIdentityDTOList();
    }
}

