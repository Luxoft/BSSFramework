using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;
using Framework.Infrastructure;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

[ApiController]
public abstract partial class AuthMainController : ApiControllerBase<IAuthorizationBLLContext, IAuthorizationDTOMappingService>;
