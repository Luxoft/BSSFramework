using Framework.DomainDriven;
using Framework.DomainDriven.Repository;

using Microsoft.AspNetCore.Mvc;

using NHibernate.Linq;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ImpersonateController : ControllerBase
{
    private readonly IServiceEvaluator<IRepositoryFactory<NoSecurityObject>> serviceEvaluator;

    private readonly IDBSessionEvaluator dbSessionEvaluator;

    public ImpersonateController(
        IServiceEvaluator<IRepositoryFactory<NoSecurityObject>> serviceEvaluator,
        IDBSessionEvaluator dbSessionEvaluator)
    {
        this.serviceEvaluator = serviceEvaluator;
        this.dbSessionEvaluator = dbSessionEvaluator;
    }

    [HttpPost(nameof(TestSave))]
    public async Task<NoSecurityObjectIdentityDTO> TestSave(string impersonateLogin, CancellationToken cancellationToken = default)
    {
        return await this.serviceEvaluator.EvaluateAsync(
                   DBSessionMode.Write,
                   impersonateLogin,
                   async repositoryFactory =>
                   {
                       var obj = new NoSecurityObject();

                       await repositoryFactory.Create().SaveAsync(obj, cancellationToken);

                       return obj.ToIdentityDTO();
                   });
    }

    [HttpPost(nameof(GetFullList))]
    public async Task<List<NoSecurityObjectSimpleDTO>> GetFullList(CancellationToken cancellationToken = default)
    {
        return await this.dbSessionEvaluator.EvaluateAsync(
                   DBSessionMode.Read,
                   async serviceProvider =>
                   {
                       var repositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory<NoSecurityObject>>();

                       var mappingService = serviceProvider.GetRequiredService<ISampleSystemDTOMappingService>();
                       var result = await repositoryFactory.Create().GetQueryable().ToListAsync(cancellationToken);

                       return result.ToSimpleDTOList(mappingService);
                   });
    }
}
