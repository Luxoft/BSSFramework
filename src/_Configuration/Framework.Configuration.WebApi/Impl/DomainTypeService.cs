using System;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

namespace Framework.Configuration.WebApi
{
    public partial class ConfigSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetSimpleDomainTypeByPath))]
        public DomainTypeSimpleDTO GetSimpleDomainTypeByPath(string path)
        {
            return this.Evaluate(DBSessionMode.Read, data =>
                data.Context.Logics.DomainType // without security?
                                   .GetByPath(path)
                                   .ToSimpleDTO(data.MappingService));
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(ForceDomainTypeEvent))]
        public void ForceDomainTypeEvent(DomainTypeEventModelStrictDTO domainTypeEventModel)
        {
            if (domainTypeEventModel == null) throw new ArgumentNullException(nameof(domainTypeEventModel));

            this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                evaluateData.Context.Authorization.CheckAccess(ConfigurationSecurityOperation.ForceDomainTypeEvent);

                evaluateData.Context.Logics.DomainType.ForceEvent(domainTypeEventModel.ToDomainObject(evaluateData.MappingService));
            });
        }
    }
}
