using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Configuration.WebApi;

public partial class ConfigSLJsonController
{
    [HttpPost]
    public DomainTypeSimpleDTO GetSimpleDomainTypeByPath(string path)
    {
        return this.Evaluate(DBSessionMode.Read, data =>
                                                         data.Context.Logics.DomainType // without security?
                                                             .GetByPath(path)
                                                             .ToSimpleDTO(data.MappingService));
    }

    [HttpPost]
    public void ForceDomainTypeEvent(DomainTypeEventModelStrictDTO domainTypeEventModel)
    {
        if (domainTypeEventModel == null) throw new ArgumentNullException(nameof(domainTypeEventModel));

        this.Evaluate(DBSessionMode.Write, evaluateData =>
                                           {
                                               evaluateData.Context.Authorization.SecuritySystem.CheckAccess(SecurityRole.Administrator);

                                               evaluateData.Context.Logics.DomainType.ForceEvent(domainTypeEventModel.ToDomainObject(evaluateData.MappingService));
                                           });
    }
}
