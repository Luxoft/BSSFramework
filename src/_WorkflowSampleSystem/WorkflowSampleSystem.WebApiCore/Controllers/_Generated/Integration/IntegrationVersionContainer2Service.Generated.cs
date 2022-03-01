namespace WorkflowSampleSystem.WebApiCore.Controllers.Integration
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/v{version:apiVersion}/[controller]")]
    public partial class IntegrationVersionContainer2Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public IntegrationVersionContainer2Controller(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Save IntegrationVersionContainer2
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveIntegrationVersionContainer2")]
        public virtual WorkflowSampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveIntegrationVersionContainer2Internal(integrationVersionContainer2, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2Internal(WorkflowSampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IIntegrationVersionContainer2BLL bll = evaluateData.Context.Logics.IntegrationVersionContainer2;
            Framework.DomainDriven.BLL.Security.AuthorizationBLLContextExtensions.CheckAccess(evaluateData.Context.Authorization, WorkflowSampleSystem.BLL.WorkflowSampleSystemSecurityOperation.SystemIntegration);
            return this.SaveIntegrationVersionContainer2Internal(integrationVersionContainer2, evaluateData, bll);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2Internal(WorkflowSampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData, WorkflowSampleSystem.BLL.IIntegrationVersionContainer2BLL bll)
        {
            WorkflowSampleSystem.Domain.IntergrationVersions.IntegrationVersionContainer2 domainObject = bll.GetById(integrationVersionContainer2.Id, false, null, Framework.DomainDriven.LockRole.Update);
            if (object.ReferenceEquals(domainObject, null))
            {
                domainObject = new WorkflowSampleSystem.Domain.IntergrationVersions.IntegrationVersionContainer2();
            }
            if ((domainObject.IntegrationVersion <= integrationVersionContainer2.IntegrationVersion))
            {
                integrationVersionContainer2.MapToDomainObject(evaluateData.MappingService, domainObject);
                bll.Insert(domainObject, integrationVersionContainer2.Id);
            }
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
