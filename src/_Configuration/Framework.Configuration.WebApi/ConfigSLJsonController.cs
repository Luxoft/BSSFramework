using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven.WebApiNetCore;
using Framework.WebApi.Utils.SL;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Configuration.WebApi;

[SLJsonCompatibilityActionFilter]
[TypeFilter(typeof(SLJsonCompatibilityResourceFilter))]
[ApiController]
[Route("api/ConfigSLJsonFacade.svc/[action]")]
[ApiExplorerSettings(IgnoreApi = true)]
public abstract partial class ConfigSLJsonController : ApiControllerBase<IConfigurationBLLContext, IConfigurationDTOMappingService>
{
}
