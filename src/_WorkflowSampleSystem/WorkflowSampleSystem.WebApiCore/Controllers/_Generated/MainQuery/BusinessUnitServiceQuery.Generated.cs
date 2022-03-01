namespace WorkflowSampleSystem.WebApiCore.Controllers.MainQuery
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/v{version:apiVersion}/[controller]")]
    public partial class BusinessUnitQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public BusinessUnitQueryController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get BusinessUnits (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<WorkflowSampleSystem.Domain.BusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<WorkflowSampleSystem.Domain.BusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<WorkflowSampleSystem.Domain.BusinessUnit> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO>(WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get BusinessUnits (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<WorkflowSampleSystem.Domain.BusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<WorkflowSampleSystem.Domain.BusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<WorkflowSampleSystem.Domain.BusinessUnit> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO>(WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get BusinessUnits (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<WorkflowSampleSystem.Domain.BusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<WorkflowSampleSystem.Domain.BusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<WorkflowSampleSystem.Domain.BusinessUnit> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO>(WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
