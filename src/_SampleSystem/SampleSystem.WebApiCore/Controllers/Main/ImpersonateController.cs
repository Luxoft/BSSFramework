using Framework.Application;
using Framework.Application.Repository;
using Framework.Database;

using Anch.GenericQueryable;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class ImpersonateController(
    IServiceEvaluator<IRepositoryFactory<NoSecurityObject>> serviceEvaluator,
    IDBSessionEvaluator dbSessionEvaluator)
    : ControllerBase
{
    [HttpPost]
    public async Task<NoSecurityObjectIdentityDTO> TestSave(string impersonateLogin, CancellationToken cancellationToken = default) =>
        await serviceEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            impersonateLogin,
            async repositoryFactory =>
            {
                var obj = new NoSecurityObject();

                await repositoryFactory.Create().SaveAsync(obj, cancellationToken);

                return obj.ToIdentityDTO();
            });

    [HttpPost]
    public async Task<List<NoSecurityObjectSimpleDTO>> GetFullList(CancellationToken cancellationToken = default) =>
        await dbSessionEvaluator.EvaluateAsync(
            DBSessionMode.Read,
            async serviceProvider =>
            {
                var repositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory<NoSecurityObject>>();

                var mappingService = serviceProvider.GetRequiredService<ISampleSystemDTOMappingService>();
                var result = await repositoryFactory.Create().GetQueryable().GenericToListAsync(cancellationToken);

                return result.ToSimpleDTOList(mappingService);
            });
}
