using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore.Integration;
using Framework.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Generated.DTO;

namespace WorkflowSampleSystem.WebApiCore.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class IntegrationController : IntegrationSchemaControllerBase<IServiceEnvironment<IWorkflowSampleSystemBLLContext>,
        IWorkflowSampleSystemBLLContext, EvaluatedData<IWorkflowSampleSystemBLLContext, IWorkflowSampleSystemDTOMappingService>>
    {
        public IntegrationController(IServiceEnvironment<IWorkflowSampleSystemBLLContext> environment, IExceptionProcessor exceptionProcessor)
            : base(environment, exceptionProcessor)
        {
        }

        protected override string IntegrationNamespace => "http://WorkflowSampleSystem.luxoft.com/integrationEvent";

        protected override void CheckAccess(EvaluatedData<IWorkflowSampleSystemBLLContext, IWorkflowSampleSystemDTOMappingService> eval) =>
            eval.Context.Authorization.CheckAccess(WorkflowSampleSystemSecurityOperation.SystemIntegration);

        protected override EvaluatedData<IWorkflowSampleSystemBLLContext, IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(IDBSession session, IWorkflowSampleSystemBLLContext context) =>
            new EvaluatedData<IWorkflowSampleSystemBLLContext, IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));

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
