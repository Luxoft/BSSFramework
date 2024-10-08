﻿using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven.WebApiNetCore;
using Framework.WebApi.Utils.SL;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

[SLJsonCompatibilityActionFilter]
[TypeFilter(typeof(SLJsonCompatibilityResourceFilter))]
[ApiController]
[Route("api/AuthSLJsonFacade.svc/[action]")]
[ApiExplorerSettings(IgnoreApi = true)]
//[Authorize(nameof(AuthenticationSchemes.NTLM))]
public abstract partial class AuthSLJsonController : ApiControllerBase<IAuthorizationBLLContext, IAuthorizationDTOMappingService>
{
}
