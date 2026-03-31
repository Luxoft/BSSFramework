using Framework.Configuration.Generated.DTO;
using Framework.Database;

using SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Configuration.WebApi;

public partial class ConfigMainController
{
    [HttpPost]
    public DomainTypeSimpleDTO GetSimpleDomainTypeByPath(string path)
    {
        return this.Evaluate(
            DBSessionMode.Read,
            data =>
                data.Context.Logics.DomainTypeFactory.Create(SecurityRole.Administrator)
                    .GetByPath(path)
                    .ToSimpleDTO(data.MappingService));
    }

    [HttpPost]
    public void ForceDomainTypeEvent(DomainTypeEventModelStrictDTO domainTypeEventModel)
    {
        if (domainTypeEventModel == null) throw new ArgumentNullException(nameof(domainTypeEventModel));

        this.Evaluate(
            DBSessionMode.Write,
            evaluateData =>
            {
                evaluateData.Context.Authorization.SecuritySystem.CheckAccessAsync(SecurityRole.Administrator, this.HttpContext.RequestAborted).GetAwaiter().GetResult();

                evaluateData.Context.Logics.DomainType.ForceEvent(domainTypeEventModel.ToDomainObject(evaluateData.MappingService));
            });
    }
}
