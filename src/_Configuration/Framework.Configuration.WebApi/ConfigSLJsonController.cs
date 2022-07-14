using System;

using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.WebApi.Utils.SL;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace Framework.Configuration.WebApi
{
    [SLJsonCompatibilityActionFilter]
    [TypeFilter(typeof(SLJsonCompatibilityResourceFilter))]
    [ApiController]
    [Route("ConfigSLJsonFacade.svc")]
    [ApiExplorerSettings(IgnoreApi = true)]
    //[Authorize(nameof(AuthenticationSchemes.NTLM))]
    public abstract partial class ConfigSLJsonController : ApiControllerBase<IServiceEnvironment, IConfigurationBLLContext, EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>
    {
        protected ConfigSLJsonController(IServiceEnvironment environment, IExceptionProcessor exceptionProcessor)
            : base(environment, exceptionProcessor)
        {
        }

        protected override EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService> GetEvaluatedData(IDBSession session, IConfigurationBLLContext context)
        {
            return new EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>(session, context, new ConfigurationServerPrimitiveDTOMappingService(context));
        }
    }
}
