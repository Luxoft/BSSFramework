using System;

using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.WebApi.Utils.SL;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace Framework.Authorization.WebApi
{
    [SLJsonCompatibilityActionFilter]
    [TypeFilter(typeof(SLJsonCompatibilityResourceFilter))]
    [ApiController]
    [Route("AuthSLJsonFacade.svc")]
    [ApiExplorerSettings(IgnoreApi = true)]
    //[Authorize(nameof(AuthenticationSchemes.NTLM))]
    public abstract partial class AuthSLJsonController : ApiControllerBase<IServiceEnvironment, IAuthorizationBLLContext, EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>
    {
        protected AuthSLJsonController(IServiceEnvironment environment, IExceptionProcessor exceptionProcessor)
            : base(environment, exceptionProcessor)
        {
        }

        protected override EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService> GetEvaluatedData(IDBSession session, IAuthorizationBLLContext context)
        {
            return new EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>(session, context, new AuthorizationServerPrimitiveDTOMappingService(context));
        }
    }
}
