using Anch.SecuritySystem;

using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;
using Framework.Infrastructure.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.WebApi;

public partial class ConfigMainController
{
    [HttpPost]
    public async Task ForceDomainTypeEvent(DomainTypeEventModelStrictDTO domainTypeEventModel, CancellationToken ct)
    {
        if (domainTypeEventModel is null) throw new ArgumentNullException(nameof(domainTypeEventModel));

        var evaluateData = this.HttpContext.RequestServices.GetRequiredService<EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>();

        await evaluateData.Context.Authorization.SecuritySystem.CheckAccessAsync(SecurityRole.Administrator, ct);

        await evaluateData.Context.Logics.DomainType.ForceEventAsync(domainTypeEventModel.ToDomainObject(evaluateData.MappingService), ct);
    }
}
