using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore.Integration;
using Framework.Exceptions;
using Microsoft.AspNetCore.Mvc;
using SampleSystem.BLL;
using SampleSystem.Generated.DTO;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.WebApiCore.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class IntegrationController : IntegrationSchemaControllerBase<ISampleSystemBLLContext, EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>
    {
        public IntegrationController(IDateTimeService dateTimeService)
            : base(dateTimeService)
        {
        }

        protected override string IntegrationNamespace => "http://sampleSystem.example.com/integrationEvent";

        protected override void CheckAccess(EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService> eval) =>
            eval.Context.Authorization.CheckAccess(SampleSystemSecurityOperation.SystemIntegration);

        protected override EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService> GetEvaluatedData(IDBSession session, ISampleSystemBLLContext context) =>
            new(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));

        protected override IEnumerable<Type> GetEventDTOTypes()
        {
            foreach (var type in TypeSource.FromSample(typeof(EmployeeSaveEventDTO)).GetTypes().Where(z => typeof(Generated.DTO.EventDTOBase).IsAssignableFrom(z)))
            {
                yield return type;
            }
        }

        protected override IEnumerable<Type> GetAuthEventDTOTypes()
        {
            foreach (var type in TypeSource.FromSample(typeof(PermissionSaveEventDTO)).GetTypes().Where(z => typeof(Framework.Authorization.Generated.DTO.EventDTOBase).IsAssignableFrom(z)))
            {
                yield return type;
            }
        }
    }
}
