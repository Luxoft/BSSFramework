using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.WebApi.Utils.SL;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi
{
    [SLJsonCompatibilityActionFilter]
    [TypeFilter(typeof(SLJsonCompatibilityResourceFilter))]
    [ApiController]
    [Route("AuthSLJsonFacade.svc")]
    [ApiExplorerSettings(IgnoreApi = true)]
    //[Authorize(nameof(AuthenticationSchemes.NTLM))]
    public abstract partial class AuthSLJsonController : ApiControllerBase<IAuthorizationBLLContext, EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>
    {
    }
}
