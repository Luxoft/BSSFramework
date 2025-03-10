﻿using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.GenericQueryable;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
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

    [HttpPost]
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

    [HttpPost]
    public async Task<List<NoSecurityObjectSimpleDTO>> GetFullList(CancellationToken cancellationToken = default)
    {
        return await this.dbSessionEvaluator.EvaluateAsync(
                   DBSessionMode.Read,
                   async serviceProvider =>
                   {
                       var repositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory<NoSecurityObject>>();

                       var mappingService = serviceProvider.GetRequiredService<ISampleSystemDTOMappingService>();
                       var result = await repositoryFactory.Create().GetQueryable().GenericToListAsync(cancellationToken);

                       return result.ToSimpleDTOList(mappingService);
                   });
    }
}
