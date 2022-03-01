namespace WorkflowSampleSystem.WebApiCore.Controllers.MainQuery
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/v{version:apiVersion}/[controller]")]
    public partial class HRDepartmentQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public HRDepartmentQueryController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get HRDepartments (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartmentsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<WorkflowSampleSystem.Domain.HRDepartment> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<WorkflowSampleSystem.Domain.HRDepartment>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<WorkflowSampleSystem.Domain.HRDepartment> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.HRDepartmentFullDTO>(WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get HRDepartments (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleHRDepartmentsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<WorkflowSampleSystem.Domain.HRDepartment> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<WorkflowSampleSystem.Domain.HRDepartment>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<WorkflowSampleSystem.Domain.HRDepartment> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.HRDepartmentSimpleDTO>(WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get HRDepartments (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualHRDepartmentsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<WorkflowSampleSystem.Domain.HRDepartment> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<WorkflowSampleSystem.Domain.HRDepartment>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<WorkflowSampleSystem.Domain.HRDepartment> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.HRDepartmentVisualDTO>(WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
